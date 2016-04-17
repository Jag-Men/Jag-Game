using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2
{
  public class Entity
  {
    public float X;
    public float Y;
    public int ID;
    public Vector2 XandY
    {
      get { return new Vector2(this.X, this.Y); }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    public virtual void Update() { }
    public virtual void UpdateEvenWhenPaused() { }
    public virtual void Draw(SpriteBatch spriteBatch) { }
  }
}
