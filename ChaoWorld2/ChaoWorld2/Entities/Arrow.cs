using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoWorld2.Util;

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

      if(!Owner.IsTilePassable(Utility.GetTilePos(X, Y)))
      {
        Owner.AddEntity(new Explosion(X, Y - (Game1.TileSize / 2)));
        Owner.RemoveEntity(this);
        return;
      }

      foreach (var i in Owner.GetEntitiesInside(this.GetCollisionBox(), "NPC"))
        if(i is Enemy)
        {
          int dmg = Game1.Random.Next(5, 16);
          (i as Enemy).Damage(dmg);
          Owner.RemoveEntity(this);
          return;
        }
        else if(i is Stupidmadoka)
        {
          Owner.AddEntity(new Explosion(i.X, i.Y - (Game1.TileSize / 2)));
          Owner.RemoveEntity(i);
          Owner.RemoveEntity(this);
          Game1.PlaySound("madoka_death", 0.2f);
          return;
        }

      if (Owner.RectCollidesWith(this.GetCollisionBox(), "Solid"))
      {
        Owner.RemoveEntity(this);
        return;
      }

      this.Lifetime -= gameTime.ElapsedGameTime.Milliseconds;
      if (this.Lifetime <= 0)
      {
        Owner.RemoveEntity(this);
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
      spriteBatch.Draw(ContentLibrary.Sprites["wep:arrow"], new Vector2(X, Y - (Game1.TileSize / 2)).DrawPos(), null, Color.White, (float)this.Angle, new Vector2(8, 8), 3f * (Game1.PixelZoom / 4), SpriteEffects.None, Layer.Object - Y / 1e5f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.BelowObject);
    }
  }
}
