using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChaoWorld2.Networking.Server
{
  public class NetworkTicker
  {
    public int Port;
    public TcpListener Listener;

    public NetworkTicker(int port)
    {
      this.Port = port;
      this.Listener = new TcpListener(IPAddress.Any, 9018);
      this.Listener.Start();
    }

    public void TickLoop()
    {
      Console.WriteLine("Started network loop on port " + Port);
      while (!ClientManager.Terminated)
      {
        int timestamp = Utility.GetUnixTimestamp();

        foreach (ServerClient i in ClientManager.Clients.Values)
        {
          if (!i.TcpClient.Connected)
          {
            i.Disconnect();
            break;
          }

          if (i.TcpClient.Client.Poll(500, SelectMode.SelectRead) && i.TcpClient.Client.Available == 0)
          {
            i.Disconnect();
            break;
          }

          foreach (var pkt in i.Handler.ReceivePackets())
            i.ReceivePacket(pkt);

          i.Handler.SendPackets();
        }

        while (Listener.Pending())
        {
          var cli = new ServerClient(Listener.AcceptTcpClient());
          Console.WriteLine("Received client: " + cli.TcpClient.Client.RemoteEndPoint.ToString());
          ClientManager.AddClient(cli);
        }
      }
    }
  }
}
