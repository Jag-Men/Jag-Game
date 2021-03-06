﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ChaoWorld2
{
  public class Entity
  {
    public float X;
    public float Y;
    public float ani;
    public int animation;
    public int animationb;
    public int speed;
    public int ID = -1;
    public World Owner;
    public List<string> Collision = new List<string>();
    
    public Vector2 XandY
    {
      get { return new Vector2(this.X, this.Y); }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    public Entity() { }

    public virtual void Update(GameTime gameTime) { }
    public virtual void UpdateEvenWhenPaused(GameTime gameTime) { }
    public virtual void Draw(SpriteBatch spriteBatch) { }

    public virtual Rectangle GetCollisionBox()
    {
      return new Rectangle((int)X, (int)Y, Game1.TileSize, Game1.TileSize);
    }

    public virtual void Read(BinaryReader rdr)
    {
      ID = rdr.ReadInt32();
      X = rdr.ReadSingle();
      Y = rdr.ReadSingle();
    }

    public virtual void Write(BinaryWriter wtr)
    {
      wtr.Write(ID);
      wtr.Write(X);
      wtr.Write(Y);
    }
  }
}
