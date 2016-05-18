using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TiledSharp;

namespace ChaoWorld2
{
  public static class Utility
  {
    public static TmxTileset GetTilesetForTile(TmxMap map, TmxLayerTile tile)
    {
      TmxTileset currentTileset = null;
      foreach (var tileset in map.Tilesets.OrderByDescending(_ => _.FirstGid))
        if (tile.Gid >= tileset.FirstGid)
        {
          currentTileset = tileset;
          return currentTileset;
        }
      return currentTileset;
    }

    public static Rectangle GetTileSourceRect(TmxMap map, TmxLayerTile tile)
    {
      TmxTileset tileset = GetTilesetForTile(map, tile);
      if (tileset == null)
        return Rectangle.Empty;
      int relativeGid = tile.Gid - tileset.FirstGid;
      int tileColumn = relativeGid % tileset.Columns.Value;
      int tileRow = (int)Math.Floor((double)relativeGid / tileset.Columns.Value);
      return new Rectangle(tileColumn * tileset.TileWidth, tileRow * tileset.TileHeight, tileset.TileWidth, tileset.TileHeight);
    }

    public static Vector2 AddZoom(this Vector2 vector)
    {
      return vector * (Game1.PixelZoom / 4);
    }

    public static Vector2 AddCamera(this Vector2 vector)
    {
      return vector - Game1.CameraPos;
    }

    public static Vector2 RemoveZoom(this Vector2 vector)
    {
      return vector / (Game1.PixelZoom / 4);
    }

    public static Vector2 RemoveCamera(this Vector2 vector)
    {
      return vector + Game1.CameraPos;
    }

    public static Vector2 DrawPos(this Vector2 vector)
    {
      Vector2 vec = vector.AddZoom().AddCamera();
      return new Vector2((int)vec.X, (int)vec.Y);
    }

    public static Vector2 WorldPos(this Vector2 vector)
    {
      return vector.RemoveCamera().RemoveZoom();
    }

    public static string GetResourceName(string path)
    {
      if (File.Exists(path))
        return path;
      else
        return path.Replace('/', '.');
    }

    public static Vector2 GetTilePos(float x, float y)
    {
      return new Vector2((float)Math.Floor(x / Game1.TileSize), (float)Math.Floor(y / Game1.TileSize));
    }

    public static int GetUnixTimestamp()
    {
      return (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }

    public static int ManhattanDist(Vector2 a, Vector2 b)
    {
      int distX = (int)(a.X - b.X);
      int distY = (int)(a.Y - b.Y);
      return Math.Abs(distX) + Math.Abs(distY);
    }

    public static void DrawOutlinedString(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, Color outlineColor, int outlineDepth)
    {
      spriteBatch.DrawString(spriteFont, text, Vector2.Add(new Vector2(-outlineDepth, -outlineDepth), position), outlineColor, rotation, origin, scale, effects, layerDepth + (0.0000001f));
      spriteBatch.DrawString(spriteFont, text, Vector2.Add(new Vector2(outlineDepth, outlineDepth), position), outlineColor, rotation, origin, scale, effects, layerDepth + (0.0000001f));
      spriteBatch.DrawString(spriteFont, text, Vector2.Add(new Vector2(-outlineDepth, outlineDepth), position), outlineColor, rotation, origin, scale, effects, layerDepth + (0.0000001f));
      spriteBatch.DrawString(spriteFont, text, Vector2.Add(new Vector2(outlineDepth, -outlineDepth), position), outlineColor, rotation, origin, scale, effects, layerDepth + (0.0000001f));
      spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
    }

    public static Dictionary<string, T> LoadListContent<T>(this ContentManager contentManager, string contentFolder)
    {
      DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
      if (!dir.Exists)
        throw new DirectoryNotFoundException();
      Dictionary<String, T> result = new Dictionary<String, T>();

      FileInfo[] files = dir.GetFiles("*.*");
      foreach (FileInfo file in files)
      {
        string key = Path.GetFileNameWithoutExtension(file.Name);


        result[key] = contentManager.Load<T>(contentFolder + "/" + key);
      }
      DirectoryInfo[] directories = dir.GetDirectories();
      foreach (DirectoryInfo directory in directories)
      {
        Dictionary<string, T> nextDict = contentManager.LoadListContent<T>(contentFolder + Path.DirectorySeparatorChar + directory.Name);
        foreach (var i in nextDict)
        {
          result[directory.Name + ":" + i.Key] = i.Value;
        }
      }
      return result;
    }
  }
}
