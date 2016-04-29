using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
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
    public static Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();

    public static void Init()
    {
      Sprites = Game1.GameContent.LoadListContent<Texture2D>("sprites");
      Fonts = Game1.GameContent.LoadListContent<SpriteFont>("fonts");
      Sounds = Game1.GameContent.LoadListContent<SoundEffect>("sounds");

      string pathBase = Game1.GameContent.RootDirectory + Path.DirectorySeparatorChar;
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
    }
  }
}
