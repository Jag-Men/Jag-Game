using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaoWorld2.Entities
{
  public class Arrow : Entity
  {
    void arrowmove(bool brool) 
    {
      if (brool == true)
      {

      }
    }
    bool groolean;
    int jagxcache;
    int jagycache;
    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (Mouse.GetState().LeftButton == ButtonState.Pressed)
      {
        groolean = true;

      }


    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      if (groolean == true)
      spriteBatch.Draw(ContentLibrary.Sprites["arrow"], new Vector2(0, 0), Color.White);
    }
  }
}
