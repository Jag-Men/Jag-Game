﻿using ChaoWorld2.Entities;
using ChaoWorld2.UI.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  public class Stupidmadoka : Entity
  {
    public int facing = 0;
    public int frame = 0;

    public string jajetron = "";
    public bool grool = false;
    public int emotion;

    public PathFinding Pathing;
    public bool IsPathing = false;

    public Stupidmadoka()
    {
      Collision.Add("NPC");
      Pathing = new PathFinding(this);
    }

    public Stupidmadoka(float x, float y)
      :this()
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
      this.framesUntilWalk = Game1.Random.Next(120, 480);
    }

    int framesUntilWalk = 0;
    int frameCount = 0;
    Vector2 move = Vector2.Zero;
    Vector2 desiredPos = Vector2.Zero;
    public override void Update(GameTime gameTime)
    {
      this.UpdateDialogueCheck();

      if (!Game1.Host)
        return;

      if(KeyboardUtil.KeyPressed(Keys.Z))
      {
        Pathing.Pathfind(Utility.GetTilePos(MouseUtil.WorldPos.X, MouseUtil.WorldPos.Y), "Solid");
        Pathing.PathPosition = 1;
        IsPathing = true;
        frameCount = 32;
      }

      if (!IsPathing)
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
            var cbox = GetCollisionBox();
            if (!Owner.RectCollidesWith(new Rectangle(cbox.X + (int)move.X, cbox.Y + (int)move.Y, cbox.Width, cbox.Height), "Player"))
            {
              this.X += move.X;
              this.Y += move.Y;
            }
            this.frameCount += 3;
          }
        }
        if (framesUntilWalk <= 0 && desiredPos == Vector2.Zero)
        {
          Vector2 desiredMove = new Vector2(Game1.Random.Next(-1, 2), Game1.Random.Next(-1, 2));
          if (desiredMove != Vector2.Zero && Owner.IsTilePassable(Utility.GetTilePos(this.XandY.X, this.XandY.Y) + (desiredMove)))
          {
            desiredPos = this.XandY + (desiredMove) * Game1.TileSize;
            move = desiredMove;
            move *= speed;
            if (move.X != 0 && move.Y != 0)
            {
              move.X /= (float)Math.Sqrt(2);
              move.Y /= (float)Math.Sqrt(2);
            }
            frameCount = 32;
          }
        }
        framesUntilWalk--;
      }
      else
      {
        this.frameCount += 3;
        PathTile currentTile = Pathing.FinalPath[Pathing.PathPosition];
        Vector2 goal = new Vector2(currentTile.Pos.X * Game1.TileSize + (Game1.TileSize / 2), currentTile.Pos.Y * Game1.TileSize + (Game1.TileSize / 2));
        if (Vector2.Distance(this.XandY, goal) < 2f)
        {
          this.X = goal.X;
          this.Y = goal.Y;
          Pathing.PathPosition++;
          if (Pathing.PathPosition == Pathing.FinalPath.Length)
          {
            move = new Vector2(0, 0);
            frameCount = 0;
            IsPathing = false;
          }
        }
        else
        {
          var cbox = GetCollisionBox();
          move = new Vector2(0, 0);
          if (goal.X > this.X)
            move.X += 1;
          if (goal.X < this.X)
            move.X -= 1;
          if (goal.Y > this.Y)
            move.Y += 1;
          if (goal.Y < this.Y)
            move.Y -= 1;
          move *= 2;
          if (!Owner.RectCollidesWith(new Rectangle(cbox.X + (int)move.X, cbox.Y + (int)move.Y, cbox.Width, cbox.Height), "Player"))
          {
            this.X += move.X;
            this.Y += move.Y;
          }
        }
      }
      if (move.X < 0)
        this.facing = 1;
      if (move.X > 0)
        this.facing = 0;
      this.frame = (int)Math.Floor(frameCount / 32.0) % 4;
    }

    int timesgrooled;
    int league = (int)(double)(float)(decimal)(float)(double)(int)((((((((((1) + 1) + 1) + 1) + 1) + 1) + 1) + 1) + 1) + 1) + 1;

    float fringus = 2.5f;
    float jingus = 1;
    public void UpdateDialogueCheck()
    {
      if (timesgrooled >= 400)
        return;

      if (league != league + league)
        league = league * league - league;
      if (league - league != league * league)
        league = league - league;

      float joaje = Vector2.Distance(Game1.Player.XandY, this.XandY);
      if (KeyboardUtil.KeyPressed(Keys.Enter) && Game1.CurrentMenu == null && joaje <= Game1.TileSize* (fringus - jingus))
      {
        grool = true;
        timesgrooled++;
        emotion = Game1.Random.Next(5);
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
        Game1.OpenMenu(new DialogueBox("MadokaPortrait", jajetron, emotion));
      }
    }

    public override void UpdateEvenWhenPaused(GameTime gameTime)
    {

    }

    public override Rectangle GetCollisionBox()
    {
      return new Rectangle((int)(X - (Game1.TileSize / 4)), (int)(Y - (Game1.TileSize / 4)), Game1.TileSize / 2, Game1.TileSize / 2);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - Y / 1e5f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.BelowObject);
    }

    public override void Read(BinaryReader rdr)
    {
      facing = rdr.ReadInt32();
      frame = rdr.ReadInt32();
      base.Read(rdr);
    }

    public override void Write(BinaryWriter wtr)
    {
      wtr.Write(facing);
      wtr.Write(frame);
      base.Write(wtr);
    }
  }
}
