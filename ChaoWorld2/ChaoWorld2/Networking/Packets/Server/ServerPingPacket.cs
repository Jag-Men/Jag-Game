using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoWorld2.Networking.Packets.Server
{
  public class ServerPingPacket : Packet
  {
    public ServerPingPacket() : base(PacketID.ServerPingPacket) { }

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
