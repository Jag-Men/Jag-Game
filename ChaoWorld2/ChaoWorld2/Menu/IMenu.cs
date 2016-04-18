using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Menu
{
  public interface IMenu
  {
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
  }
}
