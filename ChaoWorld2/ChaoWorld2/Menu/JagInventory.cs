using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaoWorld2.Menu
{
  public class JagInventory : IMenu
  {
    public JagInventory()
    {

    }

    public void Update(GameTime gameTime)
    {
      if(KeyboardUtil.KeyPressed(Keys.E))
      {
        Game1.CloseMenu();
        return;
      }
      
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites["Untitled"], Vector2.Zero, Color.White);
    }
  }
}
