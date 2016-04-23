using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoWorld2.Networking.Packets
{
  public enum PacketID
  {
    Hello = 0,
    ServerPingPacket = 1,
    ClientPingPacket = 2,
    SendMessage = 3,
    ChatMessage = 4,
  }
}
