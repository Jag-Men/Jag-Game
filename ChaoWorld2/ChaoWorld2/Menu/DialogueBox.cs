﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChaoWorld2.Menu
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
      if(KeyboardUtil.KeyPressed(Keys.Enter))
      {
        Game1.CloseMenu();
        return;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Vector2 textPos = new Vector2(Game1.GameWidth / 2 - 30, Game1.GameHeight - (Game1.GameHeight * .34f));
      spriteBatch.Draw(ContentLibrary.Sprites[this.PortraitName], textPos - new Vector2(256, 0), new Rectangle((this.Emotion % 2) * this.PortraitSize, (int)Math.Floor((double)this.Emotion / 2) * this.PortraitSize, this.PortraitSize, this.PortraitSize), Color.White, 0f, Vector2.Zero, 256/this.PortraitSize, SpriteEffects.None, 0.00001f);
      spriteBatch.DrawString(ContentLibrary.Fonts["fontman"], this.Text, textPos, Color.DeepPink, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.00001f);
    }
  }
}