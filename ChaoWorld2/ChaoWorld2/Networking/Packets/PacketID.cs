using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChaoWorld2.Networking.Packets
{
  public enum PacketID
  {
    Connect = 0,
    ServerPingPacket = 1,
    ClientPingPacket = 2,
    SendMessage = 3,
    ChatMessage = 4,
    Hello = 5,
    UpdateEntities = 6,
    AddRemoveEntities = 7,
  }
}
