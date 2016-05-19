using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoWorld2.Entities.Weapons
{
  public class Gunderwear : Crossbow
  {
    public Gunderwear()
    {
      weaponName = "wep:Gunderwear";
      weaponSize = 32;
      weaponScale = 1.75f;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      base.Draw(spriteBatch);
    }
  }
}
