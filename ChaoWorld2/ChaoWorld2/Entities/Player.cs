using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;
using System.IO;
using System.Collections.Concurrent;

namespace ChaoWorld2.Entities
{
  public class Player : Entity
  {
    public int god;
    public int facing;
    public int frame;
    public int health = 115;
    public int maxhealth = 115;
    int joj;
    public Player() { }

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
      joj += gameTime.ElapsedGameTime.Milliseconds/1000;
      if (this != Game1.Player)
        return;

      if (KeyboardUtil.KeyPressed(Keys.H))
        health--;
      if (health<0)
        health++;

      if (KeyboardUtil.KeyPressed(Keys.F2))
      {
        var tilePos = Utility.GetTilePos(X, Y);
        Game1.AddEntity(new Stupidmadoka(tilePos.X, tilePos.Y));
      }

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
      if (Mouse.GetState().LeftButton == ButtonState.Pressed)
      {
        move.X /= 2;
        move.Y /= 2;
        if(Game1.Random.Next(20) == 0)
        {
          double my = Mouse.GetState().Y - this.XandY.DrawPos().Y;
          double mx = Mouse.GetState().X - this.XandY.DrawPos().X;
          Arrow arrow = new Arrow(Math.Atan2(my, mx), 8, 1000);
          arrow.X = this.X;
          arrow.Y = this.Y;
          Game1.AddEntity(arrow);
        }
      }
      if (move.X != 0 && move.Y != 0)
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

      if(speed == 24)
        Game1.AddEntity(new FakePlayer(this.X, this.Y, this.facing, this.frame));
      
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
      spriteBatch.Draw(ContentLibrary.Sprites["dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - Y / 100000f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.51f);
      this.DrawHealthBar(spriteBatch);
      this.DrawCrossBow(spriteBatch);
      this.jon("tronss",spriteBatch);
    }

    void jon(string trons, SpriteBatch spriteBatch)
    {
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], trons, Vector2.Zero, Color.White);
    }
    void DrawCrossBow(SpriteBatch spriteBatch)
    {
      Vector2 bowdirection = new Vector2();
      float joaje;
      switch (this.frame)
      {
        case 0 :
          joaje = 0;
          break;
        case 1 :
          joaje = 4;
          break;
        case 2 :
          joaje = 0;
          break;
        case 3 :
          joaje = -8;
          break;
        default:
          joaje = 0;
          break;
          
      }
      string byakuya = "morning naegi";
      if (byakuya == "morning naegi")
      {
      }
      if (Mouse.GetState().LeftButton == ButtonState.Pressed)
      {
        if (this.facing == 0)
          spriteBatch.Draw(ContentLibrary.Sprites["crossman"], new Vector2(X +13 + joaje, Y - 67).DrawPos(), new Rectangle(joj * 16, 0, 16, 16), Color.White, 0, Vector2.Zero, 3.5f, SpriteEffects.None, 0.5f - Y / 100000f);
        if (this.facing == 1)
          spriteBatch.Draw(ContentLibrary.Sprites["crossman"], new Vector2(X + 50 + -90 + joaje, Y - 67).DrawPos(), new Rectangle(joj * 16, 0, 16, 16), Color.White, 0, Vector2.Zero, 3.5f, SpriteEffects.FlipHorizontally, 0);
      }
      else
      {
        if (this.facing == 0)
          spriteBatch.Draw(ContentLibrary.Sprites["crossman"], new Vector2(X + 50 + joaje, Y - 50).DrawPos(), new Rectangle(joj*16, 0, 16, 16), Color.White, -30, Vector2.Zero, 3.5f, SpriteEffects.None, 0.5f - Y / 100000f);
        if (this.facing == 1)
          spriteBatch.Draw(ContentLibrary.Sprites["crossman"], new Vector2(X + 58 + -90 + joaje, Y + 10).DrawPos(), new Rectangle(joj * 16, 0, 16, 16), Color.White, 30, Vector2.Zero, 3.5f, SpriteEffects.FlipHorizontally, 0);
      }
    }

    void DrawHealthBar(SpriteBatch spriteBatch)
    {
      Vector2 barPos = new Vector2(Game1.GameWidth - 276, Game1.GameHeight - 52);
      spriteBatch.Draw(ContentLibrary.Sprites["hp"], barPos + new Vector2(28, 0), new Rectangle(0, 0, 2 * (int)(((double)health / maxhealth) * 100), 32), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.000022f);
      spriteBatch.Draw(ContentLibrary.Sprites["superfancyhpbar1"], barPos, new Rectangle(0, 0, 128, 16), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.000021f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], health + "", barPos + new Vector2(228, 0), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.00002f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], "__", barPos + new Vector2(230, 2), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.00002f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], maxhealth + "", barPos + new Vector2(228, 15), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.00002f);
    }

    public override void Read(BinaryReader rdr)
    {
      facing = rdr.ReadInt32();
      frame = rdr.ReadInt32();
      health = rdr.ReadInt32();
      maxhealth = rdr.ReadInt32();
      base.Read(rdr);
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(facing);
      wtr.Write(frame);
      wtr.Write(health);
      wtr.Write(maxhealth);
      base.Write(wtr);
    }
  }
}
