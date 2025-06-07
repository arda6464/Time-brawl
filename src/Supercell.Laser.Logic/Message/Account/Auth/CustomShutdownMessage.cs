namespace Supercell.Laser.Logic.Message.Account.Auth
{
    public class CustomShutdownMessage : GameMessage
    {
        public string Message { get; set; }
        public int TimeLeft { get; set; } // Kalan süre (saniye)
        public bool IsUrgent { get; set; } // Acil durum mu?

        public CustomShutdownMessage() : base()
        {
            Message = "Sunucu kapatılıyor...";
            TimeLeft = 60;
            IsUrgent = false;
        }

        public override void Encode()
        {
            Stream.WriteString(Message);
            Stream.WriteInt(TimeLeft);
            Stream.WriteBoolean(IsUrgent);
            
            // Ek bilgiler
            Stream.WriteString(""); // Redirect URL
            Stream.WriteString(""); // Content URL
            Stream.WriteString(""); // Update URL
            Stream.WriteInt(-1);
            Stream.WriteBoolean(false);
        }

        public override int GetMessageType()
        {
            return 20103; // AuthenticationFailedMessage ile aynı tip
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
} 