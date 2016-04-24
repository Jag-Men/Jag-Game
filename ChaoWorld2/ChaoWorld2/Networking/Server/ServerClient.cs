using ChaoWorld2.Networking.Packets;
using ChaoWorld2.Networking.Packets.Client;
using ChaoWorld2.Networking.Packets.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ChaoWorld2.Networking.Server
{
  public class ServerClient
  {
    public int ID;
    public string RealUsername;
    public string Username;
    public TcpClient TcpClient;
    public PacketHandler Handler;

    public ServerClient(TcpClient cli)
    {
      this.TcpClient = cli;
      this.ID = -1;
      this.Handler = new PacketHandler(cli);
    }

    public void Disconnect(string msg = "{0} disconnected")
    {
      Console.WriteLine("Disconnecting client");
      ClientManager.SendMessage(string.Format(msg, Username));
      ClientManager.RemoveClient(this);
      if (TcpClient.Connected)
        TcpClient.Close();
    }


    public void HandleHelloPacket(HelloPacket pkt)
    {
      if (pkt.Username != "")
      {
        int matchingUsernames = 0;
        foreach (var i in ClientManager.Clients.Values)
          if (i.RealUsername == pkt.Username)
            matchingUsernames++;
        this.RealUsername = pkt.Username;
        this.Username = pkt.Username + (matchingUsernames > 0 ? matchingUsernames.ToString() : "");
      }
    }

    public void ChangeUsername(string username)
    {
      string oldName = this.Username;
      int matchingUsernames = 0;
      foreach (var i in ClientManager.Clients.Values)
        if (i.RealUsername == username && i != this)
          matchingUsernames++;
      this.RealUsername = username;
      this.Username = username + (matchingUsernames > 0 ? matchingUsernames.ToString() : "");
      ClientManager.SendMessage(oldName + " changed name to " + this.Username);
    }

    public void HandleSendMessagePacket(SendMessagePacket pkt)
    {
      if (pkt.Text.StartsWith("/") && pkt.Text.Length > 1)
      {
        string[] args = pkt.Text.Split(' ');
        if (args[0] == "/me")
        {
          ClientManager.SendMessage("*" + Username + " " + string.Join(" ", args.Skip(1)), "");
          return;
        }
        if (args[0] == "/nick")
        {
          string nick = string.Join(" ", args.Skip(1)).Trim();
          if (!new Regex("^[a-zA-Z0-9_-]*$").IsMatch(nick))
            SendMessage("Invalid username.", "*Error*");
          else if (nick.Length < 1)
            SendMessage("Username must have at least 1 character.", "*Error*");
          else if (nick.Length > 15)
            SendMessage("Username must be shorter than 15 characters.", "*Error*");
          else
            ChangeUsername(nick);
          return;
        }
        SendMessage("Unknown command: " + args[0], "*Error*");
      }
      else
      {
        ClientManager.SendMessage(pkt.Text, Username);
      }
    }

    public void HandlePingPacket(ClientPingPacket pkt)
    {
      if (!pkt.Pong)
        SendPacket(new ServerPingPacket
        {
          Pong = true
        });
    }

    public void SendMessage(string text, string sender = "*Server*")
    {
      Console.WriteLine("Sent message: " + text);
      this.SendPacket(new ChatMessagePacket
      {
        Sender = sender,
        Text = text
      });
    }

    public void ReceivePacket(Packet packet)
    {
      switch (packet.ID)
      {
        case PacketID.Hello:
          HandleHelloPacket(packet as HelloPacket); break;
        case PacketID.SendMessage:
          HandleSendMessagePacket(packet as SendMessagePacket); break;
        case PacketID.ClientPingPacket:
          HandlePingPacket(packet as ClientPingPacket); break;
        default:
          Console.WriteLine("Unhandled packet: " + packet.ID.ToString()); break;
      }
    }

    public void SendPacket(Packet packet)
    {
      Console.WriteLine("Sending packet");
      if (TcpClient.Connected)
        this.Handler.Sending.Enqueue(packet);
    }
  }
}
