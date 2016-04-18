﻿using ChaoWorld2.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Entities
{
  public class Stupidmadoka : Entity
  {
    public int facing = 0;
    public int frame = 0;

    public string jajetron = "";
    public bool grool = false;
    public int emotion;

    public Stupidmadoka(float x, float y)
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
      this.framesUntilWalk = Game1.Random.Next(120, 480);
    }

    int framesUntilWalk = 0;
    int frameCount = 0;
    Vector2 move = Vector2.Zero;
    Vector2 desiredPos = Vector2.Zero;
    public override void Update()
    {
      int speed = 2;

      if (desiredPos != Vector2.Zero)
      {
        if (Vector2.Distance(this.XandY, desiredPos) < 2f)
        {
          this.X = desiredPos.X;
          this.Y = desiredPos.Y;
          framesUntilWalk = Game1.Random.Next(120, 480);
          move = Vector2.Zero;
          desiredPos = Vector2.Zero;
          frameCount = 0;
        }
        else
        {
          this.X += move.X;
          this.Y += move.Y;
          this.frameCount += 3;
        }
      }
      if (framesUntilWalk <= 0 && desiredPos == Vector2.Zero)
      {
        Vector2 desiredMove = new Vector2(Game1.Random.Next(-1, 2), Game1.Random.Next(-1, 2));
        if(desiredMove != Vector2.Zero && Game1.IsTilePassable(Utility.GetTilePos(this.XandY.X, this.XandY.Y) + (desiredMove)))
        {
          desiredPos = this.XandY + (desiredMove) * Game1.TileSize;
          move = desiredMove;
          if (move.X != 0 && move.Y != 0)
          {
            move.X /= (float)Math.Sqrt(2);
            move.Y /= (float)Math.Sqrt(2);
          }
          move *= speed;
          frameCount = 32;
        }
      }
      if (move.X < 0)
        this.facing = 1;
      if (move.X > 0)
        this.facing = 0;
      this.frame = (int)Math.Floor(frameCount / 32.0) % 4;

      framesUntilWalk--;
    }

    float fringus = 2.5f;
    float jingus = 1;
    public override void UpdateEvenWhenPaused()
    {
      float joaje = Vector2.Distance(Game1.Player.XandY, this.XandY);
      if (KeyboardUtil.KeyPressed(Keys.Enter) && joaje <= Game1.TileSize * (fringus - jingus))
      {
        grool = !grool;
        emotion = Game1.Random.Next(5);
        Game1.Paused = grool;
      }
      if (joaje >= Game1.TileSize * (fringus - jingus))
        grool = false;
      if (grool == true)
        switch (emotion)
        {
          case 0:
            jajetron = "manawyrm";
            break;
          case 1:
            jajetron = "happy";
            break;
          case 2:
            jajetron = "sad";
            break;
          case 3:
            jajetron = "o";
            break;
          case 4:
            jajetron = "angry";
            break;
        }
      else
        jajetron = "";
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (grool == true)
      {
        Vector2 textPos = new Vector2(Game1.GameWidth / 2 - 30, Game1.GameHeight - (Game1.GameHeight * .34f));
        spriteBatch.Draw(ContentLibrary.Sprites["MadokaPortrait"], textPos - new Vector2(256, 0), new Rectangle((this.emotion % 2) * 128, (int)Math.Floor((double)this.emotion / 2) * 128, 128, 128), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0.00001f);
        spriteBatch.DrawString(ContentLibrary.Fonts["fontman"], jajetron, textPos, Color.DeepPink, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00001f);
      }
      spriteBatch.Draw(ContentLibrary.Sprites["dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - Y / 100000f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.51f);
    }
  }
}
