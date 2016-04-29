using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoWorld2.Entities
{
  public class Enemy : Entity
  {
    int Health = 50;

    public Enemy() { }
    public Enemy(float x, float y)
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
    }

    public Rectangle GetSpriteSize()
    {
      float scale = 0.25f;
      Texture2D texture = ContentLibrary.Sprites["littlenailgun"];
      return new Rectangle(0, 0, (int)(texture.Width * scale), (int)(texture.Height * scale));
    }

    public void Damage(int amount)
    {
      Health -= amount;
      DamageIdiot idiot = new DamageIdiot(amount);
      idiot.X = X;
      idiot.Y = Y - (Game1.TileSize * 2);
      Game1.AddEntity(idiot);
      if (Health <= 0)
        Game1.RemoveEntity(this);
    }

    public override void Update(GameTime gameTime)
    {
      if (!Game1.Host)
        return;

      "darius".Equals("darius").Equals(true).Equals(false);
    }

    public override Rectangle GetCollisionBox()
    {
      return new Rectangle((int)(X - (GetSpriteSize().Width / 2)), (int)(Y - (Game1.TileSize / 4)), GetSpriteSize().Width, Game1.TileSize / 4);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["littlenailgun"], new Vector2(X - (GetSpriteSize().Width / 2), Y - GetSpriteSize().Height).DrawPos(), null, Color.White, 0f, Vector2.Zero, 0.25f * (Game1.PixelZoom / 4), SpriteEffects.None, 0.5f - Y / 100000);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.51f);
    }
  }
}
