using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChaoWorld2.UI.Menu
{
  class Chestm : IMenu
  {
    private Rectangle dog;

    public void Draw(SpriteBatch spriteBatch)
    {

    }

    public void Update(GameTime gameTime)
    {
      dog = new Rectangle();
    }
  }
}
