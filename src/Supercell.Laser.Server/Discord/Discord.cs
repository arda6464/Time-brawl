namespace Supercell.Laser.Server.Discord
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using NetCord;
    using NetCord.Gateway;
    using NetCord.Services;
    using NetCord.Services.Commands;
    using Supercell.Laser.Server.Settings;

    public class CustomLogger
    {
        public void Log(LogSeverity severity, string message, Exception exception = null)
        {
            string formattedMessage = $"[DISCORD] {message}";

            if (severity == LogSeverity.Info)
            {
                Console.WriteLine(formattedMessage);
            }
            else
            {
                Console.WriteLine($"[{severity}] {formattedMessage}");
            }
        }
    }

    public class DiscordBot
    {
        private readonly CustomLogger _logger = new CustomLogger();
        private GatewayClient _client;
        private readonly ulong[] _allowedUserIds = { 1141268241117872158, 807674732547014686, 828243314024120381 };
        private readonly string[] _allowedCommands = { "status", "hesabım", "startevent", "event", "yardım", "kayıt", "liderlik" };
       private readonly string[] _yetkiliCommands = { "ban", "resetseason", "mute", "unmute", "userinfo", "deleteclub", "unlockall", "addgems", "removegems", "bildirim", "bildirimall", "popupall", "removepremium", "givepremium", "isimdegistir", "unban", "gemsall", "settrophies", "addtrophies", "iddegis", "reports" };

        public async Task StartAsync()
        {
            _client = new GatewayClient(
                new BotToken(Configuration.Instance.BotToken),
                new GatewayClientConfiguration()
                {
                    Intents =
                        GatewayIntents.GuildMessages
                        | GatewayIntents.DirectMessages
                        | GatewayIntents.MessageContent,
                }
            );

            CommandService<CommandContext> commandService = new();
            commandService.AddModules(typeof(DiscordBot).Assembly);

           _client.MessageCreate += async message =>
{
    if (!message.Content.StartsWith("tbp!") || message.Author.IsBot)
        return;

    var commandName = message.Content.Substring(4).Split(' ')[0]; // Prefix uzunluğu kadar substring alındı.

    if (_yetkiliCommands.Contains(commandName))
    {
        if (!_allowedUserIds.Contains(message.Author.Id))
        {
            await message.ReplyAsync("Bu komutu kullanma izniniz yok.");
            return;
        }
    }

    var result = await commandService.ExecuteAsync(
        prefixLength: 4, // Prefix'in uzunluğu
        new CommandContext(message, _client)
    );

    if (result is IFailResult failResult)
    {
        try
        {
            await message.ReplyAsync(failResult.Message);
        }
        catch { }
    }
};


            _client.Log += message =>
            {
                _logger.Log(message.Severity, message.Message, message.Exception);
                return default;
            };

            await _client.StartAsync();
        }
    }
}