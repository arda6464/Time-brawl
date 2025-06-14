﻿namespace Supercell.Laser.Logic.Message.Ranking
{
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Helper;
    using Supercell.Laser.Logic.Home;

    public class LeaderboardMessage : GameMessage
    {
        public int LeaderboardType { get; set; }
        public int CharachterId { get; set; }

        public List<KeyValuePair<ClientHome, ClientAvatar>> Avatars;
        public Dictionary<ClientHome, ClientAvatar> Brawlers;
        public List<Alliance> AllianceList;
        public long OwnAvatarId;
        public string Region { get; set; }

        public LeaderboardMessage() : base()
        {
            Avatars = new List<KeyValuePair<ClientHome, ClientAvatar>>();
            AllianceList = new List<Alliance>();
            LeaderboardType = 1;
        }

        public override void Encode()
        {
            int playerIndex = 0;

            Stream.WriteVInt(LeaderboardType);
            Stream.WriteVInt(2);
            ByteStreamHelper.WriteDataReference(Stream, CharachterId);
            Stream.WriteString(Region); // Region

            if (LeaderboardType == 1)
            {
                Stream.WriteVInt(Avatars.Count);
                foreach (var pair in Avatars)
                {
                    var home = pair.Key;
                    var avatar = pair.Value;
                    if (avatar.AccountId == OwnAvatarId)
                    {
                        playerIndex = Avatars.IndexOf(pair) + 1;
                    }

                    Stream.WriteVLong(avatar.AccountId);

                    Stream.WriteVInt(1);
                    Stream.WriteVInt(avatar.Trophies);

                    Stream.WriteVInt(1);

                    Stream.WriteString(avatar.AllianceName); // Club name

                    Stream.WriteString(avatar.Name ?? "Brawler");
                    Stream.WriteVInt(100);
                    Stream.WriteVInt(home.ThumbnailId);
                    Stream.WriteVInt(home.NameColorId);
                    Stream.WriteVInt(home.HasPremiumPass || (home.NameColorId == 43000012) ? home.NameColorId : 0);
                    Stream.WriteVInt(0);
                }
            }
            else if (LeaderboardType == 2)
            {
                Stream.WriteVInt(AllianceList.Count);
                foreach (var alliance in AllianceList)
                {
                    Stream.WriteVLong(alliance.Id);

                    Stream.WriteVInt(1);
                    Stream.WriteVInt(alliance.Trophies);

                    Stream.WriteVInt(2);

                    Stream.WriteString(alliance.Name);
                    Stream.WriteVInt(alliance.Members.Count);
                    ByteStreamHelper.WriteDataReference(Stream, alliance.AllianceBadgeId);
                }
            }
            else if (LeaderboardType == 0)
            {
                Stream.WriteVInt(Brawlers.Count);
                foreach (KeyValuePair<ClientHome, ClientAvatar> pair in Brawlers)
                {
                    var home = pair.Key;
                    var avatar = pair.Value;
                    if (avatar.AccountId == OwnAvatarId)
                    {
                        playerIndex = Avatars.IndexOf(pair) + 1;
                    }
                    Stream.WriteVLong(avatar.AccountId);

                    Stream.WriteVInt(1);
                    Stream.WriteVInt(avatar.GetHero(CharachterId).Trophies);

                    Stream.WriteVInt(1);

                    Stream.WriteString(avatar.AllianceName); // Club name

                    Stream.WriteString(avatar.Name ?? "Brawler");
                    Stream.WriteVInt(100);
                    Stream.WriteVInt(home.ThumbnailId);
                    Stream.WriteVInt(home.NameColorId);
                    Stream.WriteVInt(home.HasPremiumPass ? home.NameColorId : 0);
                    Stream.WriteVInt(0);
                }
            }
            else if (LeaderboardType == 3)
            {
                Stream.WriteVInt(Avatars.Count);
                foreach (var pair in Avatars)
                {
                    var home = pair.Key;
                    var avatar = pair.Value;
                    if (avatar.AccountId == OwnAvatarId)
                    {
                        playerIndex = Avatars.IndexOf(pair) + 1;
                    }

                    Stream.WriteVLong(avatar.AccountId);

                    Stream.WriteVInt(1);
                    Stream.WriteVInt(22);

                    Stream.WriteVInt(3);

                    Stream.WriteString(avatar.AllianceName); // Club name

                    Stream.WriteString(avatar.Name ?? "Brawler");
                    Stream.WriteVInt(100);
                    Stream.WriteVInt(home.ThumbnailId);
                    Stream.WriteVInt(home.NameColorId);
                    Stream.WriteVInt(home.HasPremiumPass ? home.NameColorId : 0);
                    Stream.WriteVInt(0);
                }
            }
            else
            {
                Stream.WriteVInt(0);
            }

            Stream.WriteVInt(0);
            Stream.WriteVInt(playerIndex);
            Stream.WriteVInt(0);
            Stream.WriteVInt(0);
            Stream.WriteString("US");
        }

        public override int GetMessageType()
        {
            return 24403;
        }

        public override int GetServiceNodeType()
        {
            return 9;
        }
    }
}
