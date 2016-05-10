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
    public List<Vector2> SolidTiles = new List<Vector2>();

    public Dictionary<string, List<TmxLayerTile>> OptLayers = new Dictionary<string, List<TmxLayerTile>>();

    public GameMap(string filename)
      :base(filename)
    {
      foreach(var layer in Layers)
      {
        List<TmxLayerTile> tiles = new List<TmxLayerTile>();
        foreach (var tile in layer.Tiles)
          if (tile.Gid != 0)
            tiles.Add(tile);
        OptLayers.Add(layer.Name, tiles);
      }
      if (Layers.Contains("Ground"))
        foreach (var tile in Layers["Ground"].Tiles)
        {
          if (tile.Gid == 0)
            SolidTiles.Add(new Vector2(tile.X, tile.Y));
        }
      if (Layers.Contains("Solid"))
        foreach (var tile in OptLayers["Solid"])
            SolidTiles.Add(new Vector2(tile.X, tile.Y));
      if (Layers.Contains("Special"))
        foreach (var tile in OptLayers["Special"])
        {
          TmxTileset tileset = Utility.GetTilesetForTile(this, tile);
          if (tileset != null && tileset.Name == "special" && tile.Gid - tileset.FirstGid == 0)
            Trees.Add(new Vector2(tile.X, tile.Y));
        }
    }
  }
}
