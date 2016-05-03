using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  public class FakePlayer : Entity
  {
    public int facing;
    public int frame;
    public float alpha;

    public FakePlayer() { }

    public FakePlayer(float x, float y, int facing, int frame)
    {
      this.X = x;
      this.Y = y;
      this.facing = facing;
      this.frame = frame;
      this.alpha = 0.5f;
    }

    public override void Update(GameTime gameTime)
    {
      if (!Game1.Host)
        return;

      this.alpha -= 0.05f;
      if (this.alpha <= 0)
        Owner.RemoveEntity(this);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White * alpha, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - Y / 1e5f);
    }

    public override void Read(BinaryReader rdr)
    {
      facing = rdr.ReadInt32();
      frame = rdr.ReadInt32();
      alpha = rdr.ReadSingle();
      base.Read(rdr);
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(facing);
      wtr.Write(frame);
      wtr.Write(alpha);
      base.Write(wtr);
    }
  }
}
