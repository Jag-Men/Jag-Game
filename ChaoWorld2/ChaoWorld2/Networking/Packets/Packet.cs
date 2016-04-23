using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ChaoWorld2.Networking.Packets.Client;
using ChaoWorld2.Networking.Packets.Server;

namespace ChaoWorld2.Networking.Packets
{
  public class Packet
  {
    public static Packet GetPacketFromID(byte id)
    {
      switch ((PacketID)id)
      {
        case PacketID.Hello:
          return new HelloPacket();
        case PacketID.ServerPingPacket:
          return new ServerPingPacket();
        case PacketID.ClientPingPacket:
          return new ClientPingPacket();
        case PacketID.SendMessage:
          return new SendMessagePacket();
        case PacketID.ChatMessage:
          return new ChatMessagePacket();
        default:
          return null;
      }
    }

    public PacketID ID;
    public Packet(PacketID id)
    {
      this.ID = id;
    }

    public void Read(byte[] body)
    {
      Read(new BinaryReader(new MemoryStream(body)));
    }

    public int Write(byte[] buff)
    {
      var ms = new MemoryStream(buff, 5, buff.Length - 5);
      Write(new BinaryWriter(ms));

      var len = (int)ms.Position;
      Buffer.BlockCopy(BitConverter.GetBytes(len + 5), 0, buff, 0, 4);
      buff[4] = (byte)ID;
      return len + 5;
    }

    public virtual void Read(BinaryReader rdr) { }
    public virtual void Write(BinaryWriter wtr) { }
  }
}
