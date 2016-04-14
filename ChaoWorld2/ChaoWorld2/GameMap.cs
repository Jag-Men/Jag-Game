using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace ChaoWorld2
{
  public class GameMap : TmxMap
  {
    public Vector2 PlayerSpawn;

    public GameMap(string filename)
      :base(filename)
    {
      PlayerSpawn = Vector2.Zero;
      if (Layers.Contains("Special"))
        foreach (var tile in Layers["Special"].Tiles)
        {
          TmxTileset tileset = Utility.GetTilesetForTile(this, tile);
          if (tileset != null && tileset.Name == "special" && tile.Gid - tileset.FirstGid == 0)
          {
            Console.WriteLine("ya");
            PlayerSpawn = new Vector2(tile.X, tile.Y);
          }
        }
    }
  }
}
