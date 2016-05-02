using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaoWorld2.Entities
{
  public class Reallystupidmadoka : Stupidmadoka
  {
    public Reallystupidmadoka()
      :base()
    {
      this.X = -100;
      this.Y = -100;
    }

    bool initialized = false;
    int frameCount = 0;
    public override void Update(GameTime gameTime)
    {
      if (!Game1.Host)
        return;

      if(!initialized)
      {
        int side = Game1.Random.Next(4);
        int xtile = 0;
        int ytile = 0;
        if(side == 0 || side == 1)
        {
          xtile = Game1.Random.Next(-1, Game1.Map.Width + 1);
          if (side == 0)
          {
            ytile = -1;
            facing = 0;
          }
          if (side == 1)
          {
            ytile = Game1.Map.Height + 1;
            facing = 1;
          }
        }
        if(side == 2 || side == 3)
        {
          ytile = Game1.Random.Next(-1, Game1.Map.Height + 1);
          if (side == 2)
          {
            xtile = -1;
            facing = 0;
          }
          if (side == 3)
          {
            xtile = Game1.Map.Width;
            facing = 1;
          }
        }
        this.X = (xtile * Game1.TileSize) + (Game1.TileSize / 2);
        this.Y = (ytile * Game1.TileSize) + (Game1.TileSize / 2);
        initialized = true;
      }
      else
      {
        this.frameCount += 3;
        this.frame = (int)Math.Floor(frameCount / 32.0) % 4;

        float prevX = this.X;
        float prevY = this.Y;

        float py = Game1.Player.Y - this.Y;
        float px = Game1.Player.X - this.X;
        this.X += 2 * (float)Math.Cos(Math.Atan2(py, px));
        this.Y += 2 * (float)Math.Sin(Math.Atan2(py, px));

        if (this.X > prevX)
          this.facing = 0;
        else if (this.X < prevX)
          this.facing = 1;
      }
    }
  }
}
