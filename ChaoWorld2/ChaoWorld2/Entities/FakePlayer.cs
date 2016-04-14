using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Entities
{
  public class FakePlayer
  {
    public float X;
    public float Y;
    public int facing;
    public int frame;
    public float alpha;

    public FakePlayer(float x, float y, int facing, int frame)
    {
      this.X = x;
      this.Y = y;
      this.facing = facing;
      this.frame = frame;
      this.alpha = 0.5f;
    }

    public void Update()
    {
      this.alpha -= 0.05f;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White * alpha, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - Y / 100000f);
    }
  }
}
