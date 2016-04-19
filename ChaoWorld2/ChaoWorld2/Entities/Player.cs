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
  public class Player : Entity
  {
    public int god;
    public int facing;
    public int frame;

    public List<FakePlayer> FakePlayers = new List<FakePlayer>();

    public Player(float x, float y)
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
      this.facing = 0;
      this.frame = 0;
    }

    int frameCount = 0;
    public override void Update(GameTime gameTime)
    {
      int speed = 3;
      if (KeyboardUtil.IsKeyDown(Keys.LeftShift))
        speed = 24;

      Vector2 move = new Vector2(0, 0);
      if (KeyboardUtil.IsKeyDown(Keys.W))
        move.Y -= 1;
      if (KeyboardUtil.IsKeyDown(Keys.S))
        move.Y += 1;
      if (KeyboardUtil.IsKeyDown(Keys.A))
        move.X -= 1;
      if (KeyboardUtil.IsKeyDown(Keys.D))
        move.X += 1;
      if(move.X != 0 && move.Y != 0)
      {
        move.X /= (float)Math.Sqrt(2);
        move.Y /= (float)Math.Sqrt(2);
      }
      move *= speed;
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

      List<FakePlayer> removedFakePlayers = new List<FakePlayer>();
      foreach(var i in FakePlayers)
      {
        i.Update();
        if (i.alpha <= 0)
          removedFakePlayers.Add(i);
      }
      foreach (var i in removedFakePlayers)
        FakePlayers.Remove(i);

      if(speed == 24)
        FakePlayers.Add(new FakePlayer(this.X, this.Y, this.facing, this.frame));
      
      if (move.X != 0 && move.Y != 0)
      {
        if (Game1.IsTilePassable(Utility.GetTilePos(this.X + move.X, this.Y)))
          this.X += move.X;
        if (Game1.IsTilePassable(Utility.GetTilePos(this.X, this.Y + move.Y)))
          this.Y += move.Y;
      }
      else
      {
        if (Game1.IsTilePassable(Utility.GetTilePos(this.X + move.X, this.Y + move.Y)))
        {
          this.X += move.X;
          this.Y += move.Y;
        }
      }
    }
    Color color;
    public override void UpdateEvenWhenPaused(GameTime gameTime)
    {
      if (KeyboardUtil.KeyPressed(Keys.E))
        color = Color.Blue;
      else if (KeyboardUtil.KeyReleased(Keys.E))
        color = Color.Purple;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      foreach (var i in FakePlayers)
        i.Draw(spriteBatch);
      spriteBatch.Draw(ContentLibrary.Sprites["dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - Y / 100000f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.51f);
    }
  }
}
