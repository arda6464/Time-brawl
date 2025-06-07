﻿namespace Supercell.Laser.Server.Logic.Game
{
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Server.Database.Models;

    public static class Leaderboards
    {
        private static List<Account> Accounts;
        private static List<Alliance> Alliances;
        private static Dictionary<int, List<Account>> Brawlers;
        private static Thread Thread;

        public static void Init()
        {
            Accounts = new List<Account>();
            Alliances = new List<Alliance>();
            Brawlers = new Dictionary<int, List<Account>>();

            Thread = new Thread(Update);
            Thread.Start();
        }

        public static Account[] GetAvatarRankingList()
        {
            return Accounts.ToArray();
        }

        public static Dictionary<int, List<Account>> GetBrawlersRankingList()
        {
            return Brawlers;
        }

        public static Alliance[] GetAllianceRankingList()
        {
            return Alliances.ToArray();
        }

        private static void Update()
        {
            while (true)
            {
                Accounts = Database.Accounts.GetRankingList();
                Alliances = Database.Alliances.GetRankingList();
                Brawlers = Database.Accounts.GetBrawlersRankingList();
            }
        }
    }
}
