using ChaoWorld2.Networking.Packets;
using ChaoWorld2.Networking.Packets.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace ChaoWorld2.Networking.Server
{
  public class ClientManager
  {
    public static ConcurrentDictionary<int, ServerClient> Clients = new ConcurrentDictionary<int, ServerClient>();

    public static NetworkTicker Network;
    public static bool Terminated = false;

    private static int nextClientID = 0;
    private static Thread networkThread;

    public static void AddClient(ServerClient client)
    {
      client.ID = nextClientID++;
      client.Username = client.RealUsername = "User" + (client.ID < 100 ? "0" : "") + (client.ID < 10 ? "0" : "") + client.ID;
      if(Clients.TryAdd(client.ID, client))
      {
        client.SendMessage("Connected to groombaland");
        SendMessage(client.Username + " entered the room");
      }
    }

    public static void RemoveClient(ServerClient client)
    {
      ServerClient dummy;
      if (Clients.TryRemove(client.ID, out dummy))
      {

      }
    }

    public static void Start()
    {
      Network = new NetworkTicker(9018);
      networkThread = new Thread(Network.TickLoop)
      {
        Name = "Network",
        CurrentCulture = CultureInfo.InvariantCulture
      };
      networkThread.Start();
    }

    public static void BroadcastPacket(Packet pkt, ServerClient exclude)
    {
      foreach (var i in Clients.Values.Where(c => c != exclude))
        i.SendPacket(pkt);
    }

    public static void SendMessage(string text, string sender = "*Server*")
    {
      BroadcastPacket(new ChatMessagePacket
      {
        Sender = sender,
        Text = text
      }, null);
    }
  }
}
