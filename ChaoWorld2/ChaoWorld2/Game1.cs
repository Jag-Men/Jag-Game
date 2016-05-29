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
using ChaoWorld2.UI.Menu;
using ChaoWorld2.Networking.Server;
using ChaoWorld2.Networking.Packets.Server;
using ChaoWorld2.Items;

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
    public static World World;
    public static Random Random;
    public static bool Paused;
    public static IMenu CurrentMenu;
    public static bool Server = false;
    public static bool Host = true;
    public static int PlayerId = -1;
    public static float Fade = 0;
    
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    RenderTarget2D gameRender;
    RenderTarget2D lighting;

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

      gameRender = new RenderTarget2D(GraphicsDevice, Game1.GameWidth, Game1.GameHeight);
      lighting = new RenderTarget2D(GraphicsDevice, Game1.GameWidth, Game1.GameHeight);

      ContentLibrary.Init();
      Item.Init();
      Crafting.Init();
      Game1.World = new World("area");

      if (Game1.Host)
      {
        Game1.Player = (Player)Game1.World.AddEntity(new Player(5, 5));
        Game1.PlayerId = Game1.Player.ID;
        Game1.World.AddEntity(new Stupidmadoka(8, 6));
        Game1.World.AddEntity(new Stupidmadoka(10, 8));
      }
    }

    protected override void UnloadContent()
    {

    }

    bool playedMusic = false;
    public static int timeman;
    public static int timescale(int integer)
    {
      integer += timeman + 1;
      return integer;
    }
    protected override void Update(GameTime gameTime)
    {
      Buffs.Update(gameTime);
      KeyboardUtil.Update();
      MouseUtil.Update();

      if(!Game1.IsPaused())
        Thyme.Update(gameTime);
      if (KeyboardUtil.KeyPressed(Keys.B))
        World.AddEntity(new Sodamachine(Utility.GetTilePos(MouseUtil.WorldPos.X, MouseUtil.WorldPos.Y)));
      if (KeyboardUtil.KeyPressed(Keys.K))
        timeman++;
      if (KeyboardUtil.KeyPressed(Keys.J))
        timeman--;

      if (MouseUtil.ButtonPressed(MouseButton.RightButton))
        Game1.World.AddEntity(new Treeeeeeee(Utility.GetTilePos(MouseUtil.WorldPos.X,MouseUtil.WorldPos.Y)));
      if (MouseUtil.ButtonPressed(MouseButton.MiddleButton))
        Game1.World.AddEntity(new Giantdog(Utility.GetTilePos(MouseUtil.WorldPos.X, MouseUtil.WorldPos.Y)));
      if (KeyboardUtil.KeyPressed(Keys.J))
        Game1.World.AddEntity(new Plant(Utility.GetTilePos(Player.X, Player.Y)));

      if (KeyboardUtil.KeyPressed(Keys.T))
        Thyme.active = !Thyme.active;

      if (!playedMusic)
      {
        Music.Play("music");
        Music.Volume(0.2f);
        playedMusic = true;
      }

      if(KeyboardUtil.KeyPressed(Keys.M))
      {
        if (KeyboardUtil.IsKeyDown(Keys.LeftControl))
        {
          Music.Dispose();
          Music.Play("monoman");
          Music.Volume(0.3f);
        }
        else
        {
          if (!Music.IsMuted)
            Music.Mute();
          else
            Music.UnMute();
        }
      }

      Game1.Random = new Random();
      
      if (Game1.CurrentMenu != null)
      {
        Game1.CurrentMenu.Update(gameTime);
        KeyboardUtil.Update();
      }

      if(Game1.World != null)
        Game1.World.Update(gameTime);
      if (KeyboardUtil.KeyPressed(Keys.X))
        World.AddEntity(new Chest(Utility.GetTilePos(MouseUtil.WorldPos.X, MouseUtil.WorldPos.Y)));

      if (KeyboardUtil.KeyPressed(Keys.E) && Game1.CurrentMenu == null)
        Game1.OpenMenu(new JagInventory());
      if (KeyboardUtil.KeyPressed(Keys.V))
        World.AddEntity(new Chest((Utility.GetTilePos(MouseUtil.WorldPos.X, MouseUtil.WorldPos.Y))));
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

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);

      if(Game1.World != null && Game1.World.Map != null && Game1.World.GroundRender == null)
      {
        Game1.World.GroundRender = new RenderTarget2D(GraphicsDevice, Game1.World.Map.Width * 16, Game1.World.Map.Height * 16);
        GraphicsDevice.SetRenderTarget(Game1.World.GroundRender);
        spriteBatch.Begin();
        Game1.World.DrawGround(spriteBatch);
        spriteBatch.End();
      }

      GraphicsDevice.SetRenderTarget(gameRender);
      GraphicsDevice.Clear(Color.Black);
      spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
      if (Game1.World != null && Game1.World.GroundRender != null)
      {
        spriteBatch.Draw(Game1.World.GroundRender, Vector2.Zero.DrawPos(), null, Color.White, 0, Vector2.Zero, Game1.PixelZoom, SpriteEffects.None, 1);
        Game1.World.Draw(spriteBatch);
      }
      Texture2D blank = new Texture2D(GraphicsDevice, 1, 1);
      blank.SetData(new Color[] { Color.White });
      spriteBatch.Draw(blank, new Rectangle(0, 0, Game1.GameWidth, Game1.GameHeight), Color.Black * Fade);
      Thyme.Draw(spriteBatch);
      spriteBatch.End();

      GraphicsDevice.SetRenderTarget(null);
      spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
      spriteBatch.Draw(gameRender, Vector2.Zero, null, Thyme.light, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
      if (Game1.CurrentMenu != null)
        Game1.CurrentMenu.Draw(spriteBatch);
      Buffs.Draw(spriteBatch);
      spriteBatch.Draw(ContentLibrary.Sprites["ui:cursorPict"], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, Layer.Mouse);
      spriteBatch.End();

      base.Draw(gameTime);
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
