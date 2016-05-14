using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoWorld2.UI.Menu;
using Microsoft.Xna.Framework.Input;
using ChaoWorld2.Items;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  class Chest : Entity, IContainer
  {
    public Item[] Inventory = new Item[16];

    public Chest(Vector2 pos)
      : this()
    {
      this.X = pos.X * Game1.TileSize;
      this.Y = pos.Y * Game1.TileSize;
    }

    public Chest(float x, float y)
      :this()
    {
      X = x;
      Y = y;
    }

    public Chest()
    {
      Collision.Add("Solid");
    }

    public override void Update(GameTime gameTime)
    {
      if (KeyboardUtil.KeyPressed(Keys.Enter) && Vector2.Distance(this.XandY, Game1.Player.XandY) < Game1.TileSize * 3)
        Game1.OpenMenu(new ChestInventory(this));
      base.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:chest"], XandY.DrawPos(), null,Color.White,0f,Vector2.Zero,2f, SpriteEffects.None, Layer.Object - (Y + Game1.TileSize) / 1e5f);
      base.Draw(spriteBatch);
    }

    public Item[] GetInventory()
    {
      return this.Inventory;
    }
  }
}
