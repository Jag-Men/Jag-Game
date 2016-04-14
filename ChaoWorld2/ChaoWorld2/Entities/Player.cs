using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace ChaoWorld2.Entities
{
  public class Player
  {
    public float X;
    public float Y;
    public int facing;
    public int frame;

    public Player(float x, float y)
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
      this.facing = 0;
      this.frame = 0;
    }

    int frameCount = 0;
    public void Update()
    {
      Vector2 move = new Vector2(0, 0);
      if (KeyboardUtil.IsKeyDown(Keys.W))
        move.Y -= 3;
      if (KeyboardUtil.IsKeyDown(Keys.S))
        move.Y += 3;
      if (KeyboardUtil.IsKeyDown(Keys.A))
        move.X -= 3;
      if (KeyboardUtil.IsKeyDown(Keys.D))
        move.X += 3;
      if(move.X != 0 && move.Y != 0)
      {
        move.X /= (float)Math.Sqrt(2);
        move.Y /= (float)Math.Sqrt(2);
      }
      if (move.X != 0 || move.Y != 0)
        if (this.frameCount == 0)
          this.frameCount = 32;
        else
          this.frameCount += 3;
      else
        this.frameCount = 0;
      if (move.X > 0)
        this.facing = 0;
      if (move.X < 0)
        this.facing = 1;
      this.frame = (int)Math.Floor(frameCount / 32.0) % 4;

      if (move.X != 0 && move.Y != 0)
      {
        TmxLayerTile tileX = Game1.GetTileAt(this.X + move.X, this.Y);
        TmxLayerTile tileY = Game1.GetTileAt(this.X, this.Y + move.Y);
        if (tileX != null && tileX.Gid != 0)
          this.X += move.X;
        if (tileY != null && tileY.Gid != 0)
          this.Y += move.Y;
      }
      else
      {
        TmxLayerTile tile = Game1.GetTileAt(this.X + move.X, this.Y + move.Y);
        if (tile != null && tile.Gid != 0)
        {
          this.X += move.X;
          this.Y += move.Y;
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.51f);
    }
  }
}
