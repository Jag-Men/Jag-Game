using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoWorld2.Networking.Packets.Client
{
  public class HelloPacket : Packet
  {
    public HelloPacket() : base(PacketID.Hello) { }

    public string Username;

    public override void Read(BinaryReader rdr)
    {
      Username = rdr.ReadString();
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(Username);
    }
  }
}
