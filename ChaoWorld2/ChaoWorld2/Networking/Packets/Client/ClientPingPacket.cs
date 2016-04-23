using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoWorld2.Networking.Packets.Client
{
  public class ClientPingPacket : Packet
  {
    public ClientPingPacket() : base(PacketID.ClientPingPacket) { }

    public bool Pong;

    public override void Read(BinaryReader rdr)
    {
      Pong = rdr.ReadBoolean();
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(Pong);
    }
  }
}
