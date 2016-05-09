using ChaoWorld2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoWorld2.Entities
{
  class Plant : Entity
  {
    public Plant(Vector2 pos)
    {
      this.X = pos.X * Game1.TileSize;
      this.Y = pos.Y * Game1.TileSize;
    }
    int frame;
    public Plant(float x, float y)
    {
      X = x;
      Y = y;
    }
    int dog;
    public override void Update(GameTime gameTime)
    {
      dog += gameTime.ElapsedGameTime.Milliseconds;
      if (dog >= 30000)
      {
        dog = 1;
        if (frame<3)
          frame++;
      }

      base.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:plant"], new Vector2(this.X, this.Y).DrawPos(), new Rectangle(this.frame * 16, 0, 16, 16), Color.White,0,Vector2.Zero,Game1.PixelZoom, SpriteEffects.None, Layer.Object - (Y + Game1.TileSize) / 1e5f);
      base.Draw(spriteBatch);
    }
  }
}
