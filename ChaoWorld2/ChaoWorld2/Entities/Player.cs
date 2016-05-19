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
using ChaoWorld2.Util;
using ChaoWorld2.Items;
using ChaoWorld2.Entities.Weapons;

namespace ChaoWorld2.Entities
{
  public class Player : Entity, IContainer
  {
    public int god;
    public int facing;
    public int frame;
    public int health = 115;
    public int maxhealth = 115;
    public Weapon weapon;

    public float speedMult = 1;

    public Item[] Inventory = new Item[16];
    
    public Player()
    {

      Collision.Add("Player");
    }

    public Player(float x, float y)
      :this()
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
      this.facing = 0;
      this.frame = 0;
    }

    int joj;
    int timeUntilJoj = 0;
    int frameCount = 0;
    bool chargingShot = false;
    bool lockedShot = false;
    bool siegeOfMadoka = false;
    int somSpawnTime = 0;
    public override void Update(GameTime gameTime)
    {
      if (this != Game1.Player)
        return;

      if (KeyboardUtil.KeyPressed(Keys.Q))
        weapon = new Crossbow();
      if (KeyboardUtil.KeyPressed(Keys.L))
        weapon = new Gunderwear();

      if (KeyboardUtil.KeyPressed(Keys.H))
        health--;
      if (health<0)
        health++;

      if (siegeOfMadoka)
      {
        somSpawnTime += gameTime.ElapsedGameTime.Milliseconds;
        if (somSpawnTime >= 1750)
        {
          var rsm = new Reallystupidmadoka();
          Owner.AddEntity(rsm);
          somSpawnTime = 0;
        }
        float shortestDist = Game1.TileSize * 16;
        foreach (var i in Owner.Entities.Values)
          if(i is Reallystupidmadoka)
          {
            float dist = Vector2.Distance(i.XandY, this.XandY);
            if (dist < shortestDist)
              shortestDist = dist;
          }
        Music.Volume(Math.Max(0, (1 - (shortestDist / (Game1.TileSize * 14))) * 0.3f));
      }

      if (KeyboardUtil.KeyPressed(Keys.F2))
      {
        var tilePos = Utility.GetTilePos(X, Y);
        Owner.AddEntity(new Stupidmadoka(tilePos.X, tilePos.Y));
      }
      if (KeyboardUtil.KeyPressed(Keys.F3))
      {
        var tilePos = Utility.GetTilePos(X, Y);
        Owner.AddEntity(new Enemy(tilePos.X, tilePos.Y));
      }
      if (KeyboardUtil.KeyPressed(Keys.F4))
      {
        siegeOfMadoka = !siegeOfMadoka;
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
      if (move.X > 0)
        this.facing = 0;
      if (move.X < 0)
        this.facing = 1;

      if (weapon != null)
        weapon.Update(gameTime);

      move *= (speed * speedMult);
      if (move.X != 0 && move.Y != 0)
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
      this.frame = (int)Math.Floor(frameCount / 32.0) % 4;

      if(speed == 24)
        Owner.AddEntity(new FakePlayer(this.X, this.Y, this.facing, this.frame));

      string[] collisions = new string[] { "Solid", "NPC" };
      var cbox = GetCollisionBox();
      if (move.X != 0 && move.Y != 0)
      {
        float prevX = this.X;
        float prevY = this.Y;
        if (!Owner.RectCollidesWith(new Rectangle(cbox.X + (int)move.X, cbox.Y, cbox.Width, cbox.Height), collisions))
          this.X += move.X;
        if (!Owner.RectCollidesWith(new Rectangle(cbox.X, cbox.Y + (int)move.Y, cbox.Width, cbox.Height), collisions))
          this.Y += move.Y;
        if (Owner.RectCollidesWith(GetCollisionBox(), collisions))
        {
          this.X = prevX;
          this.Y = prevY;
        }
      }
      else
      {
        float prevX = this.X;
        float prevY = this.Y;
        this.X += move.X;
        this.Y += move.Y;
        if (Owner.RectCollidesWith(GetCollisionBox(), collisions))
        {
          this.X = prevX;
          this.Y = prevY;
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

    public override Rectangle GetCollisionBox()
    {
      return new Rectangle((int)X - (Game1.TileSize / 2) + 4, (int)Y - (Game1.TileSize / 4), Game1.TileSize - 8, Game1.TileSize / 2);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["ent:dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - Y / 1e5f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.BelowObject);
      this.DrawHealthBar(spriteBatch);
      if (weapon != null)
        weapon.Draw(spriteBatch);
      this.jon("tronss",spriteBatch);
    }

    void jon(string trons, SpriteBatch spriteBatch)
    {
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], trons, Vector2.Zero, Color.White);
    }

    void DrawHealthBar(SpriteBatch spriteBatch)
    {
      Vector2 barPos = new Vector2(Game1.GameWidth - 276, Game1.GameHeight - 52);
      spriteBatch.Draw(ContentLibrary.Sprites["ui:hp"], barPos + new Vector2(28, 0), new Rectangle(0, 0, 2 * (int)(((double)health / maxhealth) * 100), 32), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, Layer.Menu + 3e-3f);
      spriteBatch.Draw(ContentLibrary.Sprites["ui:superfancyhpbar1"], barPos, new Rectangle(0, 0, 128, 16), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, Layer.Menu + 2e-3f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], health + "", barPos + new Vector2(228, 0), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, Layer.Menu + 1e-3f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], "__", barPos + new Vector2(230, 2), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, Layer.Menu + 1e-3f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], maxhealth + "", barPos + new Vector2(228, 15), Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, Layer.Menu + 1e-3f);
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

    public bool HasEmptySlot()
    {
      for (int i = 0; i < Inventory.Length; i++)
        if (Inventory[i] == null)
          return true;
      return false;
    }

    public bool AddItem(Item item)
    {
      for (int i = 0; i < Inventory.Length; i++)
        if (Inventory[i] == null)
        {
          Inventory[i] = item;
          return true;
        }
      return false;
    }

    public Item[] GetInventory()
    {
      return this.Inventory;
    }
  }
}
