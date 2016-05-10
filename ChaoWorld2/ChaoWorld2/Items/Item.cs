using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Items
{
  public class Item
  {
    public static Item TestItem;
    public static Item Groomba;
    
    public static void Init()
    {
      TestItem = new Item().SetTexture("Item");
      Groomba = new Item().SetTexture("groomba").SetScale(4);
    }

    public string Name;
    public string Texture;
    public Rectangle? TexSource;
    public float Scale;

    public Item()
    {
      this.Name = "Unnamed";
      this.Texture = "Item";
      this.TexSource = null;
      this.Scale = 1;
    }

    public Item SetName(string name)
    {
      this.Name = name;
      return this;
    }

    public Item SetTexture(string texture)
    {
      this.Texture = texture;
      return this;
    }

    public Item SetTexSource(Rectangle? rect)
    {
      this.TexSource = rect;
      return this;
    }

    public Item SetTexSource(int x, int y, int width, int height)
    {
      return this.SetTexSource(new Rectangle(x, y, width, height));
    }

    public Item SetScale(float scale)
    {
      this.Scale = scale;
      return this;
    }
  }
}
