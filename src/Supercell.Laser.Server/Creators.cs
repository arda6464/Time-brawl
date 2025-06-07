using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Supercell.Laser.Server
{
    public static class Creators
    {
        private static readonly object _lock = new object();
        private static string FilePath { get; }
        private static List<Creator> CreatorList { get; set; }

        static Creators()
        {
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "creators.json");
            LoadCreators();
        }

        private static void LoadCreators()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    CreatorList = JsonConvert.DeserializeObject<List<Creator>>(json) ?? new List<Creator>();
                }
                else
                {
                    CreatorList = new List<Creator>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading creators: {ex.Message}");
                CreatorList = new List<Creator>(); // Fallback to an empty list
            }
        }

        private static void SaveCreators()
        {
            try
            {
                var json = JsonConvert.SerializeObject(CreatorList, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving creators: {ex.Message}");
            }
        }

        public static void AddCreatorByName(string creatorName, long id)
        {
            lock (_lock)
            {
                if (!CreatorExists(creatorName))
                {
                    CreatorList.Add(new Creator { Name = creatorName, Id = id, UsageCount = 0 });
                    SaveCreators();
                }
            }
        }

        public static void DeleteCreatorByName(string creatorName)
        {
            lock (_lock)
            {
                CreatorList.RemoveAll(c => c.Name.Equals(creatorName, StringComparison.OrdinalIgnoreCase));
                SaveCreators();
            }
        }

        public static void IncreaseCreator(string creatorName)
        {
            lock (_lock)
            {
                var creator = CreatorList.Find(c => c.Name.Equals(creatorName, StringComparison.OrdinalIgnoreCase));
                if (creator != null)
                {
                    creator.UsageCount++;
                    SaveCreators();
                }
            }
        }

        public static void ReduceCreator(string creatorName)
        {
            lock (_lock)
            {
                var creator = CreatorList.Find(c => c.Name.Equals(creatorName, StringComparison.OrdinalIgnoreCase));
                if (creator != null && creator.UsageCount > 0)
                {
                    creator.UsageCount--;
                    SaveCreators();
                }
            }
        }

        public static List<Creator> GetAllCreators()
        {
            return CreatorList;
        }

        public static bool CreatorExists(string creatorName)
        {
            if (string.IsNullOrEmpty(creatorName)) return false;
            return CreatorList.Exists(c => c.Name.Equals(creatorName, StringComparison.OrdinalIgnoreCase));
        }

        public static string CreatorInfoByName(string creatorName)
        {
            var creator = CreatorList.Find(c => c.Name.Equals(creatorName, StringComparison.OrdinalIgnoreCase));
            return creator != null ? $"Name: {creator.Name}, ID: {creator.Id}, Usage Count: {creator.UsageCount}" : "Creator not found.";
        }

        public static long GetCreatorIdByName(string creatorName)
        {
            var creator = CreatorList.Find(c => c.Name.Equals(creatorName, StringComparison.OrdinalIgnoreCase));
            return creator?.Id ?? -1; // Return -1 if creator is not found
        }

        public class Creator
        {
            public string Name { get; set; }
            public long Id { get; set; }
            public int UsageCount { get; set; }
        }
    }
}