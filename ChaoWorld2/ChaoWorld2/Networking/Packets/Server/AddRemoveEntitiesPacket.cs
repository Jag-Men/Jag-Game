using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Networking.Packets.Server
{
  public class AddRemoveEntitiesPacket : Packet
  {
    public AddRemoveEntitiesPacket() : base(PacketID.AddRemoveEntities) { }

    public List<Entity> AddedEntities;
    public List<int> RemovedEntities;

    public override void Read(BinaryReader rdr)
    {
      AddedEntities = new List<Entity>();
      int addedLen = rdr.ReadInt32();
      for (var i = 0; i < addedLen; i++)
      {
        string entityType = rdr.ReadString();
        Entity newEntity = (Entity)Activator.CreateInstance(Type.GetType(entityType));
        newEntity.Read(rdr);
        AddedEntities.Add(newEntity);
      }

      RemovedEntities = new List<int>();
      int removedLen = rdr.ReadInt32();
      for (var i = 0; i < removedLen; i++)
        RemovedEntities.Add(rdr.ReadInt32());
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(AddedEntities.Count);
      foreach (var i in AddedEntities)
      {
        wtr.Write(i.GetType().AssemblyQualifiedName);
        i.Write(wtr);
      }

      wtr.Write(RemovedEntities.Count);
      foreach (var i in RemovedEntities)
        wtr.Write(i);
    }
  }
}
