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

namespace ChaoWorld2
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Game
  {
    public static int GameWidth = 1600;
    public static int GameHeight = 900;
    public static float TileSize = 64;
    public static float PixelZoom = 4;
    public static Vector2 CameraPos = new Vector2(0, 0);
    public static ContentManager GameContent;
    public static Player Player;
    public static GameMap Map;
    public static Random Random;
    public static bool Paused;
    public static IMenu CurrentMenu;

    public static ConcurrentDictionary<int, Entity> Entities = new ConcurrentDictionary<int, Entity>();
    public static int NextEntityID = 0;

    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
      graphics.PreferredBackBufferWidth = Game1.GameWidth;
      graphics.PreferredBackBufferHeight = Game1.GameHeight;
      Content.RootDirectory = "Content";
      Game1.GameContent = Content;
    }


    protected override void Initialize()
    {
      Game1.Random = new Random();
      Game1.Paused = false;
      Game1.CurrentMenu = null;
      base.Initialize();
    }


    protected override void LoadContent()
    {
      spriteBatch = new SpriteBatch(GraphicsDevice);
      
      ContentLibrary.Init();
      Game1.Map = ContentLibrary.Maps["area"];

      Game1.AddEntity(new Player(5, 5));
      Game1.AddEntity(new Stupidmadoka(8, 6));
      Game1.AddEntity(new Stupidmadoka(10, 8));
    }

    protected override void UnloadContent()
    {

    }

    public static Entity AddEntity(Entity entity)
    {
      if (Game1.Entities.TryAdd(NextEntityID, entity))
      {
        entity.ID = NextEntityID;
        if (entity is Player)
          Game1.Player = entity as Player;
        NextEntityID++;
      }
      return entity;
    }

    public static Entity RemoveEntity(Entity entity)
    {
      Entity dummy = entity;
      if (Game1.Entities.TryRemove(entity.ID, out dummy))
      {
        if (entity is Player)
          Game1.Player = null;
      }
      return dummy;
    }

    protected override void Update(GameTime gameTime)
    {
      Game1.Random = new Random();

      KeyboardUtil.Update();
      if (Game1.CurrentMenu != null)
      {
        Game1.CurrentMenu.Update(gameTime);
        KeyboardUtil.Update();
      }
      foreach (var entity in Entities)
      {
        if(!Game1.Paused)
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
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - (tile.Y * Game1.TileSize + (Game1.TileSize / 2)) / 100000f);
      }
      foreach (var tile in GetTilesInLayer("Above"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Game1.Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.5f - ((tile.Y + 1) * Game1.TileSize + (Game1.TileSize / 2)) / 100000f);
      }
      foreach (var tile in GetTilesInLayer("Front"))
      {
        TmxTileset tileset = Utility.GetTilesetForTile(Game1.Map, tile);
        if (tileset == null)
          continue;
        spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 0.1f - (tile.Y * Game1.TileSize + (Game1.TileSize / 2)) / 100000f);
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
  }
}
