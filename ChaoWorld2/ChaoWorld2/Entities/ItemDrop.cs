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
    float Speed;

    public ItemDrop() { }

    public ItemDrop(Item item, float x, float y)
      :this()
    {
      this.X = x;
      this.Y = y;
      this.Item = item;
      this.Speed = 0;
    }

    public override Rectangle GetCollisionBox()
    {
      return new Rectangle((int)X - 8, (int)Y - 8, 16, 16);
    }

    public override void Update(GameTime gameTime)
    {
      if (Game1.Player.HasEmptySlot() && Vector2.Distance(Game1.Player.XandY, this.XandY) <= Game1.TileSize * 4)
      {
        if(Speed < 4f)
          Speed += 0.1f;
        if (Speed > 4f)
          Speed = 4f;
      }
      else
      {
        if (Speed > 0)
          Speed -= 0.2f;
        if (Speed < 0)
          Speed = 0;
      }

      float py = Game1.Player.Y - this.Y;
      float px = Game1.Player.X - this.X;

      this.X += Speed * (float)Math.Cos(Math.Atan2(py, px));
      this.Y += Speed * (float)Math.Sin(Math.Atan2(py, px));

      if (Game1.Player.GetCollisionBox().Intersects(GetCollisionBox()))
        if (Game1.Player.AddItem(this.Item))
        {
          Game1.PlaySound("pickup", 0.2f);
          Owner.RemoveEntity(this);
          return;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      Vector2 spriteSize = this.Item.GetSpriteSize();
      spriteBatch.Draw(ContentLibrary.Sprites["item:" + this.Item.Texture], new Vector2(this.X - spriteSize.X / 2, this.Y - spriteSize.Y / 2).DrawPos(), this.Item.TexSource, Color.White, 0, Vector2.Zero, this.Item.Scale, SpriteEffects.None, Layer.AboveObject - (Y / 1e5f) - (X / 1e8f));
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 4), Y - (Game1.TileSize / 8)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom / 2, SpriteEffects.None, Layer.BelowObject);
    }
  }
}
