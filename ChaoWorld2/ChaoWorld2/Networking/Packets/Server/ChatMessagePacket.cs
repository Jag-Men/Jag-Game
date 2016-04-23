using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoWorld2.Networking.Packets.Server
{
  public class ChatMessagePacket : Packet
  {
    public ChatMessagePacket() : base(PacketID.ChatMessage) { }

    public string Sender;
    public string Text;

    public override void Read(BinaryReader rdr)
    {
      Sender = rdr.ReadString();
      Text = rdr.ReadString();
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(Sender);
      wtr.Write(Text);
    }
  }
}
