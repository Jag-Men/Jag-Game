using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ChaoWorld2.Util;
using ChaoWorld2.UI.Menu;
using Microsoft.Xna.Framework.Input;

namespace ChaoWorld2.Entities
{
  class Jasmine : Entity
  {
    public Jasmine(float x, float y)
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
      this.framesUntilWalk = Game1.Random.Next(120, 480);
    }
    private Vector2 desiredPos = Vector2.Zero;
    private Vector2 move = Vector2.Zero;
    public int speed = 2;
    private int facing;
    private int frame;
    private double frameCount;
    private int framesUntilWalk;
    private bool grool;
    private int timesgrooled;
    private int emotion;
    private int timeN;
    private int timeB;

    private bool john = true;
    public string jajetron { get; private set; }

    public override void Update(GameTime gameTime)
    {

      if (KeyboardUtil.IsKeyDown(Keys.B))
        timeB += gameTime.ElapsedGameTime.Milliseconds;
      if (timeB > 0 && KeyboardUtil.IsKeyUp(Keys.B))
      {
        Animation.Animate("spin", this);
        timeB -= gameTime.ElapsedGameTime.Milliseconds;
      }
      if (KeyboardUtil.IsKeyDown(Keys.N))
        timeN += gameTime.ElapsedGameTime.Milliseconds;
      if (timeN > 0 && KeyboardUtil.IsKeyUp(Keys.N))
      {
        Animation.Animate("bspin", this);
        timeN -= gameTime.ElapsedGameTime.Milliseconds;
      }
      if (timeN == 0 && timeB == 0 && ani > 0)
        ani -= 0.01f;
      if (timeN == 0 && timeB == 0 && ani < 0)
        ani += 0.01f;
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


      if (move.Y > 0)
        this.facing = 1;
      if (move.Y < 0)
        this.facing = 0;
      if (move.X < 0)
        this.facing = 3;
      if (move.X > 0)
        this.facing = 2;
      this.frame = (int)Math.Floor(frameCount / 32.0) % 2;
      float joaje = Vector2.Distance(Game1.Player.XandY, this.XandY);
      if (KeyboardUtil.KeyPressed(Keys.Enter) && Game1.CurrentMenu == null && joaje <= Game1.TileSize * 1.5f)
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
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:Jasmine spritesheet"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 32, 16, 32), Color.White, this.ani, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - Y / 1e5f);
      base.Draw(spriteBatch);
    }
  }
}
