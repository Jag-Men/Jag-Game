using ChaoWorld2.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2
{
  class Buffs
  {
    static int bufflevel;
    public static void AddBuff(BuffType type, int duration, float level)
    {
      Buff newBuff = new Buff();
      newBuff.type = type;
      newBuff.duration = duration;
      newBuff.level = level;
      Bufflist.Add(newBuff);
    }
    public static List<Buff> Bufflist = new List<Buff>();
    public static void Update(GameTime gameTime)
    {
      List<Buff> RemovedBuffs = new List<Buff>();
      foreach (var i in Bufflist)
      {
        bool removed = false;
        i.duration -= gameTime.ElapsedGameTime.Milliseconds;
        if (i.duration <= 0)
        {
          removed = true;
          RemovedBuffs.Add(i);
        }
        switch (i.type)
        {
          case BuffType.Speed:
            if (removed)
              Game1.Player.speedMult = 1f;
            else
              Game1.Player.speedMult = i.level;
            break;
        }
      }
      foreach (var i in RemovedBuffs)
        Bufflist.Remove(i);
    }
    public static void Draw(SpriteBatch spriteBatch)
    {
      foreach (var i in Bufflist)
      {
        spriteBatch.Draw(ContentLibrary.Sprites["ent:sodamachine"], new Vector2(1, 1), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, Layer.Menu);
      }
    }
  }
  class Buff
  {
    public float level;
    public BuffType type;
    public int duration;
  }
  enum BuffType
  {
    Speed
  }
}
