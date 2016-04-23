using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChaoWorld2.Networking.Packets
{
  public class PacketHandler
  {
    public ConcurrentQueue<Packet> Sending = new ConcurrentQueue<Packet>();
    public TcpClient TClient;

    public const int BUFFER_SIZE = 0x10000;
    public byte[] receiveBuff;
    public byte[] sendBuff;

    public PacketHandler(TcpClient client)
    {
      this.TClient = client;

      this.receiveBuff = new byte[BUFFER_SIZE];
      this.sendBuff = new byte[BUFFER_SIZE];
    }

    public List<Packet> ReceivePackets()
    {
      List<Packet> ret = new List<Packet>();
      try
      {
        while (TClient.Client.Available > 0)
        {
          int blen = TClient.Client.Receive(receiveBuff, 0, 5, SocketFlags.None);
          if (blen == 5)
          {
            byte id = receiveBuff[4];
            int len = BitConverter.ToInt32(receiveBuff, 0);
            byte[] body = new byte[len - 5];
            TClient.Client.Receive(body, 0, len - 5, SocketFlags.None);
            Packet pkt = Packet.GetPacketFromID(id);
            pkt.Read(body);
            ret.Add(pkt);
          }
          receiveBuff = new byte[BUFFER_SIZE];
        }
      }
      catch
      {
      }
      return ret;
    }

    public void SendPackets()
    {
      Packet pkt;
      try
      {
        while (Sending.TryDequeue(out pkt))
        {
          int len = pkt.Write(sendBuff);
          TClient.Client.Send(sendBuff, 0, len, SocketFlags.None);
          sendBuff = new byte[BUFFER_SIZE];
        }
      }
      catch
      {
      }
    }
  }
}
