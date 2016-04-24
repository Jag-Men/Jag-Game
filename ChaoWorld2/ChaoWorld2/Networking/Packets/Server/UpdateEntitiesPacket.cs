using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoWorld2.Networking.Packets.Server
{
  public class UpdateEntitiesPacket : Packet
  {
    public UpdateEntitiesPacket() : base(PacketID.UpdateEntities) { }
    
    public Dictionary<int,byte[]> ReadEntities;
    public List<Entity> WriteEntities;

    public override void Read(BinaryReader rdr)
    {
      ReadEntities = new Dictionary<int, byte[]>();
      int entityLen = rdr.ReadInt32();
      for (var i = 0; i < entityLen; i++)
      {
        int entityId = rdr.ReadInt32();
        int byteCount = rdr.ReadInt32();
        byte[] bytes = rdr.ReadBytes(byteCount);
        ReadEntities.Add(entityId, bytes);
      }
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(WriteEntities.Count);
      foreach(var i in WriteEntities)
      {
        wtr.Write(i.ID);
        MemoryStream mem = new MemoryStream();
        BinaryWriter wtr2 = new BinaryWriter(mem);
        i.Write(wtr2);
        byte[] entityBytes = mem.ToArray();
        wtr.Write(entityBytes.Length);
        wtr.Write(entityBytes);
        wtr2.Close();
      }
    }
  }
}
