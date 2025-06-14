namespace Supercell.Laser.Server
{
    using System.Drawing;
    using Supercell.Laser.Server.Handler;
    using Supercell.Laser.Server.Settings;

    static class Program
    {
        public const string SERVER_VERSION = "29.258";
        public const string BUILD_TYPE = "Beta";

        private static void Main(string[] args)
        {
            Console.Title = "BrawlStars - server emulator v" + SERVER_VERSION + " Build: " + BUILD_TYPE;
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            Colorful.Console.WriteWithGradient(
                @"
    ____                      __   _____ __
   / __ )_________ __      __/ /  / ___// /_____ ___________
  / __  / ___/ __ `/ | /| / / /   \__ \/ __/ __ `/ ___/ ___/
 / /_/ / /  / /_/ /| |/ |/ / /   ___/ / /_/ /_/ / /  (__  )
/_____/_/   \__,_/ |__/|__/_/   /____/\__/\__,_/_/  /____/

       " + "\n\n\n", Color.Fuchsia, Color.Cyan, 8);

            Logger.Init();
            Configuration.Instance = Configuration.LoadFromFile("config.json");

            Resources.InitDatabase();
            Resources.InitDiscord();
            Resources.InitLogic();
            Resources.InitNetwork();

            // Sunucu başlatıldı
            Logger.Print("Server started successfully!");
           ExitHandler.Init();
            CmdHandler.Start();
           
        }
    }
}
