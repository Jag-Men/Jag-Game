using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoWorld2.UI.Menu;
using Microsoft.Xna.Framework.Input;

namespace ChaoWorld2.Entities
{
  class Chest : Entity
  {
    public Chest(Vector2 pos)
      : this()
    {
      this.X = pos.X * Game1.TileSize;
      this.Y = pos.Y * Game1.TileSize;
    }
    public Chest(float x, float y)
    {
      X = x;
      Y = y;
    }

    public Chest()
    {
    }

    public override void Update(GameTime gameTime)
    {
      if (KeyboardUtil.KeyPressed(Keys.Enter) && Vector2.Distance(this.XandY, Game1.Player.XandY) < Game1.TileSize * 3)
        Game1.OpenMenu(new Chestm());
      base.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:chest"], XandY.DrawPos(), null,Color.White,0f,Vector2.Zero,2f, SpriteEffects.None, 0.000001f);
      base.Draw(spriteBatch);
    }
  }
}
