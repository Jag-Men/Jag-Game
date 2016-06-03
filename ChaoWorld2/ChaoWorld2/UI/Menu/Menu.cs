using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaoWorld2.UI.Menu
{
  class Menu : IMenu
  {
    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.DrawString(ContentLibrary.Fonts["fonfman"], "dog", new Vector2(Game1.GameWidth / 2 - 30, Game1.GameHeight / 2 - 30), Color.Azure);
    }

    public void Update(GameTime gameTime)
    {
      if (KeyboardUtil.KeyPressed(Keys.Escape))
      {

        Game1.CloseMenu(); 
        return;
      }

    }
  }
}
