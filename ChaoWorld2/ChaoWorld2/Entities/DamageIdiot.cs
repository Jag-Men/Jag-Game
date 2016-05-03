using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  public class DamageIdiot : Entity
  {
    int DamaaaaaageIdiot;
    float Alpha;

    public DamageIdiot() { }
    public DamageIdiot(int idiot)
    {
      this.DamaaaaaageIdiot = idiot;
      this.Alpha = 1;
    }

    public override void Update(GameTime gameTime)
    {
      this.Y -= 2;
      this.Alpha -= 0.05f;
      if (this.Alpha <= 0)
        Owner.RemoveEntity(this);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      string drawText = "-" + DamaaaaaageIdiot;
      var fonnman = ContentLibrary.Fonts["fonnman"];
      Vector2 textSize = fonnman.MeasureString(drawText) * Game1.PixelZoom;
      spriteBatch.DrawOutlinedString(ContentLibrary.Fonts["fonnman"], drawText, new Vector2(X - (textSize.X / 2), Y - (textSize.Y / 2)).DrawPos(), new Color(255, 0, 0, Alpha), 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.AboveObject, Color.Black, 1);
    }
  }
}
