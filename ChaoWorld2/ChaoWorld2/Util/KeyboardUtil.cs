using ChaoWorld2.Util;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2
{
  public static class KeyboardUtil
  {
    public static event EventHandler<KeyboardEventArgs> onKeyDown;
    public static event EventHandler<KeyboardEventArgs> onKeyUp;

    private static Keys[] oldKeys;
    private static List<Keys> justPressed = new List<Keys>();
    private static List<Keys> justReleased = new List<Keys>();

    public static void Update()
    {
      if (oldKeys == null)
        oldKeys = Keyboard.GetState().GetPressedKeys();
      else
      {
        justPressed.Clear();
        justReleased.Clear();
        Keys[] newKeys = Keyboard.GetState().GetPressedKeys();
        foreach (var key in newKeys)
          if (!oldKeys.Contains(key))
          {
            justPressed.Add(key);
            if(onKeyDown != null)
              onKeyDown(null, new KeyboardEventArgs(key));
          }
        foreach (var key in oldKeys)
          if (!newKeys.Contains(key))
          {
            justReleased.Add(key);
            if(onKeyUp != null)
              onKeyUp(null, new KeyboardEventArgs(key));
          }
        oldKeys = newKeys;
      }
    }

    public static bool IsKeyDown(Keys key)
    {
      return Keyboard.GetState().IsKeyDown(key);
    }

    public static bool IsKeyUp(Keys key)
    {
      return Keyboard.GetState().IsKeyUp(key);
    }

    public static bool KeyPressed(Keys key)
    {
      return justPressed.Contains(key);
    }

    public static bool KeyReleased(Keys key)
    {
      return justReleased.Contains(key);
    }
  }
}
