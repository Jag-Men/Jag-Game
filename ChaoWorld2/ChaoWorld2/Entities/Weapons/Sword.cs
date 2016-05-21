using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ChaoWorld2.Entities.Weapons
{
  class Sword : Weapon
  {
    string texturename = "wep:sword";
    int scaleX = 32;
    int scaleY = 32;
    float animation;
    bool swingin;
    public override void Update(GameTime gameTime)
    {
      if (swingin == false)
        animation = 70;
      base.Update(gameTime);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(ContentLibrary.Sprites[texturename], new Vector2(Game1.Player.X, Game1.Player.Y), new Rectangle(0, 0, scaleX, scaleY), Color.White, this.animation,new Vector2(0,0),0f, SpriteEffects.None, 0.000001f);
      base.Draw(spriteBatch);
    }
  }
}
