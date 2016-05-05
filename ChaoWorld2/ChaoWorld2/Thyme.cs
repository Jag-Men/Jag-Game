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
    public const int MSPerMinute = 500;

    static int timenoc;
    public static int minutes = 0;
    public static int hours = 6;
    public static string ampm = "am";
    public static long totalMinutes = 240;
    public static Color light = new Color(Color.White.ToVector3());
    public static bool active = true;
    public static void Update(GameTime gameTime)
    {
      if (active)
      {
        timenoc += gameTime.ElapsedGameTime.Milliseconds;
        if (timenoc >= MSPerMinute)
        {
          timenoc = 0;
          minutes++;
          if (hours <= 1)
            hours = 1;
          if (hours >= 2)
            totalMinutes++;
        }
        if (hours <= 1)
          hours = 1;
        if (minutes >= 60)
        {
          minutes = 0;
          hours++;
          if(hours == 12)
          {
            if (ampm == "am")
              ampm = "pm";
            else if (ampm == "pm")
              ampm = "am";
          }
        }
        if (hours > 12)
        {
          totalMinutes = 0;
          hours = 1;
        }
      }
      
      if(ampm == "am")
      {
        int fullTime = (60 * 10);
        light.R = (hours >= 2 && hours != 12) ? (byte)(100 + (int)(((float)Thyme.totalMinutes / fullTime) * 155)) : (byte)100;
        light.G = (hours >= 2 && hours != 12) ? (byte)(100 + (int)(((float)Thyme.totalMinutes / fullTime) * 155)) : (byte)100;
      }
      else if(ampm == "pm")
      {
        int fullTime = (60 * 10);
        light.R = (hours >= 2 && hours != 12) ? (byte)(255 - (int)(((float)Thyme.totalMinutes / fullTime) * 155)) : (byte)255;
        light.G = (hours >= 2 && hours != 12) ? (byte)(255 - (int)(((float)Thyme.totalMinutes / fullTime) * 155)) : (byte)255;
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
