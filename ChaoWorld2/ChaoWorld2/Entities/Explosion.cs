using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoWorld2.Entities
{
  public class Explosion : Entity
  {
    public int TicksAlive = 0;
    public int Frame = 0;

    public Explosion() { }
    public Explosion(float x, float y)
    {
      this.X = x;
      this.Y = y;
      Game1.PlaySound("explosion", 0.15f);
    }

    public override void Update(GameTime gameTime)
    {
      this.TicksAlive++;
      if (this.TicksAlive % 3 == 0)
        this.Frame++;
      if (this.Frame > 5)
      {
        Owner.RemoveEntity(this);
        return;
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["exploooded"], new Vector2(X - Game1.TileSize, Y - Game1.TileSize).DrawPos(), new Rectangle(this.Frame * 64, 0, 64, 64), Color.White, 0f, Vector2.Zero, 2f * (Game1.PixelZoom / 4), SpriteEffects.None, 0.48f - (Y / 100000));
    }
  }
}
