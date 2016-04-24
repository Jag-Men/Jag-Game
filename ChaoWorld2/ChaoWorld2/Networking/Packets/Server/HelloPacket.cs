using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChaoWorld2.Networking.Packets.Server
{
  public class HelloPacket : Packet
  {
    public HelloPacket() : base(PacketID.Hello) { }

    public int PlayerId;
    public List<Entity> Entities;

    public override void Read(BinaryReader rdr)
    {
      PlayerId = rdr.ReadInt32();

      Entities = new List<Entity>();
      int entityLen = rdr.ReadInt32();
      for (var i = 0; i < entityLen; i++)
      {
        string entityType = rdr.ReadString();
        Entity newEntity = (Entity)Activator.CreateInstance(Type.GetType(entityType));
        newEntity.Read(rdr);
        Entities.Add(newEntity);
      }
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(PlayerId);

      wtr.Write(Entities.Count);
      foreach(var i in Entities)
      {
        wtr.Write(i.GetType().AssemblyQualifiedName);
        i.Write(wtr);
      }
    }
  }
}
