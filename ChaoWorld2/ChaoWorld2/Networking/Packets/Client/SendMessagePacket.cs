using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoWorld2.Networking.Packets.Client
{
  public class SendMessagePacket : Packet
  {
    public SendMessagePacket() : base(PacketID.SendMessage) { }

    public string Text;

    public override void Read(BinaryReader rdr)
    {
      Text = rdr.ReadString();
      
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(Text);
    }
  }
}
