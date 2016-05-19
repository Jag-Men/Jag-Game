using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Entities
{
  public class Weapon
  {
    public Weapon() { }
    public virtual void Update(GameTime gameTime) { }
    public virtual void UpdateEvenWhenPaused(GameTime gameTime) { }
    public virtual void Draw(SpriteBatch spriteBatch){ }
  }
}
