using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoWorld2.Util;

namespace ChaoWorld2.UI.Menu
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
      spriteBatch.Draw(ContentLibrary.Sprites["ui:Untitled"],new Vector2(Game1.GameWidth/2-128*2, Game1.GameHeight/2 -128*2),new Rectangle(0,0,128,128),Color.White,0f,Vector2.Zero,new Vector2(4,4),SpriteEffects.None,Layer.Menu);
    }
  }
}
