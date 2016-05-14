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
  public class DialogueBox : IMenu
  {
    public string PortraitName;
    public string Text;
    public int Emotion;
    protected int PortraitSize;

    public DialogueBox(string portrait, string text, int emotion, int portraitSize = 128)
    {
      this.PortraitName = portrait;
      this.Text = text;
      this.Emotion = emotion;
      this.PortraitSize = portraitSize;
    }

    public void Update(GameTime gameTime)
    {
      if(KeyboardUtil.KeyPressed(Keys.Enter) || KeyboardUtil.KeyPressed(Keys.Escape))
      {
        Game1.CloseMenu();
        return;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Vector2 textPos = new Vector2(Game1.GameWidth / 2 - 30, Game1.GameHeight - (Game1.GameHeight * .34f));
      var backgo = ContentLibrary.Sprites["ui:backgo"];
      spriteBatch.Draw(ContentLibrary.Sprites["ui:backgo"], new Vector2(0, Game1.GameHeight - backgo.Height), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Menu);
      spriteBatch.Draw(ContentLibrary.Sprites["head:" + this.PortraitName], textPos - new Vector2(256, 0), new Rectangle((this.Emotion % 2) * this.PortraitSize, (int)Math.Floor((double)this.Emotion / 2) * this.PortraitSize, this.PortraitSize, this.PortraitSize), Color.White, 0f, Vector2.Zero, 256/this.PortraitSize, SpriteEffects.None, Layer.Menu - 1e-5f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fontman"], this.Text, textPos, Color.DeepPink, 0f, Vector2.Zero, 1f, SpriteEffects.None, Layer.Menu - 1e-5f);
    }
  }
}
