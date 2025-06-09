    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Numerics;
    using System.Reflection;  
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Supercell.Laser.Server; 
    using Newtonsoft.Json.Linq;
    using MySql.Data.MySqlClient;
    using NetCord.Services.Commands;
    using Supercell.Laser.Logic.Avatar;
    using Supercell.Laser.Logic.Avatar.Structures;
    using Supercell.Laser.Logic.Battle;
    using Supercell.Laser.Logic.Battle.Structures;
    using Supercell.Laser.Logic.Club;
    using Supercell.Laser.Logic.Command;
    using Supercell.Laser.Logic.Command.Avatar;
    using Supercell.Laser.Logic.Command.Home;
    using Supercell.Laser.Logic.Data;
    using Supercell.Laser.Logic.Data.Helper;
    using Supercell.Laser.Logic.Friends;
    using Supercell.Laser.Logic.Home; 
    using Supercell.Laser.Logic.Home.Items;
    using Supercell.Laser.Logic.Home.Quest;
    using Supercell.Laser.Logic.Home.Structures;
    using Supercell.Laser.Logic.Listener;
    using Supercell.Laser.Logic.Message;
    using Supercell.Laser.Server.Networking.Security;
    using Supercell.Laser.Logic.Message.Account;
    using Supercell.Laser.Logic.Message.Account.Auth;
    using Supercell.Laser.Logic.Message.Battle;
    using Supercell.Laser.Logic.Message.Club;
    using Supercell.Laser.Logic.Message.Friends;
    using Supercell.Laser.Logic.Message.Home;
    using Supercell.Laser.Logic.Message.Latency;
    using Supercell.Laser.Logic.Message.Ranking;
    using Supercell.Laser.Logic.Message.Security;
    using Supercell.Laser.Logic.Message.Team;
    using Supercell.Laser.Logic.Message.Team.Stream;
    using Supercell.Laser.Logic.Stream.Entry;
    using Supercell.Laser.Logic.Team;
    using Supercell.Laser.Logic.Team.Stream;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Server.Utils;
    using Supercell.Laser.Server.Database;
    using Supercell.Laser.Server.Database.Cache;
    using Supercell.Laser.Server.Database.Models;
    using Supercell.Laser.Server.Logic.Game;
    using Supercell.Laser.Server.Networking;

    using Supercell.Laser.Server.Networking.Session;
    using Supercell.Laser.Server.Networking.UDP.Game;
    using Supercell.Laser.Server.Settings;
  

   // public class DatabaseHelper
   //   {
       //  private static string GetConnectionString()
        // {
           //  return $"server=127.0.0.1;"
              //   + $"user={Configuration.Instance.DatabaseUsername};"
               //  + $"database={Configuration.Instance.DatabaseName};"
                // + $"port=3306;"
                // + $"password={Configuration.Instance.DatabasePassword}";
         //}

      //   public static string ExecuteScalar(string query, params (string, object)[] parameters)
         //{
            // try
             //{
                // using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                // {
                  //   connection.Open();

               //      MySqlCommand cmd = new MySqlCommand(query, connection);
                  //   foreach (var (paramName, paramValue) in parameters)
                    // {
                     //    cmd.Parameters.AddWithValue(paramName, paramValue);
                     //}

                 //    object result = cmd.ExecuteScalar();
                    // return result?.ToString() ?? "N/A";
                 //}
            // }
            // catch (Exception ex)
             //{
                // return $"Error: {ex.Message}";
             //}
        // }

         //public static bool ExecuteNonQuery(string query, params (string, object)[] parameters)
         //{
            // try
             //{
                // using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                 //{
                    // connection.Open();

                     //MySqlCommand cmd = new MySqlCommand(query, connection);
                     //foreach (var (paramName, paramValue) in parameters)
                     //{
                        // cmd.Parameters.AddWithValue(paramName, paramValue);
                    // }

                     //int rowsAffected = cmd.ExecuteNonQuery();
                 //    return rowsAffected > 0;
                 //}
             //}
             //catch (Exception ex)
             //{
               //  Console.WriteLine($"Error: {ex.Message}");
                // return false;
             //}
         //}
     //}


  
public static class WebhookHelper // eğer Discord webhook kullanmak istemiyorsanız bu sınıfı yorum satırı haline getirin
{
    private static readonly string WebhookUrl = "https://discord.com/api/webhooks/1324821140069416960/RBn5ZcA581XFof69gYD1YHY2mAOaaY5vl8gSg1OoGeRZh_HIzm-zO3_6-d_SwrWjyZQH";

    public static void SendNotification(string message)
    {
        using (var client = new HttpClient())
        {
            var content = new StringContent(
                $"{{\"content\": \"{message}\"}}",
                Encoding.UTF8,
                "application/json"
            );

            var response = client.PostAsync(WebhookUrl, content).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Webhook gönderimi başarısız oldu: {response.StatusCode} {response.ReasonPhrase}"
                );
            }
        }
    }
}


// ÇALIŞMIYOR AMA İLERDE DÜZELTİRİM BENCE:d
/*public class AddTeklifCommand : CommandModule<CommandContext>
{
    // Tekliflerin ekleneceği dosyanın tam yolu
    private static readonly string FilePath = @"C:\Users\arda\Desktop\projeler\time brawl - Kopya (2)\src\Supercell.Laser.Logic\Home\ClientHome.cs";

    [Command("addteklif")]
    public string AddTeklif(string title, int durationDays, string itemType, int quantity, int price)
    {
        // Teklif stringini oluştur
        var offerString = $@"
            OfferBundle bundle = new OfferBundle();
            bundle.Title = ""{title}"";
            bundle.EndTime = DateTime.UtcNow.AddDays({durationDays});
            bundle.BackgroundExportName = ""offer_custom"";
            Offer item = new Offer(ShopItem.{itemType}, {quantity});
            bundle.Items.Add(item);
            bundle.Cost = {price};
            bundle.Currency = 0;
            OfferBundles.Add(bundle);
        ";

        // Teklifi dosyaya yazma işlemi
        try
        {
            if (File.Exists(FilePath))
            {
                // Dosyanın sonuna teklifi ekle
                File.AppendAllText(FilePath, offerString + Environment.NewLine);

                return $"Yeni teklif başarıyla eklendi ve `ClientHome.cs` dosyasına yazıldı:\n" +
                       $"- Başlık: `{title}`\n" +
                       $"- Süre: {durationDays} gün\n" +
                       $"- Ürün: {quantity}x {itemType}\n" +
                       $"- Fiyat: {price}";
            }
            else
            {
                return "Hata: ClientHome.cs dosyası bulunamadı. Lütfen dosya yolunu kontrol edin.";
            }
        }
        catch (Exception ex)
        {
            return $"Hata: Teklif eklenirken bir sorun oluştu. ({ex.Message})";
        }
    }
}*/






    public class Ping : CommandModule<CommandContext>
    {
        [Command("ping")]
        public static string Pong() => "Pong!";
    }

    public class Deleteclub : CommandModule<CommandContext> // çalışıyor mu hatırlamıyorum sanırım çalışmıyordu
    {
        [Command("deleteclub")]
        public static string DeleteClub([CommandParameter(Remainder = true)] string clubTag)
        {
            if (!clubTag.StartsWith("#"))
            {
                return "Invalid club tag. Make sure it starts with '#'.";
            }

            long clubId = LogicLongCodeGenerator.ToId(clubTag);

            bool success = DatabaseHelper.ExecuteNonQuery(
                "DELETE FROM alliances WHERE Id = @id",
                ("@id", clubId)
            );

                 WebhookHelper.SendNotification(
            $"Oyuncu **{clubTag}** idli kulüp silindi. "
        );


            if (success)
            {
                return $"{clubTag} etiketli kulüp başarıyla silindi.";
            }
            else
            {
                return $"{clubTag} etiketli kulüp silinemedi. Lütfen etiketi kontrol edip tekrar deneyin.";
            }
        }
    }

    public class Help : CommandModule<CommandContext>
    {
        [Command("yardım")]
        public static string help()
        {
            return "# KULLANILABİLİR KOMUTLAR\n"
                + "!yardım - bu komutu gösterir\n"
                + "!status - sunucu durumunu gösterir\n"
                + "!startevent (tag) - Mevcut etkinlik var ise başlatır\n"
                + "!event (tag) - mevcut ilerlemenizi gösterir\n"
                + "!kayıt (tag) - hesabını discord'a kaydeder \n"
                + "!hesabım (tag) - hesabına bakabilirsin\n";
        }
    }

      public class Admin : CommandModule<CommandContext>
    {
        [Command("admin")]
        public static string help()
        {
            return "# Available Commands:\n"
                + "!help - shows all available commands\n"
                + "!status - show server status\n"
                + "!ping - will respond with pong\n"
                + "!unlockall - will unlock EVERYTHING on the players account (!unlockall [TAG])\n"
                + "!ban - ban an account (!ban [TAG])\n"
                + "!unban - unban an account (!unban [TAG])\n"
                + "!mute - mute a player (!mute [TAG])\n"
                + "!unmute - unmute a player (!unmute [TAG])\n"
                + "!resetseason - resets the season, duh\n"
                + "!reports - sends a link to all reported messages\n"
                + "!userinfo - show player info (!userinfo [TAG])\n"
                + "!changecredentials - change username/password of an account (!changecredentials [TAG] [newUsername] [newPassword])\n"
                + "!settrophies - set trophies of all brawlers (!settrophies [TAG] [Trophies])\n"
                + "!addgems - grant gems to a player (!addgems [TAG] [DonationCount])\n";
        }
    }


