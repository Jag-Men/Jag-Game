using ChaoWorld2.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  public class ItemDrop : Entity
  {
    public Item Item;

    public ItemDrop() { }

    public ItemDrop(Item item, float x, float y)
      :this()
    {
      this.X = x;
      this.Y = y;
      this.Item = item;
    }

    public override Rectangle GetCollisionBox()
    {
      return new Rectangle((int)X - 8, (int)Y - 8, 16, 16);
    }

    public override void Update(GameTime gameTime)
    {
      float py = Game1.Player.Y - this.Y;
      float px = Game1.Player.X - this.X;

      this.X += 3 * (float)Math.Cos(Math.Atan2(py, px));
      this.Y += 3 * (float)Math.Sin(Math.Atan2(py, px));

      if(Game1.Player.GetCollisionBox().Intersects(GetCollisionBox()))
      {
        for(int i = 0; i < Game1.Player.Inventory.Length; i++)
        {
          var item = Game1.Player.Inventory[i];
          if(item == null)
          {
            Game1.Player.Inventory[i] = this.Item;
            Game1.PlaySound("pickup", 0.4f);
            Owner.RemoveEntity(this);
            return;
          }
        }
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      Vector2 spriteSize = this.Item.GetSpriteSize();
      spriteBatch.Draw(ContentLibrary.Sprites["item:" + this.Item.Texture], new Vector2(this.X - spriteSize.X / 2, this.Y - spriteSize.Y / 2).DrawPos(), this.Item.TexSource, Color.White, 0, Vector2.Zero, this.Item.Scale, SpriteEffects.None, Layer.Object - (Y / 1e5f) - (X / 1e8f));
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 4), Y - (Game1.TileSize / 8)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom / 2, SpriteEffects.None, Layer.BelowObject);
    }
  }
}
