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
    public List<Vector2> Trees = new List<Vector2>();

    public GameMap(string filename)
      :base(filename)
    {
      if (Layers.Contains("Special"))
        foreach (var tile in Layers["Special"].Tiles)
        {
          TmxTileset tileset = Utility.GetTilesetForTile(this, tile);
          if (tileset != null && tileset.Name == "special" && tile.Gid - tileset.FirstGid == 0)
            Trees.Add(new Vector2(tile.X, tile.Y));
        }
    }
  }
}