/* public class ClubRename : CommandModule<CommandContext>
 {
     [Command("kulüpadı")]
     public static string RenameClub([CommandParameter] string tag, [CommandParameter] string newName)
     {
         if (!tag.StartsWith("#"))
         {
             return "Geçersiz kulüp etiketi. Lütfen '#' ile başladığından emin olun.";
         }

         long clubId = LogicLongCodeGenerator.ToId(tag);
         Allience = Clubs.Get(clubId);

         if (club == null)
         {
             return $"Kulüp bulunamadı: {tag}.";
         }

         club.Name = newName;
         Clubs.Save(club);

         WebhookHelper.SendNotification(
             $"Kulüp **{tag}** adlı kulübün adı **{newName}** olarak değiştirildi."
         );

         return $"Kulüp adı başarıyla değiştirildi: {newName}";
     }
 }*/

  public class SendCustomMessage : CommandModule<CommandContext> // test edilmedi!!!
    {
        [Command("ozelmesaj")]
        public static string ExecuteSendCustomMessage([CommandParameter(Remainder = true)] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return "Kullanım: !ozelmesaj [mesaj]";
            }

            try
            {
                int sentCount = 0;
                var sessions = Sessions.ActiveSessions.Values.ToArray();

                foreach (var session in sessions)
                {
                    CustomMessage customMessage = new()
                    {
                        Message = message
                    };

                    session.Connection.Send(customMessage);
                    sentCount++;
                }

                WebhookHelper.SendNotification(
                    $"Özel mesaj gönderildi: {message}\nToplam {sentCount} oyuncuya iletildi."
                );

                return $"Özel mesaj başarıyla gönderildi. Toplam {sentCount} oyuncuya iletildi.";
            }
            catch (Exception ex)
            {
                return $"Bir hata oluştu: {ex.Message}";
            }
        }
    }




    public class UnbanIP : CommandModule<CommandContext>
    {
        [Command("unbanip")]
        public static string UnbanIpCommand([CommandParameter(Remainder = true)] string ipAddress)
        {
            if (!Configuration.Instance.antiddos)
            {
                return "Anti-DDoS is disabled. Enable it in config.json to use this command.";
            }

            if (!IPAddress.TryParse(ipAddress, out _))
            {
                return "Invalid IP address format.";
            }

            if (!IsIpBanned(ipAddress))
            {
                return $"IP address {ipAddress} is not banned.";
            }

            try
            {
                string[] bannedIps = File.ReadAllLines("ipblacklist.txt");
                bannedIps = bannedIps.Where(ip => ip != ipAddress).ToArray();
                File.WriteAllLines("ipblacklist.txt", bannedIps);
                return $"IP address {ipAddress} has been unbanned.";
            }
            catch (Exception ex)
            {
                return $"Failed to unban IP address: {ex.Message}";
            }
        }

        private static bool IsIpBanned(string ipAddress)
        {
            if (!File.Exists("ipblacklist.txt"))
            {
                return false;
            }

            string[] bannedIps = File.ReadAllLines("ipblacklist.txt");
            return bannedIps.Contains(ipAddress);
        }
    }






    public class RemovePremium : CommandModule<CommandContext>
    {
        [Command("removepremium")]
        public static string RemovePremiumCommand([CommandParameter(Remainder = true)] string playerId)
        {
            string[] parts = playerId.Split(' ');
            if (parts.Length != 1)
            {
                return "Kullanım: !removepremium [ETİKET]";
            }

            long id = 0;
            bool sc = false;

            if (parts[0].StartsWith('#'))
            {
                id = LogicLongCodeGenerator.ToId(parts[0]);
            }
            else
            {
                sc = true;
                if (!long.TryParse(parts[0], out id))
                {
                    return "Geçersiz oyuncu ID formatı.";
                }
            }

            Account account = Accounts.Load(id);
            if (account == null)
            {
                return $"Bu ID'ye sahip oyuncu bulunamadı: {parts[0]}.";
            }

            if (account.Home.PremiumEndTime > DateTime.UtcNow)
            {
                account.Home.PremiumEndTime = DateTime.MinValue; // Premium süresini sıfırla
                account.Avatar.PremiumLevel = 0; // Premium seviyesini sıfırla

                string d = sc ? LogicLongCodeGenerator.ToCode(id) : parts[0];

                // Webhook bildirimi gönder
                WebhookHelper.SendNotification(
                    $"Oyuncu **{d}** adlı kullanıcının premium durumu kaldırıldı."
                );

                return $"Tamam: {d} için VIP durumu kaldırıldı.";
            }
            else
            {
                return $"Hata: {parts[0]} kullanıcısının zaten aktif bir VIP durumu bulunmamaktadır.";
            }
        }
    }









    public class SendNotificationToAll : CommandModule<CommandContext>
    {
        [Command("bildirimall")]
        public static string ExecuteSendNotificationToAll([CommandParameter(Remainder = true)] string customMessage)
        {
            if (string.IsNullOrEmpty(customMessage))
            {
                return "Kullanım: !bildirimall [Mesaj]";
            }

            try
            {
                // Tüm oyuncu hesaplarını al
                var accounts = Accounts.GetRankingList();  // Bu, tüm oyuncu hesaplarını döndürecektir

                if (accounts == null || !accounts.Any())
                {
                    return "Veritabanında hiçbir oyuncu bulunamadı.";
                }

                int notifiedCount = 0;

                // Her bir hesap için bildirim gönder
                foreach (var account in accounts)
                {
                    // Bildirim oluştur
                    Notification notification = new()
                    {
                        Id = 81,  // Bildirim idsi
                        MessageEntry = customMessage // mesaj
                    };

                    // Hesabın bildirim fabrikasına bildirimi ekle
                    account.Home.NotificationFactory.Add(notification);

                    // Server komutu oluştur ve aktif oturum varsa gönder
                    LogicAddNotificationCommand notificationCommand = new() { Notification = notification };
                    AvailableServerCommandMessage commandMessage = new AvailableServerCommandMessage
                    {
                        Command = notificationCommand
                    };

                    // Eğer oyuncunun oturumu aktifse bildirimi gönder
                    if (Sessions.IsSessionActive(account.AccountId)) // account.AccountId'yi kullanıyoruz
                    {
                        var session = Sessions.GetSession(account.AccountId); // account.AccountId'yi kullanıyoruz
                        session.GameListener.SendTCPMessage(commandMessage);
                    }

                    notifiedCount++;
                }
                WebhookHelper.SendNotification(
             $"Tüm oyunculara **{customMessage}**  adlı mesaj gönderildi. "
            );
                return $"Tüm oyunculara '{customMessage}' mesajı başarıyla gönderildi. Toplamda {notifiedCount} oyuncuya bildirim gönderildi.";
            }
            catch (Exception ex)
            {
                return $"Bir hata oluştu: {ex.Message}";
            }
        }
    }















    public class LeaderboardCommand : CommandModule<CommandContext>
    {
        [Command("liderlik")]
        public static string ShowLeaderboard()
        {
            try
            {

                var accounts = Accounts.GetRankingList(); // tüm hesapları çek

                if (accounts == null || !accounts.Any())
                {
                    return "Veritabanında hiçbir oyuncu bulunamadı.";
                }

                // İlk 20 oyuncuyu al (butonlu sistem olsaydı 200'e kadar alabilirdik...)
                var top20Players = accounts.Take(20).ToList();

                // Liderlik tablosu metni oluştur
                string leaderboard = "**🏆 **Liderlik Tablosu** 🏆**\n\n";
                leaderboard += "**#**   **Oyuncu Adı**    **Kupa**    **Kulüp**\n";
                leaderboard += "--------------------------------------\n";

                for (int i = 0; i < top20Players.Count; i++)
                {
                    var account = top20Players[i];
                    string allianceName = string.IsNullOrEmpty(account.Avatar.AllianceName) ? "Yok" : account.Avatar.AllianceName;

                    leaderboard += $"**#{i + 1}**    {account.Avatar.Name.PadRight(15)}   {account.Avatar.Trophies.ToString().PadLeft(5)}      {allianceName}\n";
                }

                return leaderboard;
            }
            catch (System.Exception ex)
            {
                return $"Bir hata oluştu: {ex.Message}";
            }
        }
    }






    // buyük ihtimal çalışmıyordu

    /*public class UnlockAll : CommandModule<CommandContext>
    {
        [Command("unlockskins")]
        public static string UnlockAllCommand([CommandParameter(Remainder = true)] string playerId)
        {
            try
            {
                // Oyuncu ID'sini kontrol et
                if (!playerId.StartsWith("#"))
                {
                    return "Invalid player ID. Make sure it starts with '#'.";
                }

                // Oyuncu ID'sini dönüştür ve hesabı yükle
                long id = LogicLongCodeGenerator.ToId(playerId);
                Account account = Accounts.Load(id);

                if (account == null)
                {
                    return $"Could not find player with ID {playerId}.";
                }

                // Skin listesi tanımlanır
                List<string> skins = new()
                {
                    "Witch", "Rockstar", "Beach", "Pink", "Panda", "White", "Hair", "Gold", "Rudo",
                    "Bandita", "Rey", "Knight", "Caveman", "Dragon", "Summer", "Summertime",
                    "Pheonix", "Greaser", "GirlPrereg", "Box", "Santa", "Chef", "Boombox", "Wizard",
                    "Reindeer", "GalElf", "Hat", "Footbull", "Popcorn", "Hanbok", "Cny", "Valentine",
                    "WarsBox", "Nightwitch", "Cart", "Shiba", "GalBunny", "Ms", "GirlHotrod", "Maple",
                    "RR", "Mecha", "MechaWhite", "MechaNight", "FootbullBlue", "Outlaw", "Hogrider",
                    "BoosterDefault", "Shark", "HoleBlue", "BoxMoonFestival", "WizardRed", "Pirate",
                    "GirlWitch", "KnightDark", "DragonDark", "DJ", "Wolf", "Brown", "Total", "Sally",
                    "Leonard", "SantaRope", "Gift", "GT", "Virus", "BoosterVirus", "Gamer", "Valentines",
                    "Koala", "BearKoala", "AgentP", "Football", "Arena", "Tanuki", "Horus", "ArenaPSG",
                    "DarkBunny", "College", "Bazaar", "RedDragon", "Constructor", "Hawaii", "Barbking",
                    "Trader", "StationSummer", "Silver", "Bank", "Retro", "Ranger", "Tracksuit", "Knight",
                    "RetroAddon"
                };

                // Karakterlerin skinlerini aç
                foreach (Hero hero in account.Avatar.Heroes)
                {
                    CharacterData characterData = DataTables
                        .Get(DataType.Character)
                        .GetDataByGlobalId<CharacterData>(hero.CharacterId);

                    if (characterData != null)
                    {
                        foreach (string skinName in skins)
                        {
                            SkinData skinData = DataTables
                                .Get(DataType.Skin)
                                .GetData<SkinData>(characterData.Name + skinName);

                            if (skinData != null && !account.Home.UnlockedSkins.Contains(skinData.GetGlobalId()))
                            {
                                account.Home.UnlockedSkins.Add(skinData.GetGlobalId());
                            }
                        }
                    }
                }

                // Oturum kontrolü ve kullanıcıya bildirim
                if (Sessions.IsSessionActive(id))
                {
                    var session = Sessions.GetSession(id);
                    session.GameListener.SendTCPMessage(new AuthenticationFailedMessage
                    {
                        Message = "hesabında tüm kostümler açıldı! iyi oyunlar!"
                    });
                    Sessions.Remove(id);
                }

                return $" {playerId} ID'li oyuncunun hesabına tüm kostümler verildi";
            }
            catch (Exception ex)
            {
                return $"An error occurred while processing: {ex.Message}";
            }
        }
    }*/







    //public class Unlockskins : CommandModule<CommandContext>
    //{
    //  [Command("giveskin")]
    //public static string UnlockSkinCommand([CommandParameter] string playerId, [CommandParameter] int skinIndex)
    //{
    //  try
    // {
    // Oyuncu ID'sini kontrol et
    //    if (!playerId.StartsWith("#"))
    //   {
    //      return "Invalid player ID. Make sure it starts with '#'.";
    // }

    // Oyuncu ID'sini dönüştür ve hesabı yükle
    //   long id = LogicLongCodeGenerator.ToId(playerId);
    //  Account account = Accounts.Load(id);

    // if (account == null)
    //   {
    //     return $"Could not find player with ID {playerId}.";
    // }

    // Skins ID dosyasını yükle
    //  string filePath = "skinsid.txt"; // Dosya yolu
    // if (!File.Exists(filePath))
    //  {
    //    return $"The file '{filePath}' could not be found.";
    // }

    // Dosyadaki skinleri yükle
    //      List<string> skinList = new();
    //    foreach (string line in File.ReadAllLines(filePath))
    //  {
    //    if (string.IsNullOrWhiteSpace(line) || !line.Contains("=")) continue;

    //  string[] parts = line.Split('=');
    //    if (parts.Length == 2)
    //  {
    //    string skinName = parts[1].Trim();
    //  skinList.Add(skinName);
    //   }
    // }

    // if (skinList.Count < skinIndex || skinIndex <= 0)
    //  {
    ////     return $"Skin index {skinIndex} is out of range. Valid indexes are 1-{skinList.Count}.";
    //}

    // İstenilen skin'i aç
    //    string selectedSkin = skinList[skinIndex - 1]; // skinIndex 1'den başladığı için -1 yapıyoruz

    //  foreach (Hero hero in account.Avatar.Heroes)
    // {
    //   CharacterData characterData = DataTables
    //     .Get(DataType.Character)
    //   .GetDataByGlobalId<CharacterData>(hero.CharacterId);
    //
    //              if (characterData != null)
    //            {
    //              SkinData skinData = DataTables
    //                .Get(DataType.Skin)
    //////              .GetData<SkinData>(characterData.Name + selectedSkin);

    //    if (skinData != null)
    ////  {
    //  Console.WriteLine($"Trying to unlock skin: {selectedSkin}, Skin ID: {skinData.GetGlobalId()}");

    //if (!account.Home.UnlockedSkins.Contains(skinData.GetGlobalId()))
    //   {
    //                account.Home.UnlockedSkins.Add(skinData.GetGlobalId());
    //              Console.WriteLine($"Skin added to UnlockedSkins list: {skinData.GetGlobalId()}");

    // Hesap kaydet
    //            account.Save();
    ////      }
    //  else
    //    {
    //      Console.WriteLine($"Skin already unlocked: {selectedSkin}");
    //    }
    //  }
    //           else
    //         {
    //           Console.WriteLine($"Skin not found for {characterData.Name} + {selectedSkin}");
    //     }
    //     }
    //     }

    // Oturum kontrolü ve kullanıcıya bildirim
    //   if (Sessions.IsSessionActive(id))
    //    {
    //     var session = Sessions.GetSession(id);
    //      session.GameListener.SendTCPMessage(new AuthenticationFailedMessage
    //  {
    //        Message = $"Tebrikler! '{selectedSkin}' kostümü açıldı!"
    //});
    //       Sessions.Remove(id);
    // }

    // r//eturn $"{playerId} ID'li oyuncuya '{selectedSkin}' kostümü başarıyla eklendi.";
    // }
    //   catch (Exception ex)
    //   {
    //     return $"An error occurred while processing: {ex.Message}";
    //   }
    //   }
    //}














    public class ChangeName : CommandModule<CommandContext>
    {
        [Command("isimdegistir")]
        public static string ChangeNameCommand(
            [CommandParameter] string tag,
            [CommandParameter(Remainder = true)] string newName
        )
        {
            if (string.IsNullOrWhiteSpace(tag) || string.IsNullOrWhiteSpace(newName))
            {
                return "Kullanım: !isimdegistir [TAG] [YENİ İSİM]";
            }

            long id;
            if (!tag.StartsWith('#') || (id = LogicLongCodeGenerator.ToId(tag)) == 0)
            {
                return "Geçersiz oyuncu TAG formatı.";
            }

            Account account = Accounts.Load(id);
            if (account == null)
            {
                return $"Bu TAG'e sahip oyuncu bulunamadı: {tag}.";
            }

            account.Avatar.Name = newName;
            Accounts.Save(account);


            WebhookHelper.SendNotification(
         $"Oyuncu **{tag}**  adlı kullanıcının adı **{newName}** olarak değiştirildi. "
     );

            return $"Başarılı: {tag} için isim başarıyla değiştirildi. Yeni isim: {newName}";
        }

    }





    public class RemoveGems : CommandModule<CommandContext>
    {
        [Command("removegems")]
        public static string RemoveGemsCommand([CommandParameter(Remainder = true)] string playerIdAndAmount)
        {
            string[] parts = playerIdAndAmount.Split(' ');
            if (parts.Length != 2 || !parts[0].StartsWith("#") || !int.TryParse(parts[1], out int removalAmount))
            {
                return "Usage: !removegems [TAG] [kaldırılcak elmas sayısı]";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            if (account.Avatar.Diamonds < removalAmount)
            {
                return $"{parts[0]} oyuncusunun {removalAmount}'ı kaldırmaya yetecek kadar taşı yok.";
            }

            account.Avatar.Diamonds -= removalAmount;

            WebhookHelper.SendNotification(
    $"{parts[0]} kimliğine sahip oyuncudan {removalAmount} değerli taş kaldırıldı. Artık ellerinde {account.Avatar.Diamonds} mücevher kaldı."
);
            return $"{parts[0]} kimliğine sahip oyuncudan {removalAmount} değerli taş kaldırıldı. Artık ellerinde {account.Avatar.Diamonds} mücevher kaldı.";
        }
    }








    public class AddGems : CommandModule<CommandContext>
    {
        [Command("addgems")]
        public static string AddGemsWithWebhook([CommandParameter(Remainder = true)] string playerIdAndAmount)
        {
            string[] parts = playerIdAndAmount.Split(' ');
            if (
                parts.Length != 2
                || !parts[0].StartsWith("#")
                || !int.TryParse(parts[1], out int donationAmount)
            )
            {
                return "Usage: !addgems [TAG] [DonationCount]";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            // Değerli taş ekleme işlemi
            account.Avatar.Diamonds += donationAmount; // Elmasları ekliyoruz
            Notification nGems = new Notification
            {
                Id = 89,
                DonationCount = donationAmount,
                MessageEntry = $"<c6>{donationAmount} değerli taş aldınız, tadını çıkarın! </c>"
            };
            account.Home.NotificationFactory.Add(nGems);
            LogicAddNotificationCommand acmGems = new() { Notification = nGems };
            AvailableServerCommandMessage asmGems = new AvailableServerCommandMessage
            {
                Command = acmGems
            };

            if (Sessions.IsSessionActive(lowID))
            {
                var sessionGems = Sessions.GetSession(lowID);
                sessionGems.GameListener.SendTCPMessage(asmGems);
            }

            // Webhook ile bildirim gönderimi
            WebhookHelper.SendNotification(
                $"Oyuncu **{account.Avatar.Name}** (ID: {parts[0]}) adlı kullanıcıya {donationAmount} değerli taş eklendi! Artık toplam elmas: {account.Avatar.Diamonds}."
            );

            return $"Oyuncu {parts[0]}'ya {donationAmount} değerli taş eklendi. Artık toplamda {account.Avatar.Diamonds} değerli taşı var.";
        }
    }


    public class SetTrophies : CommandModule<CommandContext>
    {
        [Command("settrophies")]
        public static string settrophies(
            [CommandParameter(Remainder = true)] string playerIdAndTrophyCount
        )
        {
            string[] parts = playerIdAndTrophyCount.Split(' ');
            if (
                parts.Length != 2
                || !parts[0].StartsWith("#")
                || !int.TryParse(parts[1], out int trophyCount)
            )
            {
                return "Usage: !settrophies [TAG] [amount]";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            account.Avatar.SetTrophies(trophyCount);

            if (Sessions.IsSessionActive(lowID))
            {
                var session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage()
                    {
                        Message =
                            $"Hesabınız güncellendi! Artık dövüşçülerinizin her birinde {trophyCount} kupa var!"
                    }
                );
                Sessions.Remove(lowID);
            }
            WebhookHelper.SendNotification(
                     $"{parts[0]} kimliğine sahip oyuncular için her dövüşçüye {trophyCount} kupa ayarlandı "
                 );
            return $"{parts[0]} kimliğine sahip oyuncular için her dövüşçüye {trophyCount} kupa ayarlayın.";
        }
    }









    public class AddTrophies : CommandModule<CommandContext> // set ile arasındaki fark, bu komut mevcut kupalara ekleme yapar diğeri girilen değerli kupayı tüm dövüşçülere ekler
    {
        [Command("addtrophies")]
        public static string addtrophies(
            [CommandParameter(Remainder = true)] string playerIdAndTrophyCount
        )
        {
            string[] parts = playerIdAndTrophyCount.Split(' ');
            if (
                parts.Length != 2
                || !parts[0].StartsWith("#")
                || !int.TryParse(parts[1], out int trophyCountToAdd)
            )
            {
                return "Usage: !addtrophies [TAG] [amount]";
            }

            long lowID = LogicLongCodeGenerator.ToId(parts[0]);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            // Current trophies are fetched using a property or direct field.
            int currentTrophies = account.Avatar.Trophies; // Assuming Trophies is a property or field
            int newTrophyCount = currentTrophies + trophyCountToAdd;
            account.Avatar.SetTrophies(newTrophyCount);

            if (Sessions.IsSessionActive(lowID))
            {
                var session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage()
                    {
                        Message =
                            $"Hesabınız güncellendi! Artık tüm dövüşçülerinizde {newTrophyCount} kupa var!"
                    }
                );
                Sessions.Remove(lowID);
            }
            WebhookHelper.SendNotification(
     $"{parts[0]} kimliğine sahip oyuncuya {trophyCountToAdd} kupa eklendi. Yeni toplam {newTrophyCount} kupa."
 );

            return $"{parts[0]} kimliğine sahip oyuncuya {trophyCountToAdd} kupa eklendi. Yeni toplam {newTrophyCount} kupa.";
        }
    }








    public class GivePremium : CommandModule<CommandContext>
    {
        [Command("givepremium")]
        public static string GivePremiumCommand(
            [CommandParameter(Remainder = true)] string playerId
        )
        {
            string[] parts = playerId.Split(' ');
            if (parts.Length != 1)
            {
                return "Usage: !givepremium [TAG]";
            }

            long id = 0;
            bool sc = false;

            if (parts[0].StartsWith('#'))
            {
                id = LogicLongCodeGenerator.ToId(parts[0]);
            }
            else
            {
                sc = true;
                if (!long.TryParse(parts[0], out id))
                {
                    return "Invalid player ID format.";
                }
            }

            Account account = Accounts.Load(id);
            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            if (account.Home.PremiumEndTime < DateTime.UtcNow)
            {
                account.Home.PremiumEndTime = DateTime.UtcNow.AddMonths(1);
            }
            else
            {
                account.Home.PremiumEndTime = account.Home.PremiumEndTime.AddMonths(1);
            }

            account.Avatar.PremiumLevel = 1;

            string formattedDate = account.Home.PremiumEndTime.ToString("dd'th of' MMMM yyyy");

            Notification n = new Notification
            {
                Id = 89,
                DonationCount = 200,
                MessageEntry =
                    $"<c6>VIP durumu etkinleştirildi/uzatıldı {account.Home.PremiumEndTime} UTC! ({formattedDate})</c>"
            };

            account.Home.NotificationFactory.Add(n);

            LogicAddNotificationCommand acm = new LogicAddNotificationCommand { Notification = n };

            AvailableServerCommandMessage asm = new AvailableServerCommandMessage { Command = acm };

            if (Sessions.IsSessionActive(id))
            {
                var session = Sessions.GetSession(id);
                session.GameListener.SendTCPMessage(asm);
            }
            string d = sc ? LogicLongCodeGenerator.ToCode(id) : parts[0];

            WebhookHelper.SendNotification(
    $" {d} için VIP durumunu ayarla etkinleştirildi/uzatıldı {account.Home.PremiumEndTime} UTC! ({formattedDate})"
 );

            return $"tamam {d} için VIP durumunu ayarla etkinleştirildi/uzatıldı {account.Home.PremiumEndTime} UTC! ({formattedDate})";
        }
    }

    public class ChangeUserCredentials : CommandModule<CommandContext>
    {
        [Command("iddegis")] // database'de değişiklik yapar
        public static string ChangeUserCredentialsCommand(
            [CommandParameter(Remainder = true)] string input
        )
        {
            string[] parts = input.Split(' ');
            if (parts.Length != 3)
            {
                return "Usage: !changecredentials [TAG] [newUsername] [newPassword]";
            }

            long id = 0;
            bool sc = false;

            if (parts[0].StartsWith('#'))
            {
                id = LogicLongCodeGenerator.ToId(parts[0]);
            }
            else
            {
                sc = true;
                if (!long.TryParse(parts[0], out id))
                {
                    return "Invalid player ID format.";
                }
            }

            Account account = Accounts.Load(id);
            if (account == null)
            {
                return $"Could not find player with ID {parts[0]}.";
            }

            string newUsername = parts[1];
            string newPassword = parts[2];

            bool success = DatabaseHelper.ExecuteNonQuery(
                "UPDATE users SET username = @username, password = @password WHERE id = @id",
                ("@username", newUsername),
                ("@password", newPassword),
                ("@id", id)
            );

            if (!success)
            {
                return $"Failed to update credentials for player with ID {parts[0]}.";
            }
            string d = sc ? LogicLongCodeGenerator.ToCode(id) : parts[0];


            WebhookHelper.SendNotification(
   $"{d} için kimlik bilgileri güncellendi: Kullanıcı Adı = {newUsername}, Şifre = {newPassword}"
);
            return $"{d} için kimlik bilgileri güncellendi: Kullanıcı Adı = {newUsername}, Şifre = {newPassword}";
        }
    }




    public class Ban : CommandModule<CommandContext>
    {
        [Command("ban")]
        public static string ban([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }

            account.Avatar.Banned = true;
            account.Avatar.Name = "Brawler";

            if (Sessions.IsSessionActive(lowID))
            {
                var session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage { Message = "Yasaklandın! eğer bunun yanlış olduğunu düşünüyorsan discord.gg/timebrawl" }
                );
                Sessions.Remove(lowID);
            }

            WebhookHelper.SendNotification(
           $"Oyuncu **{playerId}**  adlı kullanıcı  banlandı. "
       );


            return $"{playerId} kimliğine sahip oyuncu yasaklandı.";
        }

    }



    public class Unban : CommandModule<CommandContext>
    {
        [Command("unban")]
        public static string unban([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }

            account.Avatar.Banned = false;

            if (Sessions.IsSessionActive(lowID))
            {
                var session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage { Message = "Hesabınız güncellendi!" }
                );
                Sessions.Remove(lowID);
            }
            WebhookHelper.SendNotification(
                     $"Oyuncu **{playerId}**  adlı kullanıcı  banı kaldırıldı. "
                 );
            return $"{playerId} kimliğine sahip oyuncunun yasağı kaldırıldı.";
        }
    }

    public class Mute : CommandModule<CommandContext>
    {
        [Command("mute")]
        public static string mute([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }

            account.Avatar.IsCommunityBanned = true;
            Notification notification =
                new()
                {
                    Id = 81,
                    MessageEntry =
                        "Hesabınızın sosyal işlevleri devre dışı bırakıldı. Bir hata oluştuğunu düşünüyorsanız yönetimle iletişime geçin."
                };
            account.Home.NotificationFactory.Add(notification);

            if (Sessions.IsSessionActive(lowID))
            {
                var session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage { Message = "Sessize alındın!" }
                );
                Sessions.Remove(lowID);
            }
            WebhookHelper.SendNotification(
            $"Oyuncu **{playerId}**  adlı kullanıcı  mutelendi. "
        );

            return $"{playerId} kimliğine sahip oyuncunun sesi kapatıldı.";
        }
    }

    public class GemsToAll : CommandModule<CommandContext>
    {
        [Command("gemsall")]
        public static string ExecuteGemsToAll([CommandParameter(Remainder = true)] string amountAndMessage)
        {
            string[] parts = amountAndMessage.Split(' ', 2);
            if (
                parts.Length != 2
                || !int.TryParse(parts[0], out int gemAmount)
            )
            {
                return "Kullanım: !gemsall [Elmas Sayısı] [Mesaj]";
            }

            try
            {
                // Tüm oyuncu hesaplarını al
                var accounts = Accounts.GetRankingList();

                if (accounts == null || !accounts.Any())
                {
                    return "Veritabanında hiçbir oyuncu bulunamadı.";
                }

                int sentCount = 0;

                // Her bir hesap için elmas gönder
                foreach (var account in accounts)
                {
                    // Elmas bildirimi oluştur
                    Notification notification = new()
                    {
                        Id = 89,
                        DonationCount = gemAmount,
                        MessageEntry = parts[1] // Kullanıcının mesajını buraya ekle
                    };

                    // Hesabın bildirim fabrikasına bildirimi ekle
                    account.Home.NotificationFactory.Add(notification);

                    // Server komutu oluştur ve aktif oturum varsa gönder
                    LogicAddNotificationCommand notificationCommand = new() { Notification = notification };
                    AvailableServerCommandMessage commandMessage = new AvailableServerCommandMessage
                    {
                        Command = notificationCommand
                    };

                    // Eğer oyuncunun oturumu aktifse bildirimi gönder
                    if (Sessions.IsSessionActive(account.AccountId)) // account.AccountId'yi kullanıyoruz
                    {
                        var session = Sessions.GetSession(account.AccountId); // account.AccountId'yi kullanıyoruz
                        session.GameListener.SendTCPMessage(commandMessage);
                    }

                    sentCount++;
                }
                WebhookHelper.SendNotification(
     $"Tüm oyunculara {gemAmount} elmas ve '{parts[1]}' mesajı başarıyla gönderildi. Toplamda {sentCount} oyuncuya bildirim gönderildi. "
  );
                return $"Tüm oyunculara {gemAmount} elmas ve '{parts[1]}' mesajı başarıyla gönderildi. Toplamda {sentCount} oyuncuya bildirim gönderildi.";
            }
            catch (Exception ex)
            {
                return $"Bir hata oluştu: {ex.Message}";
            }
        }
    }



    public class UnMute : CommandModule<CommandContext>
    {
        [Command("unmute")]
        public static string unmute([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }

            account.Avatar.IsCommunityBanned = false;
            Notification notification =
                new() { Id = 81, MessageEntry = "sessizliğiniz açıldı, artık tekrar sohbet edebilirsiniz." };
            account.Home.NotificationFactory.Add(notification);

            if (Sessions.IsSessionActive(lowID))
            {
                var session = Sessions.GetSession(lowID);
                session.GameListener.SendTCPMessage(
                    new AuthenticationFailedMessage { Message = "Sesiniz açıldı!" }
                );
                Sessions.Remove(lowID);
            }

            WebhookHelper.SendNotification(
                     $"Oyuncu **{playerId}**  adlı kullanıcının sesi açıldı. "
                 );
            return $"{playerId} kimliğine sahip oyuncunun sesi açıldı.";
        }
    }

    public class UserInfo : CommandModule<CommandContext>
    {
        [Command("userinfo")]
        public static string userInfo([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }

            string ipAddress = ConvertInfoToData(account.Home.IpAddress);
            string lastLoginTime = account.Home.LastVisitHomeTime.ToString();
            string device = ConvertInfoToData(account.Home.Device);
            string name = ConvertInfoToData(account.Avatar.Name);
            string token = ConvertInfoToData(account.Avatar.PassToken);
            string soloWins = ConvertInfoToData(account.Avatar.SoloWins);
            string duoWins = ConvertInfoToData(account.Avatar.DuoWins);
            string trioWins = ConvertInfoToData(account.Avatar.TrioWins);
            string totalwins = soloWins + duoWins + trioWins;
            string trophies = ConvertInfoToData(account.Avatar.Trophies);
            string banned = ConvertInfoToData(account.Avatar.Banned);
            string muted = ConvertInfoToData(account.Avatar.IsCommunityBanned);
            string sessions = ConvertInfoToData(account.Home.SessionsCount);
           string lastMatch = account.Home.LastMatchResult?.Result.ToString() ?? "No match result";


            string username = DatabaseHelper.ExecuteScalar(
                "SELECT username FROM users WHERE id = @id",
                ("@id", lowID)
            );
            string password = DatabaseHelper.ExecuteScalar(
                "SELECT password FROM users WHERE id = @id",
                ("@id", lowID)
            );

            return $"# Information of {playerId}!\n"
                + $"IpAddress: {ipAddress}\n"
                + $"en son giriş: {lastLoginTime} UTC\n"
                + $"Cihaz {device}\n"
                + $"# hesap bilgileri\n"
                + $"isim: {name}\n"
                + $"Token: {token}\n"
                + $"kupa: {trophies}\n"
                + $"tek win: {soloWins}\n"
                + $"ikili Wins: {duoWins}\n"
                + $"3v3 Wins: {trioWins}\n"
                + $"Toplam win: {totalwins}\n"
                + $"Muted: {muted}\n"
                + $"Banned: {banned}\n"
                + $"Oturum sayısı: {sessions}\n"
                + $"Son maç sonucu: {lastMatch}\n"
                + $"# TİME ID\n"
                + $"kullanıcıadı {username}\n"
                + $"şifre {password}";
                

        }

        private static string ConvertInfoToData(object data)
        {
            return data?.ToString() ?? "N/A";
        }
    }

    public class SendPopupToAll : CommandModule<CommandContext>
    {
        [Command("popupall")]
        public static string ExecuteSendPopupToAll()
        {
            try
            {
                // Tüm oyuncu hesaplarını al
                var accounts = Accounts.GetRankingList(); // Tüm oyuncuları alır

                if (accounts == null || !accounts.Any())
                {
                    return "Veritabanında hiçbir oyuncu bulunamadı.";
                }

                int notifiedCount = 0;

                // Bildirim oluştur
                Notification popupNotification = new Notification
                {
                    Id = 83,
                    PrimaryMessageEntry = "ETKİNLİK BAŞLADI/START EVENTS ",
                    SecondaryMessageEntry = "discorda gelerek etkinliğin ne olduğunu öğrenebilirsin/You can find out what the event is by coming to Discord.",
                    ButtonMessageEntry = "Discord",
                    FileLocation = "pop_up_1920x1235_welcome.png",
                    FileSha = "6bb3b752a80107a14671c7bdebe0a1b662448d0c",
                    ExtLint = "brawlstars://extlink?page=https%3A%2F%2Fdiscord.gg%2F/timebrawl" // yönlendirilecek link
                };

                // Her bir hesap için bildirimi gönder
                foreach (var account in accounts)
                {
                    // Bildirim fabrikasını her hesap için sıfırla
                    NotificationFactory nFactory = new NotificationFactory();
                    nFactory.Add(popupNotification);

                    account.Home.NotificationFactory = nFactory;

                    // Server komutu oluştur
                    LogicAddNotificationCommand popupCommand = new LogicAddNotificationCommand
                    {
                        Notification = popupNotification
                    };

                    AvailableServerCommandMessage commandMessage = new AvailableServerCommandMessage
                    {
                        Command = popupCommand
                    };

                    // Eğer oyuncunun oturumu aktifse bildirimi gönder
                    if (Sessions.IsSessionActive(account.AccountId))
                    {
                        var session = Sessions.GetSession(account.AccountId);
                        session.GameListener.SendTCPMessage(commandMessage);
                    }

                    notifiedCount++;
                }

                // Webhook ile bildirim gönderimi
                WebhookHelper.SendNotification(
                    $"Toplamda **{notifiedCount}** kişiye popup gitti."
                );

                return $"Tüm oyunculara popup başarıyla gönderildi. Toplamda {notifiedCount} oyuncuya bildirimi ulaştırıldı.";
            }
            catch (Exception ex)
            {
                return $"Bir hata oluştu: {ex.Message}";
            }
        }
    }






