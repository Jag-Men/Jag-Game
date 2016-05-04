using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2
{
  public class Thyme
  {
    static int timenoc;
    static int minutes;
    static int hours;
    static string ampm;
    public static void Update(GameTime gameTime)
    {
      timenoc += gameTime.ElapsedGameTime.Milliseconds;
      if (timenoc >= 60000)
      {
        timenoc = 0;
        minutes++;

      }
      if (minutes >= 60)
      {
        minutes = 0;
        hours++;
      }
      if (hours >= 12)
      {
        hours = 1;
        if (ampm == "am")
          ampm = "pm";
        else if (ampm == "pm")
          ampm = "am";
          
      }
        
    }
    public static void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], "" + minutes, new Vector2(100, 100), Color.Pink);
    }
  }
}
