using ChaoWorld2.Networking.Packets;
using ChaoWorld2.Networking.Packets.Client;
using ChaoWorld2.Networking.Packets.Server;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ChaoWorld2.Networking.Client
{
  public class Client
  {
    public bool Terminating = false;
    public string DestIp = "";
    
    public TcpClient TcpClient;
    public Socket Socket;
    public PacketHandler Handler;
    public Client()
    {
      this.TcpClient = new TcpClient();
      this.Socket = this.TcpClient.Client;
      this.Handler = new PacketHandler(this.TcpClient);
    }

    public void HandleChatMessagePacket(ChatMessagePacket pkt)
    {
      string senderName = pkt.Sender;
      if (pkt.Sender.StartsWith("*") && pkt.Sender.EndsWith("*"))
        if (pkt.Sender == "*Server*")
          senderName = "SERVER";
        else
          senderName = "";
      Msg((senderName != "" ? ("<" + senderName + "> ") : "") + pkt.Text, pkt.Sender);
    }

    public void ReceivePacket(Packet pkt)
    {
      switch (pkt.ID)
      {
        case PacketID.ChatMessage:
          HandleChatMessagePacket(pkt as ChatMessagePacket); break;
      }
    }

    public void SendPacket(Packet pkt)
    {
      this.Handler.Sending.Enqueue(pkt);
    }

    public void TickLoop()
    {
      Msg("Enter the IP of the server", "*Client*");
      try
      {
        while (!TcpClient.Connected && !Terminating)
        {
          if (DestIp != "")
          {
            try
            {
              Msg("Connecting to " + DestIp, "*Client*");
              this.TcpClient.Connect(DestIp.Split(':')[0], Convert.ToInt32(DestIp.Split(':')[1]));
              Msg("Connected", "*Client*");
              SendPacket(new HelloPacket
              {
                Username = ""
              });
            }
            catch
            {
              Msg("Connection failed.", "*Error*");
              Msg("Enter the IP of the server", "*Client*");
              DestIp = "";
            }
          }
          Thread.Sleep(10);
        }
        bool connected = true;
        while (!Terminating && connected)
        {
          if (!TcpClient.Connected)
          {
            connected = false;
            continue;
          }

          if (TcpClient.Client.Poll(1000, SelectMode.SelectRead) && TcpClient.Client.Available == 0)
          {
            connected = false;
            continue;
          }

          foreach (var pkt in Handler.ReceivePackets())
            ReceivePacket(pkt);

          Handler.SendPackets();
        }
      }
      catch
      {
      }
      Msg("Lost connection to the server.", "*Error*");
      TcpClient.Close();
      TcpClient = new TcpClient();
      Socket = TcpClient.Client;
      Handler.TClient = TcpClient;
      DestIp = "";
      if (!Terminating)
        TickLoop();
    }

    public void Msg(string text, string sender)
    {
      Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] " + text);
    }
  }
}
