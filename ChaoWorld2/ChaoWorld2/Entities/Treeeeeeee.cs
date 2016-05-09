using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ChaoWorld2.Util;

namespace ChaoWorld2.Entities
{
  class Treeeeeeee : Entity
  {
    public float Alpha = 1f;

    public Treeeeeeee()
    {
      Collision.Add("Solid");
    }

    public Treeeeeeee(Vector2 pos)
      :this()
    {
      this.X = pos.X * Game1.TileSize;
      this.Y = pos.Y * Game1.TileSize;
    }

    bool underneath = false;
    public override void UpdateEvenWhenPaused(GameTime gameTime)
    {
      Rectangle hideRect = new Rectangle((int)this.X - (Game1.TileSize), (int)this.Y - (Game1.TileSize * 4), Game1.TileSize * 3, Game1.TileSize * 3);
      Rectangle plrRect = new Rectangle((int)Game1.Player.X - (Game1.TileSize / 2) + 16, (int)Game1.Player.Y - (int)(Game1.TileSize * 0.8f), Game1.TileSize - 32, (int)(Game1.TileSize * 0.8f));
      if (plrRect.Intersects(hideRect))
        underneath = true;
      else
        underneath = false;

      if (underneath && this.Alpha > 0.5f)
        this.Alpha -= 0.05f;
      if (!underneath && this.Alpha < 1f)
        this.Alpha += 0.05f;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      var sprite = ContentLibrary.Sprites["ent:Tree"];
      spriteBatch.Draw(sprite, new Vector2(this.X - Game1.TileSize, this.Y - (sprite.Height * 4) + Game1.TileSize).DrawPos(), null, Color.White * this.Alpha, 0, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - ((Y + Game1.TileSize / 2) / 1e5f) - ((X + Game1.TileSize / 2) / 1e8f));
      base.Draw(spriteBatch);
    }
  }
}
