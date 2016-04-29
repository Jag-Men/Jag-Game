using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TiledSharp;
using System.IO;
using ChaoWorld2.Util;
using ChaoWorld2.Entities;
using System.Collections.Concurrent;
using ChaoWorld2.Menu;
using ChaoWorld2.Networking.Server;
using ChaoWorld2.Networking.Packets.Server;

namespace ChaoWorld2
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Game
  {
    public static Game1 Instance;

    public static int GameWidth = 1600;
    public static int GameHeight = 900;
    public static int TileSize = 64;
    public static float PixelZoom = 4;
    public static Vector2 CameraPos = new Vector2(0, 0);
    public static ContentManager GameContent;
    public static Player Player;
    public static GameMap Map;
    public static Random Random;
    public static bool Paused;
    public static IMenu CurrentMenu;
    public static bool Server = false;
    public static bool Host = true;
    public static int PlayerId = -1;

    public static ConcurrentDictionary<int, Entity> Entities = new ConcurrentDictionary<int, Entity>();
    public static int NextEntityID = 0;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public Game1()
    {
      Game1.Instance = this;

      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = Game1.GameWidth;
      graphics.PreferredBackBufferHeight = Game1.GameHeight;
      Content.RootDirectory = "Content";
      Game1.GameContent = Content;
    }

    public Game1(string ip, int port)
      :this()
    {
      Host = false;
      Program.Client.DestIp = ip + ":" + port;
    }


    protected override void Initialize()
    {
      Game1.Random = new Random();
      Game1.Paused = false;
      Game1.CurrentMenu = null;

      base.Initialize();
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
      if (Program.Client != null)
        Program.Client.Terminating = true;
      if (Program.ClientThread != null)
        Program.ClientThread.Abort();
      ClientManager.Terminated = true;
      if (Program.NetworkThread != null)
        Program.NetworkThread.Abort();
      base.OnExiting(sender, args);
    }

    public static bool IsPaused()
    {
      return !Game1.Instance.IsActive || Game1.Paused;
    }

    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      
      ContentLibrary.Init();
      Game1.Map = ContentLibrary.Maps["area"];

      if (Game1.Host)
      {
        Game1.Player = (Player)Game1.AddEntity(new Player(5, 5));
        Game1.PlayerId = Game1.Player.ID;
        Game1.AddEntity(new Stupidmadoka(8, 6));
        Game1.AddEntity(new Stupidmadoka(10, 8));
      }
    }

    protected override void UnloadContent()
    {

    }

    public static Entity AddEntity(Entity entity)
    {
      int nextId = entity.ID;
      if (nextId == -1)
        nextId = NextEntityID++;
      if (Game1.Entities.TryAdd(nextId, entity))
      {
        AddedEntities.Add(entity);
        entity.ID = nextId;
        if (entity is Player && entity.ID == PlayerId)
          Game1.Player = entity as Player;
      }
      else
      {
        Console.WriteLine(false);
      }
      return entity;
    }

    public static Entity RemoveEntity(Entity entity)
    {
      return RemoveEntity(entity.ID);
    }

    public static Entity RemoveEntity(int id)
    {
      Entity entity;
      if (Game1.Entities.TryRemove(id, out entity))
      {
        RemovedEntities.Add(id);
        if (Game1.Player == entity)
          Game1.Player = null;
      }
      return entity;
    }

    bool playedMusic = false;
    private static List<Entity> AddedEntities = new List<Entity>();
    private static List<int> RemovedEntities = new List<int>();
    protected override void Update(GameTime gameTime)
    {
      KeyboardUtil.Update();
      MouseUtil.Update();

      if (!playedMusic)
      {
        Music.Play("music");
        Music.Volume(0.2f);
        playedMusic = true;
      }

      if(KeyboardUtil.KeyPressed(Keys.M))
      {
        if (!Music.IsMuted)
          Music.Mute();
        else
          Music.UnMute();
      }

      AddedEntities.Clear();
      RemovedEntities.Clear();
      if (Game1.Player == null)
        return;

      Game1.Random = new Random();
      
      if (Game1.CurrentMenu != null)
      {
        Game1.CurrentMenu.Update(gameTime);
        KeyboardUtil.Update();
      }
      foreach (var entity in Entities)
      {
        if(!Game1.IsPaused())
          entity.Value.Update(gameTime);
        entity.Value.UpdateEvenWhenPaused(gameTime);
      }

      if (KeyboardUtil.KeyPressed(Keys.E) && Game1.CurrentMenu == null)
        Game1.OpenMenu(new JagInventory());

      if (KeyboardUtil.KeyPressed(Keys.OemPlus))
      {
        if (Game1.PixelZoom < 1)
          Game1.PixelZoom *= 2;
        else
          Game1.PixelZoom++;
      }
      if (KeyboardUtil.KeyPressed(Keys.OemMinus))
      {
        if (Game1.PixelZoom <= 1)
          Game1.PixelZoom /= 2;
        else
          Game1.PixelZoom--;
      }

      Vector2 playerPos = new Vector2(Player.X, Player.Y).AddZoom();

      float mapWidth = Game1.Map.Width * Game1.TileSize * (Game1.PixelZoom / 4);
      float mapHeight = Game1.Map.Height * Game1.TileSize * (Game1.PixelZoom / 4);
      if (mapWidth < Game1.GameWidth)
        CameraPos.X = -((Game1.GameWidth - mapWidth) / 2);
      else
        CameraPos.X = Math.Max(0, Math.Min(mapWidth - Game1.GameWidth, playerPos.X - (Game1.GameWidth / 2)));
      if (mapHeight < Game1.GameHeight)
        CameraPos.Y = -((Game1.GameHeight - mapHeight) / 2);
      else
        CameraPos.Y = Math.Max(0, Math.Min(mapHeight - Game1.GameHeight, playerPos.Y - (Game1.GameHeight / 2)));

      if(Game1.Server && Game1.Host)
      {
        foreach(var i in ClientManager.Clients.Values)
        {
          i.SendPacket(new AddRemoveEntitiesPacket
          {
            AddedEntities = AddedEntities,
            RemovedEntities = RemovedEntities
          });

          List<Entity> SendUpdate = new List<Entity>();
          foreach(var e in Game1.Entities.Values)
            if (i.PlayerId != e.ID && !AddedEntities.Contains(e))
              SendUpdate.Add(e);
          i.SendPacket(new UpdateEntitiesPacket
          {
            WriteEntities = SendUpdate
          });
        }
      }

      base.Update(gameTime);
    }
    string stringman;
    
    float vecx;
    float vecy;
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);
      
      spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
      if (KeyboardUtil.KeyPressed(Keys.G))
      {
        stringman ="doggo";
        vecx = 100;
        vecy = 100;
        if (stringman.Length > 10 * vecx)
        {

        }
        spriteBatch.DrawString(ContentLibrary.Fonts["fonnman"], stringman, new Vector2(vecx, vecy), Color.White);
      }
      foreach (var tile in GetTilesInLayer("Ground"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Game1.Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 1f);
      }
      foreach (var tile in GetTilesInLayer("Solid"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Game1.Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - (tile.Y * Game1.TileSize + Game1.TileSize) / 100000f);
      }
      foreach (var tile in GetTilesInLayer("Above"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Game1.Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - ((tile.Y + 1) * Game1.TileSize + Game1.TileSize) / 100000f);
      }
      foreach (var tile in GetTilesInLayer("Front"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Game1.Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.1f - (tile.Y * Game1.TileSize + Game1.TileSize) / 100000f);
      }
      foreach (var entity in Entities)
        entity.Value.Draw(spriteBatch);
      if (Game1.CurrentMenu != null)
        Game1.CurrentMenu.Draw(spriteBatch);
      spriteBatch.Draw(ContentLibrary.Sprites["cursorPict"], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Azure);
      spriteBatch.End();

      base.Draw(gameTime);
    }

    public static List<TmxLayerTile> GetTilesInLayer(string layer)
    {
      if (!Game1.Map.Layers.Contains(layer))
        return new List<TmxLayerTile>();
      return Game1.Map.Layers[layer].Tiles.ToList();
    }

    public static TmxLayerTile GetTileAt(Vector2 pos, string layer)
    {
      return GetTileAt((int)pos.X, (int)pos.Y, layer);
    }
    
    public static TmxLayerTile GetTileAt(int x, int y, string layer)
    {
      if (Game1.Map == null || !Game1.Map.Layers.Contains(layer))
        return null;
      foreach (var tile in Game1.Map.Layers[layer].Tiles)
        if (tile.X == x && tile.Y == y)
          return tile;
      return null;
    }

    public static IEnumerable<Entity> GetEntitiesInside(Rectangle rect)
    {
      foreach (var entity in Game1.Entities.Values)
        if (entity.GetCollisionBox().Intersects(rect))
          yield return entity;
    }

    public static bool IsTilePassable(Vector2 pos)
    {
      return IsTilePassable((int)pos.X, (int)pos.Y);
    }

    public static bool IsTilePassable(int x, int y)
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
    
    public static IMenu OpenMenu(IMenu menu)
    {
      Game1.CurrentMenu = menu;
      if(!Game1.Server)
        Game1.Paused = true;
      return menu;
    }

    public static IMenu CloseMenu()
    {
      IMenu menu = Game1.CurrentMenu;
      Game1.CurrentMenu = null;
      Game1.Paused = false;
      return menu;
    }

    public static void PlaySound(string name, float volume = 0.5f)
    {
      ContentLibrary.Sounds[name].Play(volume, 0, 0);
    }
  }
}
