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

namespace ChaoWorld2
{
  /// <summary>
  /// This is the main type for your game
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    public static int GameWidth = 1600;
    public static int GameHeight = 900;
    public static float TileSize = 64;
    public static float PixelZoom = 4;
    public static Vector2 CameraPos = new Vector2(0, 0);
    public static ContentManager GameContent;
    public static Player Player;
    public static GameMap Map;
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
      // TODO: Add your initialization logic here

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
      Game1.Map = ContentLibrary.Maps["map"];
      Game1.Player = new Player(33, 23);
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent()
    {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime)
    {
      // Allows the game to exit
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        this.Exit();

      KeyboardUtil.Update();
      Player.Update();

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

      CameraPos.X = playerPos.X - (Game1.GameWidth / 2);
      CameraPos.Y = playerPos.Y - (Game1.GameHeight / 2);

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
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
      foreach(var layer in Game1.Map.Layers)
      {
        foreach(var tile in layer.Tiles)
        {
          TmxTileset tileset = Utility.GetTilesetForTile(Game1.Map, tile);
          if (tileset == null)
            continue;
          spriteBatch.Draw(ContentLibrary.Tilesets[tileset.Name], new Vector2(tile.X * Game1.TileSize, tile.Y * Game1.TileSize).DrawPos(), Utility.GetTileSourceRect(Game1.Map, tile), Color.White, 0f, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 1f);
        }
      }
      Player.Draw(spriteBatch);
      spriteBatch.End();

      base.Draw(gameTime);
    }

    public static TmxLayerTile GetTileAt(float x, float y)
    {
      return GetTileAt((int)Math.Floor(x / Game1.TileSize), (int)Math.Floor(y / Game1.TileSize));
    }

    public static TmxLayerTile GetTileAt(int x, int y)
    {
      if (Game1.Map == null || !Game1.Map.Layers.Contains("Ground"))
        return null;
      foreach (var tile in Game1.Map.Layers["Ground"].Tiles)
        if (tile.X == x && tile.Y == y)
          return tile;
      return null;
    }
  }
}
