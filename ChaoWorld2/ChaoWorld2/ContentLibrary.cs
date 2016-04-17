using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TiledSharp;

namespace ChaoWorld2
{
  public static class ContentLibrary
  {
    public static Dictionary<string, Texture2D> Sprites = new Dictionary<string, Texture2D>();
    public static Dictionary<string, GameMap> Maps = new Dictionary<string, GameMap>();
    public static Dictionary<string, Texture2D> Tilesets = new Dictionary<string, Texture2D>();
    public static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();

    public static void Init()
    {
      string pathBase = Game1.GameContent.RootDirectory + Path.DirectorySeparatorChar;
      foreach (var file in Directory.GetFiles(pathBase + "sprites"))
      {
        string fileName = Path.GetFileNameWithoutExtension(file);
        Sprites.Add(fileName, Game1.GameContent.Load<Texture2D>("sprites\\" + fileName));
      }
      foreach (var file in Directory.GetFiles(pathBase + "tilesets"))
      {
        string fileName = Path.GetFileNameWithoutExtension(file);
        Tilesets.Add(fileName, Game1.GameContent.Load<Texture2D>("tilesets\\" + fileName));
      }
      foreach (var file in Directory.GetFiles(pathBase + "maps"))
      {
        string fileName = Path.GetFileNameWithoutExtension(file);
        var map = new GameMap(file);
        Maps.Add(fileName, map);
      }
      foreach (var file in Directory.GetFiles(pathBase + "fonts"))
      {
        string fileName = Path.GetFileNameWithoutExtension(file);
        Fonts.Add(fileName, Game1.GameContent.Load<SpriteFont>("fonts\\" + fileName));
      }
    }
  }
}
