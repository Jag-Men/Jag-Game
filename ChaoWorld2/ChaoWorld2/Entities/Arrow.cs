using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaoWorld2.Entities
{
  public class Arrow : Entity
  {
    public double Angle;
    public int Speed;
    public int Lifetime;

    public Arrow() { }
    public Arrow(double angle, int speed, int lifetime)
    {
      this.Angle = angle;
      this.Speed = speed;
      this.Lifetime = lifetime;
    }

    public override void Update(GameTime gameTime)
    {
      if (!Game1.Host)
        return;

      foreach (var i in Game1.GetEntitiesInside(this.GetCollisionBox()))
        if(i is Enemy)
        {
          int dmg = Game1.Random.Next(5, 16);
          (i as Enemy).Damage(dmg);
          Game1.RemoveEntity(this);
          return;
        }

      this.Lifetime -= gameTime.ElapsedGameTime.Milliseconds;
      if (this.Lifetime <= 0)
      {
        Game1.RemoveEntity(this);
        return;
      }
      this.X += (float)Math.Round(this.Speed * Math.Cos(this.Angle));
      this.Y += (float)Math.Round(this.Speed * Math.Sin(this.Angle));
    }

    public override Rectangle GetCollisionBox()
    {
      return new Rectangle((int)(X - (Game1.TileSize / 2)), (int)(Y - (Game1.TileSize / 4)), Game1.TileSize, Game1.TileSize / 2);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["arrow"], new Vector2(X, Y - (Game1.TileSize / 2)).DrawPos(), null, Color.White, (float)this.Angle, new Vector2(8, 8), 3f * (Game1.PixelZoom / 4), SpriteEffects.None, 0.5f - Y / 100000f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.51f);
    }
  }
}
