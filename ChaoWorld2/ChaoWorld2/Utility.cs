using Microsoft.Xna.Framework;
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
          currentTileset = tileset;
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

    public static Vector2 DrawPos(this Vector2 vector)
    {
      return vector.AddZoom().AddCamera();
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
  }
}
