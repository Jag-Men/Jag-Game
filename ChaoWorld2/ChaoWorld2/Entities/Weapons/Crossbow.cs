using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities.Weapons
{
  class Crossbow : Weapon
  {
    public string weaponName = "wep:crossman";
    public bool chargingShot;
    public bool lockedShot;
    public int timeUntilJoj;
    public int joj;
    public override void Update(GameTime gameTime)
    {
      if (MouseUtil.ButtonPressed(MouseButton.LeftButton) && !lockedShot && !chargingShot)
        chargingShot = true;
      if (MouseUtil.IsButtonUp(MouseButton.LeftButton) && !lockedShot && chargingShot)
      {
        joj = 0;
        timeUntilJoj = 0;
        chargingShot = false;
      }
      if (chargingShot || lockedShot)
      {
        if (MouseUtil.X < Game1.Player.XandY.DrawPos().X)
          Game1.Player.facing = 1;
        else if (MouseUtil.X > Game1.Player.XandY.DrawPos().X)
          Game1.Player.facing = 0;
        if (joj < 3)
        {
          timeUntilJoj += gameTime.ElapsedGameTime.Milliseconds;
          if (timeUntilJoj >= 200)
          {
            joj++;
            timeUntilJoj = 0;
          }
        }
        if (joj == 3 && !lockedShot)
        {
          chargingShot = false;
          lockedShot = true;
          Game1.PlaySound("lock");
        }
      }
      if (MouseUtil.ButtonPressed(MouseButton.LeftButton) && lockedShot)
      {
        double my = MouseUtil.Y - Game1.Player.XandY.DrawPos().Y;
        double mx = MouseUtil.X - Game1.Player.XandY.DrawPos().X;
        Arrow arrow = new Arrow(Math.Atan2(my, mx), 16, 1000);
        arrow.X = Game1.Player.X;
        arrow.Y = Game1.Player.Y;
        Game1.Player.Owner.AddEntity(arrow);
        Game1.PlaySound("shoot");
        lockedShot = false;
        joj = 0;
        timeUntilJoj = 0;
      }
      if (MouseUtil.IsButtonUp(MouseButton.LeftButton) && !lockedShot)
      {
        joj = 0;
        timeUntilJoj = 0;
      }
      base.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      Vector2 bowdirection = new Vector2();
      float joaje;
      switch (Game1.Player.frame)
      {
        case 0:
          joaje = 0;
          break;
        case 1:
          joaje = 4;
          break;
        case 2:
          joaje = 0;
          break;
        case 3:
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
      if (chargingShot || lockedShot)
      {
        if (Game1.Player.facing == 0)
          spriteBatch.Draw(ContentLibrary.Sprites[weaponName], new Vector2(Game1.Player.X + 13 + joaje, Game1.Player.Y - 67).DrawPos(), new Rectangle(joj * 16, 0, 16, 16), Color.White, 0, Vector2.Zero, 3.5f * (Game1.PixelZoom / 4), SpriteEffects.None, Layer.Object - (Game1.Player.Y + 1) / 1e5f);
        if (Game1.Player.facing == 1)
          spriteBatch.Draw(ContentLibrary.Sprites[weaponName], new Vector2(Game1.Player.X + 50 + -90 + joaje, Game1.Player.Y - 67).DrawPos(), new Rectangle(joj * 16, 0, 16, 16), Color.White, 0, Vector2.Zero, 3.5f * (Game1.PixelZoom / 4), SpriteEffects.FlipHorizontally, Layer.Object - (Game1.Player.Y + 1) / 1e5f);
      }
      else
      {
        if (Game1.Player.facing == 0)
          spriteBatch.Draw(ContentLibrary.Sprites[weaponName], new Vector2(Game1.Player.X + 50 + joaje, Game1.Player.Y - 50).DrawPos(), new Rectangle(joj * 16, 0, 16, 16), Color.White, -30, Vector2.Zero, 3.5f * (Game1.PixelZoom / 4), SpriteEffects.None, Layer.Object - (Game1.Player.Y + 1) / 1e5f);
        if (Game1.Player.facing == 1)
          spriteBatch.Draw(ContentLibrary.Sprites[weaponName], new Vector2(Game1.Player.X + 58 + -90 + joaje, Game1.Player.Y + 10).DrawPos(), new Rectangle(joj * 16, 0, 16, 16), Color.White, 30, Vector2.Zero, 3.5f * (Game1.PixelZoom / 4), SpriteEffects.FlipHorizontally, Layer.Object - (Game1.Player.Y + 1) / 1e5f);
      }
      base.Draw(spriteBatch);
    }
    }
  }
