using ChaoWorld2.Entities;
using ChaoWorld2.Networking.Packets.Server;
using ChaoWorld2.Networking.Server;
using ChaoWorld2.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace ChaoWorld2
{
  public class World
  {
    public static ConcurrentDictionary<string, World> Worlds = new ConcurrentDictionary<string, World>();

    public static World GetInstance(string name)
    {
      World world = null;
      if (Worlds.ContainsKey(name))
        Worlds.TryGetValue(name, out world);
      if (world == null)
        world = new World(name);
      return world;
    }

    public GameMap Map;
    public List<WorldWarpPoint> WarpPoints = new List<WorldWarpPoint>();

    public ConcurrentDictionary<int, Entity> Entities = new ConcurrentDictionary<int, Entity>();
    public int NextEntityID = 0;

    public World(string name)
    {
      this.Map = ContentLibrary.Maps[name];
      if (!Worlds.ContainsKey(name))
        Worlds.TryAdd(name, this);
      if(Map.Properties.ContainsKey("warp"))
      {
        int index = 0;
        string[] args = Map.Properties["warp"].Split(' ');
        while (index < args.Length)
        {
          Console.WriteLine(args.Length);
          int fromPosX = Convert.ToInt32(args[index++]);
          int fromPosY = Convert.ToInt32(args[index++]);
          string destination = args[index++];
          int toPosX = Convert.ToInt32(args[index++]);
          int toPosY = Convert.ToInt32(args[index++]);
          WarpPoints.Add(new WorldWarpPoint(destination, new Vector2(fromPosX, fromPosY), new Vector2(toPosX, toPosY)));
        }
      }
    }

    private List<Entity> AddedEntities = new List<Entity>();
    private List<int> RemovedEntities = new List<int>();
    public void Update(GameTime gameTime)
    {
      AddedEntities.Clear();
      RemovedEntities.Clear();
      if (Game1.Player == null)
        return;

      foreach (var entity in Entities)
      {
        if (!Game1.IsPaused())
          entity.Value.Update(gameTime);
        entity.Value.UpdateEvenWhenPaused(gameTime);
      }

      Vector2 playerPos = new Vector2(Game1.Player.X, Game1.Player.Y).AddZoom();

      float mapWidth = Map.Width * Game1.TileSize * (Game1.PixelZoom / 4);
      float mapHeight = Map.Height * Game1.TileSize * (Game1.PixelZoom / 4);
      if (mapWidth < Game1.GameWidth)
        Game1.CameraPos.X = -((Game1.GameWidth - mapWidth) / 2);
      else
        Game1.CameraPos.X = Math.Max(0, Math.Min(mapWidth - Game1.GameWidth, playerPos.X - (Game1.GameWidth / 2)));
      if (mapHeight < Game1.GameHeight)
        Game1.CameraPos.Y = -((Game1.GameHeight - mapHeight) / 2);
      else
        Game1.CameraPos.Y = Math.Max(0, Math.Min(mapHeight - Game1.GameHeight, playerPos.Y - (Game1.GameHeight / 2)));

      if (Game1.Server && Game1.Host)
      {
        foreach (var i in ClientManager.Clients.Values)
        {
          i.SendPacket(new AddRemoveEntitiesPacket
          {
            AddedEntities = AddedEntities,
            RemovedEntities = RemovedEntities
          });

          List<Entity> SendUpdate = new List<Entity>();
          foreach (var e in Entities.Values)
            if (i.PlayerId != e.ID && !AddedEntities.Contains(e))
              SendUpdate.Add(e);
          i.SendPacket(new UpdateEntitiesPacket
          {
            WriteEntities = SendUpdate
          });
        }
      }

      if (Game1.Host)
      {
        foreach(var i in WarpPoints)
        {
          Vector2 tilePos = Utility.GetTilePos(Game1.Player.X, Game1.Player.Y);
          if(tilePos.X == i.FromPos.X && tilePos.Y == i.FromPos.Y)
          {
            Game1.Paused = true;
            if (Game1.Fade < 1f)
              Game1.Fade += 0.05f;
            else
            {
              RemoveEntity(Game1.Player);
              World newWorld = World.GetInstance(i.Destination);
              newWorld.AddEntity(Game1.Player);
              Game1.Player.X = i.ToPos.X * Game1.TileSize + (Game1.TileSize / 2);
              Game1.Player.Y = i.ToPos.Y * Game1.TileSize + (Game1.TileSize / 2);
              Game1.World = newWorld;
            }
            return;
          }
        }
      }
      if (Game1.Fade > 0)
      {
        Game1.Fade -= 0.05f;
        if (Game1.Fade < 0)
          Game1.Fade = 0;
        if (Game1.Fade == 0)
          Game1.Paused = false;
      }
    }

    public Entity AddEntity(Entity entity)
    {
      int nextId = entity.ID;
      if (nextId == -1)
        nextId = NextEntityID++;
      if (Entities.TryAdd(nextId, entity))
      {
        AddedEntities.Add(entity);
        entity.Owner = this;
        entity.ID = nextId;
        if (entity is Player && entity.ID == Game1.PlayerId)
          Game1.Player = entity as Player;
      }
      else
      {
        Console.WriteLine(false);
      }
      return entity;
    }

    public Entity RemoveEntity(Entity entity)
    {
      return RemoveEntity(entity.ID);
    }

    public Entity RemoveEntity(int id)
    {
      Entity entity;
      if (Entities.TryRemove(id, out entity))
      {
        RemovedEntities.Add(id);
        entity.Owner = null;
        //if (Game1.Player == entity)
        //  Game1.Player = null;
      }
      return entity;
    }

    public List<TmxLayerTile> GetTilesInLayer(string layer)
    {
      if (!Map.Layers.Contains(layer))
        return new List<TmxLayerTile>();
      return Map.Layers[layer].Tiles.ToList();
    }

    public TmxLayerTile GetTileAt(Vector2 pos, string layer)
    {
      return GetTileAt((int)pos.X, (int)pos.Y, layer);
    }

    public TmxLayerTile GetTileAt(int x, int y, string layer)
    {
      if (Map == null || !Map.Layers.Contains(layer))
        return null;
      foreach (var tile in Map.Layers[layer].Tiles)
        if (tile.X == x && tile.Y == y)
          return tile;
      return null;
    }

    public IEnumerable<Entity> GetEntitiesInside(Rectangle rect, params string[] collision)
    {
      if (collision.Length == 0)
      {
        foreach (var entity in Entities.Values)
          if (entity.GetCollisionBox().Intersects(rect))
            yield return entity;
      }
      else
      {
        foreach (var entity in Entities.Values)
          if (entity.GetCollisionBox().Intersects(rect))
            foreach (var i in collision)
              if (entity.Collision.Contains(i))
              {
                yield return entity;
                break;
              }
      }
    }

    public IEnumerable<TmxLayerTile> GetTilesInside(Rectangle rect, string layer)
    {
      foreach (var tile in Map.Layers[layer].Tiles)
        if (new Rectangle(tile.X * Game1.TileSize, tile.Y * Game1.TileSize, Game1.TileSize, Game1.TileSize).Intersects(rect))
          yield return tile;
    }

    public bool RectCollidesWith(Rectangle rect, params string[] collision)
    {
      if (GetEntitiesInside(rect, collision).Count() > 0)
        return true;
      foreach (var i in collision)
        if (Map.Layers.Contains(i))
          foreach (var tile in GetTilesInside(rect, i))
            if (i == "Ground")
            {
              if (tile.Gid == 0)
                return true;
            }
            else
            {
              if (tile.Gid != 0)
                return true;
            }
      return false;
    }

    public bool IsTilePassable(Vector2 pos)
    {
      return IsTilePassable((int)pos.X, (int)pos.Y);
    }

    public bool IsTilePassable(int x, int y)
    {
      TmxLayerTile tile;
      tile = GetTileAt(x, y, "Ground");
      if (tile == null || tile.Gid == 0)
        return false;
      tile = GetTileAt(x, y, "Solid");
      if (tile != null && tile.Gid != 0)
        return false;
      return true;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var tile in GetTilesInLayer("Ground"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Ground);
      }
      foreach (var tile in GetTilesInLayer("Solid"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - (tile.Y * Game1.TileSize) / 1e5f);
      }
      foreach (var tile in GetTilesInLayer("Above"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.Object - ((tile.Y + 1) * Game1.TileSize + Game1.TileSize) / 1e5f);
      }
      foreach (var tile in GetTilesInLayer("Front"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, Layer.AboveObject - (tile.Y * Game1.TileSize + Game1.TileSize) / 1e5f);
      }
      foreach (var entity in Entities)
        entity.Value.Draw(spriteBatch);
    }
  }

  public class WorldWarpPoint
  {
    public Vector2 FromPos;
    public Vector2 ToPos;
    public string Destination;

    public WorldWarpPoint(string destination, Vector2 from, Vector2 to)
    {
      this.Destination = destination;
      this.FromPos = from;
      this.ToPos = to;
    }
  }
}
