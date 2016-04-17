using ChaoWorld2.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Entities
{
  public class Stupidmadoka
  {
    public float X;
    public float Y;
    
    public int facing = 0;
    public int frame = 0;
    public Vector2 XandY
    {
      get { return new Vector2(this.X, this.Y); }
      set
      {
        this.X = value.X;
        this.Y = value.Y;
      }
    }

    Vector2 baaaaaa = new Vector2(Game1.GameWidth / 2 - 30, Game1.GameHeight - (Game1.GameHeight * .34f));
    string jajetron = "";
    bool grool = false;
    float fringus = 2.5f;
    float jingus = 1;
    int emotion;

    public void Update()
    {
      
      
      float joaje;
      joaje = Vector2.Distance(Game1.Player.XandY, this.XandY);
      if (KeyboardUtil.KeyPressed(Keys.Enter) && joaje <= Game1.TileSize * (fringus - jingus))
      {


        grool = !grool;
        emotion = new Random().Next(5);
      }
      if (joaje >= Game1.TileSize * (fringus - jingus))
        grool = false;
      if (grool == true)
      {
        
        switch (emotion)
        {
          case 0:
            jajetron = "talk i guess?";
            break;
          case 1:
            jajetron = "happy";
            break;
          case 2:
            jajetron = "sad";
            break;
          case 3:
            jajetron = "meh";
              break;
          case 4:
            jajetron = "blush";

            break;

            
        }

      }

      else
        jajetron = "";
      
        

    }
    public Stupidmadoka(float x, float y)
    {
      this.X = x * Game1.TileSize + (Game1.TileSize / 2);
      this.Y = y * Game1.TileSize + (Game1.TileSize / 2);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      if (grool == true)
      {
        spriteBatch.Draw(ContentLibrary.Sprites["Ashley"], baaaaaa - new Vector2(256, 0), new Rectangle((this.emotion % 2) * 64, (int)Math.Floor((double)this.emotion / 2) * 64, 64, 64), Color.White, 0f,Vector2.Zero,4,SpriteEffects.None, 0);
        spriteBatch.DrawString(Game1.fontman, jajetron, baaaaaa, Color.DeepPink);
      }
      spriteBatch.Draw(ContentLibrary.Sprites["dogo"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize * 1.5f)).DrawPos(), new Rectangle(this.frame * 16, this.facing * 24, 16, 24), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - Y / 100000f);
      spriteBatch.Draw(ContentLibrary.Sprites["shadow"], new Vector2(X - (Game1.TileSize / 2), Y - (Game1.TileSize / 4)).DrawPos(), null, Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.51f);
    }
  }
}
