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
    static string ampm = "am";
    public static void Update(GameTime gameTime)
    {
      timenoc += gameTime.ElapsedGameTime.Milliseconds;
      if (timenoc >= 1000)
      {
        timenoc = 0;
        minutes++;
        if (hours <= 1)
          hours = 1;
      }
      if (hours <= 1)
        hours = 1;
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
      if (minutes < 10)
      spriteBatch.DrawString(ContentLibrary.Fonts["fonfman"], ""+ hours + ":" +"0"+ minutes + (ampm), new Vector2(100, 100), Color.Pink);
      else
        spriteBatch.DrawString(ContentLibrary.Fonts["fonfman"], "" + hours + ":" + minutes + (ampm), new Vector2(100, 100), Color.Pink);
    }
  }
}
