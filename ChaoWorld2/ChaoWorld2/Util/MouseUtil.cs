using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Util
{
  public static class MouseUtil
  {
    public static ButtonState LeftButton { get { return Mouse.GetState().LeftButton; } }
    public static ButtonState RightButton { get { return Mouse.GetState().RightButton; } }
    public static ButtonState MiddleButton { get { return Mouse.GetState().MiddleButton; } }
    public static ButtonState XButton1 { get { return Mouse.GetState().XButton1; } }
    public static ButtonState XButton2 { get { return Mouse.GetState().XButton2; } }

    public static int X { get { return Mouse.GetState().X; } }
    public static int Y { get { return Mouse.GetState().Y; } }
    public static Vector2 XandY { get { return new Vector2(X, Y); } }
    public static Vector2 WorldPos { get { return XandY.WorldPos(); } }
    public static int ScrollWheelValue { get { return Mouse.GetState().ScrollWheelValue; } }

    public static List<MouseButton> DownButtons = new List<MouseButton>();
    public static List<MouseButton> UpButtons = new List<MouseButton>();
    public static List<MouseButton> PressedButtons = new List<MouseButton>();
    public static List<MouseButton> ReleasedButtons = new List<MouseButton>();

    private static bool Initialized = false;
    public static void Update()
    {
      PressedButtons.Clear();
      ReleasedButtons.Clear();

      UpdateButton(MouseButton.LeftButton, LeftButton);
      UpdateButton(MouseButton.RightButton, RightButton);
      UpdateButton(MouseButton.MiddleButton, MiddleButton);
      UpdateButton(MouseButton.XButton1, XButton1);
      UpdateButton(MouseButton.XButton2, XButton2);

      if(!Initialized)
      {
        PressedButtons.Clear();
        ReleasedButtons.Clear();
        Initialized = true;
      }
    }

    static void UpdateButton(MouseButton button, ButtonState state)
    {
      if(state == ButtonState.Pressed)
      {
        if(!DownButtons.Contains(button))
        {
          DownButtons.Add(button);
          PressedButtons.Add(button);
        }
        if(UpButtons.Contains(button))
          UpButtons.Remove(button);
      }
      else if(state == ButtonState.Released)
      {
        if(!UpButtons.Contains(button))
        {
          UpButtons.Add(button);
          ReleasedButtons.Add(button);
        }
        if (DownButtons.Contains(button))
          DownButtons.Remove(button);
      }
    }

    public static bool IsButtonDown(MouseButton button)
    {
      if (!Game1.Instance.IsActive)
        return false;
      return DownButtons.Contains(button);
    }

    public static bool IsButtonUp(MouseButton button)
    {
      if (!Game1.Instance.IsActive)
        return false;
      return UpButtons.Contains(button);
    }

    public static bool ButtonPressed(MouseButton button)
    {
      if (!Game1.Instance.IsActive)
        return false;
      return PressedButtons.Contains(button);
    }

    public static bool ButtonReleased(MouseButton button)
    {
      if (!Game1.Instance.IsActive)
        return false;
      return ReleasedButtons.Contains(button);
    }
  }

  public enum MouseButton
  {
    LeftButton,
    RightButton,
    MiddleButton,
    XButton1,
    XButton2
  }
}
