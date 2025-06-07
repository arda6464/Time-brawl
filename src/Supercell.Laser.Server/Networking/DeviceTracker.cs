using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Supercell.Laser.Server.Networking
{
    public class DeviceInfo
    {
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string OperatingSystem { get; set; }
        public string AppVersion { get; set; }
        public string Language { get; set; }
        public string Region { get; set; }
        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
        public int ConnectionCount { get; set; }
        public List<string> AssociatedAccounts { get; set; }
        public Dictionary<string, int> LoginAttempts { get; set; }
        public bool IsSuspicious { get; set; }
        public string SuspiciousReason { get; set; }
        public Dictionary<string, object> AdditionalInfo { get; set; }

        public DeviceInfo()
        {
            AssociatedAccounts = new List<string>();
            LoginAttempts = new Dictionary<string, int>();
            AdditionalInfo = new Dictionary<string, object>();
            FirstSeen = DateTime.UtcNow;
            LastSeen = DateTime.UtcNow;
        }
    }

    public static class DeviceTracker
    {
        private static readonly ConcurrentDictionary<string, DeviceInfo> _devices = new ConcurrentDictionary<string, DeviceInfo>();
        private static readonly string _logFilePath = "device_logs.json";
        private static readonly object _saveLock = new object();
        private static readonly int _maxLoginAttempts = 5;
        private static readonly TimeSpan _loginAttemptWindow = TimeSpan.FromMinutes(15);

        public static void TrackDevice(string deviceId, string deviceModel, string os, string appVersion, string language, string region, string accountId = null)
        {
            var device = _devices.GetOrAdd(deviceId, _ => new DeviceInfo
            {
                DeviceId = deviceId,
                DeviceModel = deviceModel,
                OperatingSystem = os,
                AppVersion = appVersion,
                Language = language,
                Region = region
            });

            device.LastSeen = DateTime.UtcNow;
            device.ConnectionCount++;

            if (!string.IsNullOrEmpty(accountId) && !device.AssociatedAccounts.Contains(accountId))
            {
                device.AssociatedAccounts.Add(accountId);
            }

            // Şüpheli aktivite kontrolü
            CheckSuspiciousActivity(device);

            // Log kaydı
            LogDeviceActivity(device);
        }

        public static bool IsDeviceSuspicious(string deviceId)
        {
            if (_devices.TryGetValue(deviceId, out DeviceInfo device))
            {
                return device.IsSuspicious;
            }
            return false;
        }

        public static void TrackLoginAttempt(string deviceId, string accountId, bool success)
        {
            if (_devices.TryGetValue(deviceId, out DeviceInfo device))
            {
                var now = DateTime.UtcNow;
                var key = $"{accountId}_{now:yyyyMMddHH}";

                if (!device.LoginAttempts.ContainsKey(key))
                {
                    device.LoginAttempts[key] = 0;
                }

                if (!success)
                {
                    device.LoginAttempts[key]++;
                }

                // Eski giriş denemelerini temizle
                var oldKeys = device.LoginAttempts.Keys
                    .Where(k => DateTime.ParseExact(k.Split('_')[1], "yyyyMMddHH", null) < now - _loginAttemptWindow)
                    .ToList();

                foreach (var oldKey in oldKeys)
                {
                    device.LoginAttempts.Remove(oldKey);
                }

                // Şüpheli aktivite kontrolü
                CheckSuspiciousActivity(device);
            }
        }

        private static void CheckSuspiciousActivity(DeviceInfo device)
        {
            var suspiciousReasons = new List<string>();

            // Çok fazla hesap kontrolü
            if (device.AssociatedAccounts.Count > 3)
            {
                suspiciousReasons.Add($"Too many associated accounts: {device.AssociatedAccounts.Count}");
            }

            // Başarısız giriş denemeleri kontrolü
            var recentFailedAttempts = device.LoginAttempts.Values.Sum();
            if (recentFailedAttempts >= _maxLoginAttempts)
            {
                suspiciousReasons.Add($"Too many failed login attempts: {recentFailedAttempts}");
            }

            // Hızlı bağlantı değişiklikleri kontrolü
            if (device.ConnectionCount > 100 && (DateTime.UtcNow - device.FirstSeen).TotalHours < 1)
            {
                suspiciousReasons.Add("Suspicious connection pattern");
            }

            // Şüpheli durumu güncelle
            device.IsSuspicious = suspiciousReasons.Any();
            if (device.IsSuspicious)
            {
                device.SuspiciousReason = string.Join(", ", suspiciousReasons);
                Logger.Warning($"Suspicious device detected: {device.DeviceId} - {device.SuspiciousReason}");
            }
        }

        private static void LogDeviceActivity(DeviceInfo device)
        {
            try
            {
                var logEntry = new
                {
                    Timestamp = DateTime.UtcNow,
                    DeviceId = device.DeviceId,
                    DeviceModel = device.DeviceModel,
                    OS = device.OperatingSystem,
                    AppVersion = device.AppVersion,
                    Region = device.Region,
                    ConnectionCount = device.ConnectionCount,
                    AssociatedAccounts = device.AssociatedAccounts.Count,
                    IsSuspicious = device.IsSuspicious,
                    SuspiciousReason = device.SuspiciousReason
                };

                var logLine = JsonSerializer.Serialize(logEntry);
                lock (_saveLock)
                {
                    File.AppendAllText(_logFilePath, logLine + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error logging device activity: {ex.Message}");
            }
        }

        public static DeviceInfo GetDeviceInfo(string deviceId)
        {
            _devices.TryGetValue(deviceId, out DeviceInfo device);
            return device;
        }

        public static List<DeviceInfo> GetSuspiciousDevices()
        {
            return _devices.Values.Where(d => d.IsSuspicious).ToList();
        }

        public static Dictionary<string, int> GetDeviceStats()
        {
            return new Dictionary<string, int>
            {
                ["Total Devices"] = _devices.Count,
                ["Suspicious Devices"] = _devices.Count(d => d.Value.IsSuspicious),
                ["Active Devices (Last 24h)"] = _devices.Count(d => (DateTime.UtcNow - d.Value.LastSeen).TotalHours <= 24),
                ["Multi-Account Devices"] = _devices.Count(d => d.Value.AssociatedAccounts.Count > 1)
            };
        }

        public static void SaveDeviceData()
        {
            try
            {
                var data = JsonSerializer.Serialize(_devices, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("device_data.json", data);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error saving device data: {ex.Message}");
            }
        }

        public static void LoadDeviceData()
        {
            try
            {
                if (File.Exists("device_data.json"))
                {
                    var data = File.ReadAllText("device_data.json");
                    var devices = JsonSerializer.Deserialize<ConcurrentDictionary<string, DeviceInfo>>(data);
                    foreach (var device in devices)
                    {
                        _devices.TryAdd(device.Key, device.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Error loading device data: {ex.Message}");
            }
        }
    }
} 