using ChaoWorld2.Networking.Client;
using ChaoWorld2.Networking.Server;
using System;
using System.Globalization;
using System.Threading;

namespace ChaoWorld2
{
#if WINDOWS || XBOX
  internal static class Program
  {
    public static NetworkTicker NetworkTicker;
    public static Thread NetworkThread;

    public static Client Client;
    public static Thread ClientThread;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args)
    {
      if (args.Length == 1)
      {
        Client = new Client();
        ClientThread = new Thread(Client.TickLoop);
        ClientThread.Start();
        using (Game1 game = new Game1("127.0.0.1", 9018))
        {
          game.Run();
        }
      }
      else
      {
        //NetworkTicker = new NetworkTicker(9018);
        //NetworkThread = new Thread(NetworkTicker.TickLoop)
        //{
        //  Name = "Server",
        //  CurrentCulture = CultureInfo.InvariantCulture
        //};
        //NetworkThread.Start();

        using (Game1 game = new Game1())
        {
          game.Run();
        }
      }
    }
  }
#endif
}

