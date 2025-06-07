﻿namespace Supercell.Laser.Logic.Home.Items
{
    using System.Linq;
    using Newtonsoft.Json;
    using Supercell.Laser.Titan.DataStream;

    public class NotificationFactory
    {
        [JsonProperty("notification_list")]
        public List<Notification> NotificationList;

        public NotificationFactory()
        {
            NotificationList = new List<Notification>();
        }

        public void Add(Notification notification)
        {
            notification.Index = GetIndex();
            NotificationList.Add(notification);
        }

        public int GetIndex()
        {
            return NotificationList.Count;
        }

        public void Encode(ByteStream stream)
        {
            stream.WriteVInt(NotificationList.Count);
            foreach (Notification notification in NotificationList.AsEnumerable().Reverse()) // reverse notification list to show most recent first
            {
                notification.Encode(stream);
            }
        }
    }
}