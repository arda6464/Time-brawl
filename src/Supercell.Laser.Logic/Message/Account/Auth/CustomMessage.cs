namespace Supercell.Laser.Logic.Message.Account.Auth
{
    public class CustomMessage : GameMessage
    {
        public string Message { get; set; }

        public CustomMessage() : base()
        {
            Message = "";
        }

        public override void Encode()
        {
            Stream.WriteString(Message);
        }

        public override void Decode()
        {
            Message = Stream.ReadString();
        }

        public override int GetMessageType()
        {
            return 20162;
        }

        public override int GetServiceNodeType()
        {
            return 1;
        }
    }
} 