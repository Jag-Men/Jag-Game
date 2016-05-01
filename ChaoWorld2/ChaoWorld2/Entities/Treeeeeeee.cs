using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoWorld2.Entities
{
  class Treeeeeeee : Entity
  {
    public Treeeeeeee(Vector2 pos)
    {
      this.X = pos.X * Game1.TileSize;
      this.Y = pos.Y * Game1.TileSize;
    }
    public Treeeeeeee (float x, float y)
    {
      X = x;
      Y = y;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["Tree"], new Vector2(this.X, this.Y).DrawPos(), Color.White);
      base.Draw(spriteBatch);
    }
  }
}
