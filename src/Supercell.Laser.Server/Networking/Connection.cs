namespace Supercell.Laser.Server.Networking
{
    using System;
    using System.Net.Sockets;
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Home;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Message;
    using System.Net;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    using System.Security.Cryptography;
    using System.Text;
    using System.Collections.Generic;

    public class Connection
    {
        public Messaging Messaging { get; }
        public MessageManager MessageManager { get; }
        public byte[] ReadBuffer { get; }
        public Socket Socket { get; }

        public int Ping { get; private set; }

        public MemoryStream Memory { get; set; }
        public bool IsOpen;

        public int MatchmakeSlot;
        public MatchmakingEntry MatchmakingEntry;

        public long UdpSessionId;

        private readonly Socket _socket;
        private readonly byte[] _buffer;
        private readonly ConcurrentQueue<byte[]> _sendQueue;
        private readonly IPEndPoint _remoteEndPoint;
        private readonly DateTime _connectionTime;
        private readonly string _connectionId;
        private int _packetCount;
        private DateTime _lastPacketTime;
        private bool _isAuthenticated;
        private readonly RateLimiter _rateLimiter;

        public ClientHome Home
        {
            get
            {
                if (MessageManager.HomeMode != null)
                {
                    return MessageManager.HomeMode.Home;
                }
                return null;
            }
        }

        public ClientAvatar Avatar
        {
            get
            {
                if (MessageManager.HomeMode != null)
                {
                    return MessageManager.HomeMode.Avatar;
                }
                return null;
            }
        }

        public RateLimiter RateLimiter { get; private set; }
        public string DeviceId { get; private set; }

        public Connection(Socket socket)
        {
            _socket = socket;
            _buffer = new byte[8192];
            _sendQueue = new ConcurrentQueue<byte[]>();
            _remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            _connectionTime = DateTime.UtcNow;
            _connectionId = GenerateConnectionId();
            _packetCount = 0;
            _lastPacketTime = DateTime.UtcNow;
            _isAuthenticated = false;
            _rateLimiter = new RateLimiter(100); // 100 paket/saniye limit

            // Güvenlik ayarları
            _socket.NoDelay = true;
            _socket.ReceiveBufferSize = 8192;
            _socket.SendBufferSize = 8192;
            _socket.ReceiveTimeout = 30000; // 30 saniye
            _socket.SendTimeout = 30000; // 30 saniye

            Socket = socket;
            ReadBuffer = new byte[1024];

            Memory = new MemoryStream();

            Messaging = new Messaging(this);
            MessageManager = new MessageManager(this);

            IsOpen = true;
            MatchmakeSlot = -1;

            UdpSessionId = -1;

            RateLimiter = new RateLimiter();
            DeviceId = null;
        }

        private string GenerateConnectionId()
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{_remoteEndPoint.Address}:{_remoteEndPoint.Port}:{DateTime.UtcNow.Ticks}"));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 16);
            }
        }

        public async Task StartAsync()
        {
            try
            {
                while (_socket.Connected)
                {
                    if (!_rateLimiter.CheckLimit())
                    {
                        await Task.Delay(100); // Rate limit aşıldığında bekle
                        continue;
                    }

                    var bytesRead = await _socket.ReceiveAsync(new ArraySegment<byte>(_buffer), SocketFlags.None);
                    if (bytesRead == 0) break;

                    _packetCount++;
                    _lastPacketTime = DateTime.UtcNow;

                    // Paket doğrulama
                    if (!ValidatePacket(_buffer, bytesRead))
                    {
                        Logger.Error($"Invalid packet received from {_remoteEndPoint}");
                        continue;
                    }

                    // Paket işleme
                    await ProcessPacketAsync(_buffer, bytesRead);
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Connection error for {_remoteEndPoint}: {ex.Message}");
            }
            finally
            {
                Close();
            }
        }

        private bool ValidatePacket(byte[] data, int length)
        {
            // Paket boyutu kontrolü
            if (length < 7 || length > 8192) return false;

            // Paket imza kontrolü
            if (!VerifyPacketSignature(data, length)) return false;

            // Paket sıra numarası kontrolü
            if (!ValidatePacketSequence(data)) return false;

            return true;
        }

        private bool VerifyPacketSignature(byte[] data, int length)
        {
            // TODO: Implement packet signature verification
            return true;
        }

        private bool ValidatePacketSequence(byte[] data)
        {
            // TODO: Implement packet sequence validation
            return true;
        }

        public async Task SendAsync(byte[] data)
        {
            if (!_socket.Connected) return;

            try
            {
                _sendQueue.Enqueue(data);
                await ProcessSendQueueAsync();
            }
            catch (Exception ex)
            {
                Logger.Error($"Send error for {_remoteEndPoint}: {ex.Message}");
                Close();
            }
        }

        private async Task ProcessSendQueueAsync()
        {
            while (_sendQueue.TryDequeue(out byte[] data))
            {
                try
                {
                    await _socket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Queue processing error for {_remoteEndPoint}: {ex.Message}");
                    return;
                }
            }
        }

        private async Task ProcessPacketAsync(byte[] data, int length)
        {
            // TODO: Implement packet processing logic
            await Task.CompletedTask;
        }

        public void Close()
        {
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"Close error for {_remoteEndPoint}: {ex.Message}");
            }
        }

        public void PingUpdated(int value)
        {
            Ping = value;
        }

        public void Send(GameMessage message)
        {
            Messaging.Send(message);
        }

        public void Write(byte[] stream)
        {
            try
            {
                Socket.BeginSend(stream, 0, stream.Length, SocketFlags.None, new AsyncCallback(TCPGateway.OnSend), Socket);
            }
            catch (Exception) { }
        }

        public void SetDeviceId(string deviceId)
        {
            DeviceId = deviceId;
        }
    }
}
