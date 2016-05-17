using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  class Giantdog : Entity
  {

    public override void Update(GameTime gameTime)
    {
      
      base.Update(gameTime);
    }
    public Giantdog(Vector2 pos)
    {
      Collision.Add("Solid");
      this.X = pos.X * Game1.TileSize;
      this.Y = pos.Y * Game1.TileSize;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:images"], new Vector2(this.X, this.Y - Game1.TileSize).DrawPos(), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, Layer.Object - (Y + Game1.TileSize) / 1e5f);
      base.Draw(spriteBatch);
    }
  }
}
