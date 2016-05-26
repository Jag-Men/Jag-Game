using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoWorld2;
using ChaoWorld2.Util;
using ChaoWorld2.Items;

namespace ChaoWorld2.Entities
{
  class Sodamachine : Entity
  {

    public override void Update(GameTime gameTime)
    {
      if (KeyboardUtil.KeyPressed(Keys.H) && Vector2.Distance(this.XandY, Game1.Player.XandY) <= Game1.TileSize * 4)
        Owner.AddEntity(new ItemDrop(Item.Sosda, this.X + (Game1.TileSize / 2), this.Y + (Game1.TileSize / 2)));
      if (KeyboardUtil.KeyPressed(Keys.X) && Vector2.Distance(this.XandY, Game1.Player.XandY) <= Game1.TileSize * 4)
        Owner.AddEntity(new ItemDrop(Item.Sword, this.X + (Game1.TileSize / 2), this.Y + (Game1.TileSize / 2)));
      if (KeyboardUtil.KeyPressed(Keys.P) && Vector2.Distance(this.XandY, Game1.Player.XandY) <= Game1.TileSize * 4)
        Owner.AddEntity(new ItemDrop(Item.doritos, this.X + (Game1.TileSize / 2), this.Y + (Game1.TileSize / 2)));

      base.Update(gameTime);
    }
    public Sodamachine(Vector2 pos)
    {
      Collision.Add("Solid");
      this.X = pos.X * Game1.TileSize;
      this.Y = pos.Y * Game1.TileSize;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:sodamachine"], new Vector2(this.X, this.Y - Game1.TileSize).DrawPos(), null, Color.White, 0f,Vector2.Zero, 4f, SpriteEffects.None, Layer.Object - (Y + Game1.TileSize) / 1e5f);
      base.Draw(spriteBatch);
    }
  }
}
