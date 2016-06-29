using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  class WWolf : Entity
  {
    public WWolf(float x, float y)
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
      this.framesUntilWalk = Game1.Random.Next(120, 480);
    }
    public Vector2 desiredpos;
    public int doggus;
    public bool grooool;
    private Vector2 move;
    private int framesUntilWalk;
    private int facing;
    private int frame;
    private Vector2 desiredPos;
    private int frameCount;

    public override void Update(GameTime gameTime)
    {
      this.frameCount += 3;
      this.frame = (int)Math.Floor(frameCount / 32.0) % 2;

      float prevX = this.X;
      float prevY = this.Y;

      float py = Game1.Player.Y - this.Y;
      float px = Game1.Player.X - this.X;

      float desiredX = 2 * (float)Math.Cos(Math.Atan2(py, px));
      float desiredY = 2 * (float)Math.Sin(Math.Atan2(py, px));
      if (this.Y + desiredY > prevY)
        this.facing = 0;
      else if (this.Y + desiredY < prevY)
        this.facing = 3;
      if (this.X + desiredX > prevX)
        this.facing = 1;
      else if (this.X + desiredX < prevX)
        this.facing = 2;

      var cbox = GetCollisionBox();
      foreach (var i in Owner.GetEntitiesInside(new Rectangle(cbox.X + (int)desiredX, cbox.Y + (int)desiredY, cbox.Width, cbox.Height), "Player", "NPC"))
        if (i != this)
          return;

      this.X += desiredX;
      this.Y += desiredY;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:wwolf"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 64, this.facing * 64, 64, 64), Color.White, this.ani, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - Y / 1e5f);
      base.Draw(spriteBatch);
    }
  }
}
