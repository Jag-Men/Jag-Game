using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Util
{
  public class KeyboardEventArgs : EventArgs
  {
    public KeyboardEventArgs(Keys key)
    {
      Key = key;
    }

    public Keys Key;
  }
}
