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

namespace ChaoWorld2
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    public static SpriteFont fontman;
    public static int GameWidth = 1600;
    public static int GameHeight = 900;
    public static float TileSize = 64;
    public static float PixelZoom = 4;
    public static Vector2 CameraPos = new Vector2(0, 0);
    public static ContentManager GameContent;
    public static Player Player;
    public static GameMap Map;
    public static Stupidmadoka Stupidman;
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

    /// <summary>
    /// Allows the game to perform any initialization it needs to before starting to run.
    /// This is where it can query for any required services and load any non-graphic
    /// related content.  Calling base.Initialize will enumerate through any components
    /// and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {


      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent()
    {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // TODO: use this.Content to load your game content here
      ContentLibrary.Init();
      Game1.Map = ContentLibrary.Maps["area"];
      Game1.Player = new Player(5, 5);
      Game1.Stupidman = new Stupidmadoka(8, 6);
      fontman = Content.Load<SpriteFont>("SpriteFont1");
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content her
      int joaje;
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      System.Console.WriteLine("joaje");

      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      KeyboardUtil.Update();
      Player.Update();
      Stupidman.Update();
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

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      // TODO: Add your drawing code here
      spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
      foreach(var tile in GetTilesInLayer("Ground"))
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
      spriteBatch.Draw(ContentLibrary.Sprites["cursorPict"], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Azure);
      Player.Draw(spriteBatch);
      Stupidman.Draw(spriteBatch);
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

  }
}