public class UserInfoo : CommandModule<CommandContext>
{
    private readonly string jsonFilePath = @"C:\Users\arda\Desktop\royale-brawl-v29-main\src\Supercell.Laser.Server\bin\Release\net8.0\json\accounts.json";

    [Command("kayıt")]
    public async Task RegisterUser([CommandParameter(Remainder = true)] string tag)
    {
        string userId = Context.Message.Author.Id.ToString();

        var accountData = new
        {
            UserId = userId,
            Tag = tag,
            DateRegistered = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Diamonds = 0
        };

        try
        {
            if (!File.Exists(jsonFilePath))
            {
                var accounts = new List<dynamic> { accountData };
                File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(accounts, Formatting.Indented));
                await Context.Message.ReplyAsync($"Hesap {tag} ile başarıyla kaydedildi.");
            }
            else
            {
                var accounts = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(jsonFilePath)) ?? new List<dynamic>();
                if (accounts.Any(a => a.UserId == userId))
                {
                    await Context.Message.ReplyAsync("Zaten bir hesabınız var.");
                    return;
                }

                accounts.Add(accountData);
                File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(accounts, Formatting.Indented));
                await Context.Message.ReplyAsync($"Hesap {tag} ile başarıyla kaydedildi.");
            }
        }
        catch (Exception ex)
        {
            await Context.Message.ReplyAsync($"Bir hata oluştu: {ex.Message}");
        }
    }

    [Command("hesabım")]
    public async Task GetUserInfo([CommandParameter(Remainder = true)] string playerId)
    {
        if (!playerId.StartsWith("#"))
        {
            await Context.Message.ReplyAsync("Geçersiz oyuncu kimliği. Lütfen '#' ile başladığından emin olun.");
            return;
        }

        try
        {
            long lowID = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                await Context.Message.ReplyAsync($"{playerId} kimliğine sahip bir oyuncu bulunamadı.");
                return;
            }

            string ipAddress = ConvertInfoToData(account.Home?.IpAddress);
            string lastLoginTime = account.Home?.LastVisitHomeTime.ToString() ?? "N/A";
            string device = ConvertInfoToData(account.Home?.Device);
            string name = ConvertInfoToData(account.Avatar?.Name);
            string token = ConvertInfoToData(account.Avatar?.PassToken);
            string soloWins = ConvertInfoToData(account.Avatar?.SoloWins);
            string duoWins = ConvertInfoToData(account.Avatar?.DuoWins);
            string trioWins = ConvertInfoToData(account.Avatar?.TrioWins);
            string trophies = ConvertInfoToData(account.Avatar?.Trophies);
            string banned = ConvertInfoToData(account.Avatar?.Banned);
            string muted = ConvertInfoToData(account.Avatar?.IsCommunityBanned);
            string bplevel = ConvertInfoToData(account.Avatar.PremiumLevel);
            string elmas = ConvertInfoToData(account.Avatar?.Diamonds);
            string Premium = ConvertInfoToData(account.Avatar.IsPremium);

            string username = DatabaseHelper.ExecuteScalar(
                "SELECT username FROM users WHERE id = @id",
                ("@id", lowID)
            )?.ToString() ?? "N/A";

            string password = DatabaseHelper.ExecuteScalar(
                "SELECT password FROM users WHERE id = @id",
                ("@id", lowID)
            )?.ToString() ?? "N/A";

            // Kulüp bilgilerini al
            string allianceName = account.Avatar?.AllianceName ?? "Yok"; // kulüpte değilse yok olarak göster
            string allianceRole = GetAllianceRole(account.Avatar?.AllianceRole);

            await Context.Message.ReplyAsync($"# işte {playerId} oyuncusunun bilgileri:\n"
                + $"Ad: {name}\n"
                + $"Kupa: {trophies}\n"
                + $"Tekli Zaferler: {soloWins}\n"
                + $"Çiftli Zaferler: {duoWins}\n"
                + $"3v3 Zaferler: {trioWins}\n"
                + $"Son Giriş: {lastLoginTime}\n"
                + $"Cihaz: {device}\n"
                + $"Muted: {muted}\n"
                + $"Banned: {banned}\n"
                + $"Kulüp: {allianceName}\n"
                + $"elmasları: {elmas}\n"
                + $"premium: {Premium}\n"
                + $"brawl pass level: {bplevel}\n"
                + $"Kulüp Rolü: {allianceRole}");
        }
        catch (Exception ex)
        {
            await Context.Message.ReplyAsync($"Bir hata oluştu: {ex.Message}");
        }
        WebhookHelper.SendNotification(
             $"Oyuncu **{playerId}**  İD'li oyuncu aratıldı. "
         );
    }

    private static string ConvertInfoToData(object data)
    {
        return data?.ToString() ?? "N/A";
    }

    // Kulüp rolünü al
    private static string GetAllianceRole(AllianceRole? role)
    {
        return role switch
        {
            AllianceRole.Member => "Üye",
            AllianceRole.Leader => "Başkan",
            AllianceRole.Elder => "Kıdemli Üye",
            AllianceRole.CoLeader => "Başkan Yardımcısı",
            _ => "Rolü yok", // !?!!?!!!
        };
    }
    public class ResetSeason : CommandModule<CommandContext>
    {
        [Command("resetseason")]
        public static string ResetSeasonCommand()
        {
            long maxAccountId = Accounts.GetMaxAvatarId();

            for (int accountId = 1; accountId <= maxAccountId; accountId++)
            {
                Account account = Accounts.LoadNoCache(accountId);
                if (account == null)
                    continue;

                if (account.Avatar.Trophies >= 550)
                {
                    List<int> heroIds = new();
                    List<int> heroTrophies = new();
                    List<int> resetTrophies = new();
                    List<int> starPointsAwarded = new();

                    int[] trophyRangesStart =
                    {
                        550, 600, 650, 700, 750, 800, 850, 900, 950, 1000, 1050, 1100, 1150, 1200, 1250, 1300, 1350, 1400
                    };

                    int[] trophyRangesEnd =
                    {
                        599, 649, 699, 749, 799, 849, 899, 949, 999, 1049, 1099, 1149, 1199, 1249, 1299, 1349, 1399, 1000000
                    };

                    int[] seasonRewardAmounts =
                    {
                        70, 120, 160, 200, 220, 240, 260, 280, 300, 320, 340, 360, 380, 400, 420, 440, 460, 480
                    };

                    int[] trophyResetValues =
                    {
                        525, 550, 600, 650, 700, 725, 750, 775, 800, 825, 850, 875, 900, 925, 950, 975, 1000, 1025
                    };

                    foreach (Hero hero in account.Avatar.Heroes)
                    {
                        if (hero.Trophies >= trophyRangesStart[0])
                        {
                            heroIds.Add(hero.CharacterId);
                            heroTrophies.Add(hero.Trophies);

                            int index = 0;
                            while (true)
                            {
                                if (hero.Trophies >= trophyRangesStart[index] && hero.Trophies <= trophyRangesEnd[index])
                                {
                                    if (trophyRangesStart[index] != 1400)
                                    {
                                        int trophiesReset = hero.Trophies - trophyResetValues[index];
                                        hero.Trophies = trophyResetValues[index];
                                        resetTrophies.Add(trophiesReset);
                                        starPointsAwarded.Add(seasonRewardAmounts[index]);
                                    }
                                    else
                                    {
                                        int extraTrophies = hero.Trophies - 1440;
                                        extraTrophies /= 2;
                                        int trophiesReset = hero.Trophies - trophyResetValues[index] - extraTrophies;
                                        hero.Trophies = trophyResetValues[index] + extraTrophies;
                                        starPointsAwarded.Add(seasonRewardAmounts[index] + (extraTrophies / 2));
                                        resetTrophies.Add(trophiesReset);
                                    }
                                    break;
                                }
                                index++;
                            }
                        }
                    }

                    if (heroIds.Count > 0)
                    {
                        account.Home.NotificationFactory.Add(
                            new Notification
                            {
                                Id = 79,
                                HeroesIds = heroIds,
                                HeroesTrophies = heroTrophies,
                                HeroesTrophiesReseted = resetTrophies,
                                StarpointsAwarded = starPointsAwarded,
                            }
                        );
                    }
                }
                Accounts.Save(account);
            }

            return "Season reset completed for all players.";
        }
    }
    public class UnlockAll : CommandModule<CommandContext>
    {
        [Command("unlockall")]
        public static string UnlockAllCommand([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Invalid player ID. Make sure it starts with '#'.";
            }

            long id = LogicLongCodeGenerator.ToId(playerId);
            Account account = Accounts.Load(id);

            if (account == null)
            {
                return $"Could not find player with ID {playerId}.";
            }

            try
            {
                account.Avatar.AddDiamonds(99999);
                account.Avatar.StarPoints += 99999;
                account.Avatar.AddGold(99999);

                List<int> allBrawlers =
                    new()
                    {
                        0,
                        1,
                        2,
                        3,
                        4,
                        5,
                        6,
                        7,
                        8,
                        9,
                        10,
                        11,
                        12,
                        13,
                        14,
                        15,
                        16,
                        17,
                        18,
                        19,
                        20,
                        21,
                        22,
                        23,
                        24,
                        25,
                        26,
                        27,
                        28,
                        29,
                        30,
                        31,
                        32,
                        34,
                        35,
                        36,
                        37,
                        38,
                        39,
                        40,
                        41
                    };

                foreach (int brawlerId in allBrawlers)
                {
                    if (brawlerId == 0)
                    {
                        CharacterData character = DataTables
                            .Get(16)
                            .GetDataWithId<CharacterData>(0);
                        if (character == null)
                            continue;

                        CardData card = DataTables
                            .Get(23)
                            .GetData<CardData>(character.Name + "_unlock");
                        if (card == null)
                            continue;

                        account.Avatar.UnlockHero(character.GetGlobalId(), card.GetGlobalId());

                        Hero hero = account.Avatar.GetHero(character.GetGlobalId());
                        if (hero != null)
                        {
                            hero.PowerPoints = 860;
                            hero.PowerLevel = 8;
                            hero.HasStarpower = true;

                            CardData starPower1 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique");
                            CardData starPower2 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_2");
                            CardData starPower3 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_3");

                            if (starPower1 != null)
                                account.Avatar.Starpowers.Add(starPower1.GetGlobalId());
                            if (starPower2 != null)
                                account.Avatar.Starpowers.Add(starPower2.GetGlobalId());
                            if (starPower3 != null && !starPower3.LockedForChronos)
                                account.Avatar.Starpowers.Add(starPower3.GetGlobalId());

                            string[] gadgets =
                            {
                                "GrowBush",
                                "Shield",
                                "Heal",
                                "Jump",
                                "ShootAround",
                                "DestroyPet",
                                "PetSlam",
                                "Slow",
                                "Push",
                                "Dash",
                                "SpeedBoost",
                                "BurstHeal",
                                "Spin",
                                "Teleport",
                                "Immunity",
                                "Trail",
                                "Totem",
                                "Grab",
                                "Swing",
                                "Vision",
                                "Regen",
                                "HandGun",
                                "Promote",
                                "Sleep",
                                "Slow",
                                "Reload",
                                "Fake",
                                "Trampoline",
                                "Explode",
                                "Blink",
                                "PoisonTrigger",
                                "Barrage",
                                "Focus",
                                "MineTrigger",
                                "Reload",
                                "Seeker",
                                "Meteor",
                                "HealPotion",
                                "Stun",
                                "TurretBuff",
                                "StaticDamage"
                            };
                            string characterName =
                                char.ToUpper(character.Name[0]) + character.Name.Substring(1);
                            foreach (string gadgetName in gadgets)
                            {
                                CardData gadget = DataTables
                                    .Get(23)
                                    .GetData<CardData>(characterName + "_" + gadgetName);
                                if (gadget != null)
                                    account.Avatar.Starpowers.Add(gadget.GetGlobalId());
                            }
                        }
                        continue;
                    }

                    if (!account.Avatar.HasHero(16000000 + brawlerId))
                    {
                        CharacterData character = DataTables
                            .Get(16)
                            .GetDataWithId<CharacterData>(brawlerId);
                        if (character == null)
                            continue;

                        CardData card = DataTables
                            .Get(23)
                            .GetData<CardData>(character.Name + "_unlock");
                        if (card == null)
                            continue;

                        account.Avatar.UnlockHero(character.GetGlobalId(), card.GetGlobalId());

                        Hero hero = account.Avatar.GetHero(character.GetGlobalId());
                        if (hero != null)
                        {
                            hero.PowerPoints = 860;
                            hero.PowerLevel = 8;
                            hero.HasStarpower = true;

                            CardData starPower1 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique");
                            CardData starPower2 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_2");
                            CardData starPower3 = DataTables
                                .Get(23)
                                .GetData<CardData>(character.Name + "_unique_3");

                            if (starPower1 != null)
                                account.Avatar.Starpowers.Add(starPower1.GetGlobalId());
                            if (starPower2 != null)
                                account.Avatar.Starpowers.Add(starPower2.GetGlobalId());
                            if (starPower3 != null && !starPower3.LockedForChronos)
                                account.Avatar.Starpowers.Add(starPower3.GetGlobalId());

                            string[] gadgets =
                            {
                                "GrowBush",
                                "Shield",
                                "Heal",
                                "Jump",
                                "ShootAround",
                                "DestroyPet",
                                "PetSlam",
                                "Slow",
                                "Push",
                                "Dash",
                                "SpeedBoost",
                                "BurstHeal",
                                "Spin",
                                "Teleport",
                                "Immunity",
                                "Trail",
                                "Totem",
                                "Grab",
                                "Swing",
                                "Vision",
                                "Regen",
                                "HandGun",
                                "Promote",
                                "Sleep",
                                "Slow",
                                "Reload",
                                "Fake",
                                "Trampoline",
                                "Explode",
                                "Blink",
                                "PoisonTrigger",
                                "Barrage",
                                "Focus",
                                "MineTrigger",
                                "Reload",
                                "Seeker",
                                "Meteor",
                                "HealPotion",
                                "Stun",
                                "TurretBuff",
                                "StaticDamage"
                            };

                            string characterName =
                                char.ToUpper(character.Name[0]) + character.Name.Substring(1);
                            foreach (string gadgetName in gadgets)
                            {
                                CardData gadget = DataTables
                                    .Get(23)
                                    .GetData<CardData>(characterName + "_" + gadgetName);
                                if (gadget != null)
                                    account.Avatar.Starpowers.Add(gadget.GetGlobalId());
                            }
                        }
                    }
                }

                List<string> skins =
                    new()
                    {
                        "Witch",
                        "Rockstar",
                        "Beach",
                        "Pink",
                        "Panda",
                        "White",
                        "Hair",
                        "Gold",
                        "Rudo",
                        "Bandita",
                        "Rey",
                        "Knight",
                        "Caveman",
                        "Dragon",
                        "Summer",
                        "Summertime",
                        "Pheonix",
                        "Greaser",
                        "GirlPrereg",
                        "Box",
                        "Santa",
                        "Chef",
                        "Boombox",
                        "Wizard",
                        "Reindeer",
                        "GalElf",
                        "Hat",
                        "Footbull",
                        "Popcorn",
                        "Hanbok",
                        "Cny",
                        "Valentine",
                        "WarsBox",
                        "Nightwitch",
                        "Cart",
                        "Shiba",
                        "GalBunny",
                        "Ms",
                        "GirlHotrod",
                        "Maple",
                        "RR",
                        "Mecha",
                        "MechaWhite",
                        "MechaNight",
                        "FootbullBlue",
                        "Outlaw",
                        "Hogrider",
                        "BoosterDefault",
                        "Shark",
                        "HoleBlue",
                        "BoxMoonFestival",
                        "WizardRed",
                        "Pirate",
                        "GirlWitch",
                        "KnightDark",
                        "DragonDark",
                        "DJ",
                        "Wolf",
                        "Brown",
                        "Total",
                        "Sally",
                        "Leonard",
                        "SantaRope",
                        "Gift",
                        "GT",
                        "Virus",
                        "BoosterVirus",
                        "Gamer",
                        "Valentines",
                        "Koala",
                        "BearKoala",
                        "AgentP",
                        "Football",
                        "Arena",
                        "Tanuki",
                        "Horus",
                        "ArenaPSG",
                        "DarkBunny",
                        "College",
                        "Bazaar",
                        "RedDragon",
                        "Constructor",
                        "Hawaii",
                        "Barbking",
                        "Trader",
                        "StationSummer",
                        "Silver",
                        "Bank",
                        "Retro",
                        "Ranger",
                        "Tracksuit",
                        "Knight",
                        "RetroAddon"
                    };

                foreach (Hero hero in account.Avatar.Heroes)
                {
                    CharacterData c = DataTables
                        .Get(DataType.Character)
                        .GetDataByGlobalId<CharacterData>(hero.CharacterId);
                    string cn = c.Name;
                    foreach (string name in skins)
                    {
                        SkinData s = DataTables.Get(DataType.Skin).GetData<SkinData>(cn + name);
                        if (s != null && !account.Home.UnlockedSkins.Contains(s.GetGlobalId()))
                        {
                            account.Home.UnlockedSkins.Add(s.GetGlobalId());
                        }
                    }
                }
                if (Sessions.IsSessionActive(id))
                {
                    var session = Sessions.GetSession(id);
                    session.GameListener.SendTCPMessage(
                        new AuthenticationFailedMessage
                        {
                            Message =
                                "Hesabınız, her şeyin kilidi açılmış ve maksimuma çıkarılmış şekilde güncellendi!"
                        }
                    );
                    Sessions.Remove(id);
                }


                return $"{playerId} kimliğine sahip oyuncu için kilidi başarıyla açıldı ve her şey maksimuma çıkarıldı.";
            }
            catch (Exception ex)
            {
                return $"An error occurred while unlocking content: {ex.Message}";
            }
        }
    }
























    public class StartEvent : CommandModule<CommandContext>
    {
        [Command("startevent")]
        public static string StartEventCommand([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Geçerli bir oyuncu ID'si değil. ID'nin başında '#' olmalı.";
            }


            long lowID = LogicLongCodeGenerator.ToId(playerId);
            // Oyuncunun bilgilerini al
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"ID {playerId} olan oyuncu bulunamadı.";
            }

            // Mevcut kupa sayısını çek
            int currentTrophies = account.Avatar.Trophies;

            // Event.json dosyasının yolu
            string directoryPath = @"C:\Users\Administrator\Desktop\royale-brawl-v29-main(1)\time\src\Supercell.Laser.Server\bin\Release\net8.0\json";
            string filePath = Path.Combine(directoryPath, $"event_{playerId}.json");

            try
            {
                Dictionary<string, int> eventData;

                // JSON dosyasını oku veya yeni bir tane oluştur
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    eventData = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
                }
                else
                {
                    eventData = new Dictionary<string, int>();
                }

                // Oyuncunun mevcut kupasını kaydet
                eventData[playerId] = currentTrophies;

                // JSON dosyasına yaz
                File.WriteAllText(filePath, JsonConvert.SerializeObject(eventData, Formatting.Indented));

                WebhookHelper.SendNotification(
                    $"Oyuncu {playerId} mevcut kupası ({currentTrophies}) başarıyla kaydedildi. "
                );
                return $"Oyuncu {playerId} mevcut kupası ({currentTrophies}) {filePath} dosyasına kaydedildi.";
            }
            catch (Exception ex)
            {
                return $"Bir hata oluştu: {ex.Message}";
            }
        }
    }







    public class Event : CommandModule<CommandContext>
    {
        [Command("event")]
        public static string StartEventCommand([CommandParameter(Remainder = true)] string playerId)
        {
            if (!playerId.StartsWith("#"))
            {
                return "Geçerli bir oyuncu ID'si değil. ID'nin başında '#' olmalı.";
            }

            // Oyuncu kimliğini uygun bir uzun tamsayıya dönüştür
            long lowID = LogicLongCodeGenerator.ToId(playerId);
            // Oyuncunun bilgilerini al
            Account account = Accounts.Load(lowID);

            if (account == null)
            {
                return $"ID {playerId} olan oyuncu bulunamadı.";
            }

            // Mevcut kupa sayısını çek
            int currentTrophies = account.Avatar.Trophies;

            // Hedef kupa
            int eventTrophies = 1000; // Bu değer event için belirlenen kupa sayısıdır

            // Event.json dosyasının yolu
            string directoryPath = @"C:\Users\Administrator\Desktop\royale-brawl-v29-main (1)\time\src\Supercell.Laser.Server\bin\Release\net8.0\json";
            string filePath = Path.Combine(directoryPath, $"event_{playerId}.json");

            try
            {
                Dictionary<string, int> eventData;

                // JSON dosyasını okur ve veriyi alır
                if (!File.Exists(filePath))
                {
                    return $"Etkinlik başlatılmamış. Lütfen önce !startevent komutunu kullanın.";
                }
                else
                {
                    // JSON dosyasını okur ve deserializes ederiz
                    string jsonData = File.ReadAllText(filePath);
                    eventData = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonData);
                }

                // Kaydedilen kupa değerini al
                int savedTrophies = eventData.ContainsKey(playerId) ? eventData[playerId] : 0;

                // Hedef kupaya ulaşılacak mı?
                int totalTrophies = savedTrophies + eventTrophies;

                if (currentTrophies >= totalTrophies)
                {
                    return $"Oyuncu {playerId} hedef kupaya ({totalTrophies}) ulaştı. Etkinlik tamamlandı.";
                }
                else
                {
                    // Mevcut kupa ile hedef kupa arasındaki farkı döner
                    int remainingTrophies = totalTrophies - currentTrophies;
                    return $"Oyuncu {playerId} hedef kupaya {remainingTrophies} kupa kaldı.";
                }
            }
            catch (Exception ex)
            {
                return $"Bir hata oluştu: {ex.Message}";
            }
        }
    }



















    public class Status : CommandModule<CommandContext>
    {
        [Command("status")]
        public static string status()
        {
            long megabytesUsed = Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024);
            DateTime startTime = Process.GetCurrentProcess().StartTime;
            DateTime now = DateTime.Now;

            TimeSpan uptime = now - startTime;

            string formattedUptime = string.Format(
                "{0}{1}{2}{3}",
                uptime.Days > 0 ? $"{uptime.Days} Gün, " : string.Empty,
                uptime.Hours > 0 || uptime.Days > 0 ? $"{uptime.Hours} Saat, " : string.Empty,
                uptime.Minutes > 0 || uptime.Hours > 0
                  ? $"{uptime.Minutes} Dakika, "
                  : string.Empty,
                uptime.Seconds > 0 ? $"{uptime.Seconds} Saniye" : string.Empty
            );

            return "# Sunucu Durumu\n"
                + $"Sunucu Oyun Sürümü: v29.270\n"
                + $"Sunucu Yapısı: v1.0 - 10.02.2024\n"
                + $"Kaynaklar SHA: {Fingerprint.Sha}\n"
                + $"Ortam: Prod\n"
                + $"Sunucu Zamanı: {now} UTC\n"
                + $"Çevrimiçi Oyuncu Sayısı: {Sessions.Count}\n"
                + $"Kullanılan Bellek: {megabytesUsed} MB\n"
                + $"Çalışma Süresi: {formattedUptime}\n"
                + $"Hesaplar Önbellekte: {AccountCache.Count}\n"
                + $"Birlikler Önbellekte: {AllianceCache.Count}\n"
                + $"Takımlar Önbellekte: {Teams.Count}\n";
        }
    }



    public class Reports : CommandModule<CommandContext> //TODO don't use litterbox api and send directly through discord
    {
        [Command("reports")]
        public static async Task<string> reports()
        {
            string filePath = "reports.txt";

            if (!File.Exists(filePath))
            {
                return "The reports file does not exist / no reports have been made yet";
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {
                        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                        ByteArrayContent fileContent = new ByteArrayContent(fileBytes);
                        content.Add(fileContent, "fileToUpload", Path.GetFileName(filePath));

                        content.Add(new StringContent("fileupload"), "reqtype");
                        content.Add(new StringContent("72h"), "time");

                        // litterbox api
                        HttpResponseMessage response = await client.PostAsync(
                            "https://litterbox.catbox.moe/resources/internals/api.php",
                            content
                        );

                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            return $"Reports uploaded to: {responseBody}";
                        }
                        else
                        {
                            return $"Failed to upload reports file to Litterbox. Status code: {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred while uploading the reports file: {ex.Message}";
            }
        }
    }
}
    
 public class SendShutdownMessage : CommandModule<CommandContext> // test edilmedi!!!
{
    [Command("kapatmesaj")]
    public static string ExecuteSendShutdownMessage([CommandParameter(Remainder = true)] string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return "Kullanım: !kapatmesaj [mesaj]";
        }

        try
        {
            int sentCount = 0;
            var sessions = Sessions.ActiveSessions.Values.ToArray();
            bool isUrgent = message.ToLower().Contains("acil") ||
                          message.ToLower().Contains("urgent") ||
                          message.ToLower().Contains("emergency");

            foreach (var session in sessions)
            {
                var shutdownMessage = new CustomShutdownMessage
                {
                    Message = message,
                    TimeLeft = isUrgent ? 30 : 60, // Acil durumda 30 saniye, normalde 60 saniye
                    IsUrgent = isUrgent
                };

                session.Connection.Send(shutdownMessage);
                sentCount++;
            }


            return $"Kapatma mesajı başarıyla gönderildi.\n" +
                   $"Mesaj: {message}\n" +
                   $"Etkilenen Oyuncu: {sentCount}\n" +
                   $"Durum: {(isUrgent ? "Acil" : "Normal")}\n" +
                   $"Kalan Süre: {(isUrgent ? "30" : "60")} saniye";
        }
        catch (Exception ex)
        {
            Logger.Error($"Kapatma mesajı gönderilirken hata: {ex.Message}");
            return $"Bir hata oluştu: {ex.Message}";
        }
    }







}
 