using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2
{
  class Animation
  {
    public static void Animate(string type, Entity name)
    {
      switch (type)
      {
        case "spin":
          name.ani += 0.1f;
          break;
        case "bspin":
          name.ani -= 0.1f;
          break;

      }
    }
  }
}
