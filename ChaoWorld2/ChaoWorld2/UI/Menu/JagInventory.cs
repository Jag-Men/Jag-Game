using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoWorld2.Util;
using ChaoWorld2.Items;

namespace ChaoWorld2.UI.Menu
{
  public class JagInventory : IMenu
  {
    int heldSlot = -1;
    bool overTrash = false;
    Rectangle? slotSelection = null;
    int currentSelected = -1;
    int originalSelected = -1;

    public JagInventory()
    {

    }

    public void Update(GameTime gameTime)
    {
      if(KeyboardUtil.KeyPressed(Keys.E))
      {
        Game1.CloseMenu();
        return;
      }
      Vector2 invPos = new Vector2(Game1.GameWidth / 2 - 128 * 2, Game1.GameHeight / 2 - 128 * 2);
      Vector2 mouseRelative = MouseUtil.XandY - invPos;

      if (MouseUtil.X >= invPos.X && MouseUtil.X < invPos.X + GetSize().X &&
        MouseUtil.Y >= invPos.Y && MouseUtil.Y < invPos.Y + GetSize().Y)
      {
        int slot = (int)((Math.Floor(mouseRelative.Y / 128) * 4) + Math.Floor(mouseRelative.X / 128));
        if (KeyboardUtil.IsKeyDown(Keys.LeftShift) && this.heldSlot == -1)
        {
          if (originalSelected == -1)
            originalSelected = slot;
          currentSelected = slot;
          Rectangle originRect = new Rectangle((originalSelected % 4) * 128, (originalSelected / 4) * 128, 128, 128);
          Rectangle currentRect = new Rectangle((currentSelected % 4) * 128, (currentSelected / 4) * 128, 128, 128);
          slotSelection = Rectangle.Union(originRect, currentRect);
          if(MouseUtil.ButtonPressed(MouseButton.LeftButton))
          {
            List<int> slots = new List<int>();
            for (int i = 0; i < Game1.Player.Inventory.Length; i++)
            {
              int slotX = (i % 4) * 128;
              int slotY = (i / 4) * 128;
              if (slotSelection.Value.Contains(slotX, slotY))
                if (Game1.Player.Inventory[i] != null)
                  slots.Add(i);
            }
            List<Item> craftItems = new List<Item>();
            slots.ForEach(_ => craftItems.Add(Game1.Player.Inventory[_]));
            Item craft = Crafting.Craft(craftItems);
            if(craft != null)
            {
              foreach (var i in slots)
                Game1.Player.Inventory[i] = null;
              Game1.Player.Inventory[slot] = craft;
            }
          }
        }
        else if (MouseUtil.ButtonPressed(MouseButton.LeftButton))
        {
          if (Game1.Player.Inventory[slot] != null)
            this.heldSlot = slot;
        }
        else if (MouseUtil.IsButtonUp(MouseButton.LeftButton) && this.heldSlot != -1)
        {
          Item replacedWith = Game1.Player.Inventory[slot];
          Game1.Player.Inventory[slot] = Game1.Player.Inventory[this.heldSlot];
          Game1.Player.Inventory[this.heldSlot] = replacedWith;
          this.heldSlot = -1;
        }
      }
      else
      {
        if (MouseUtil.IsButtonUp(MouseButton.LeftButton) && this.heldSlot != -1)
        {
          if (this.overTrash)
          {
            Game1.PlaySound("trash", 0.2f);
            Game1.Player.Inventory[this.heldSlot] = null;
            this.heldSlot = -1;
          }
          else
            this.heldSlot = -1;
        }
      }
      if (this.heldSlot != -1 && Game1.Player.Inventory[this.heldSlot] == null)
        this.heldSlot = -1;

      if (this.heldSlot != -1)
      {
        Texture2D trash = ContentLibrary.Sprites["ui:trash1"];
        if (MouseUtil.X >= invPos.X + GetSize().X && MouseUtil.X < invPos.X + GetSize().X + trash.Width &&
          MouseUtil.Y >= invPos.Y + GetSize().Y - trash.Height && MouseUtil.Y < invPos.Y + GetSize().Y)
        {
          overTrash = true;
        }
        else
          overTrash = false;
      }
      else
        overTrash = false;

      if (KeyboardUtil.IsKeyUp(Keys.LeftShift))
      {
        slotSelection = null;
        currentSelected = -1;
        originalSelected = -1;
      }
    }

    public Vector2 GetSize()
    {
      Texture2D invSprite = ContentLibrary.Sprites["ui:Untitled"];
      return new Vector2(invSprite.Width * 4, invSprite.Height * 4);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Vector2 invPos = new Vector2(Game1.GameWidth / 2 - 128 * 2, Game1.GameHeight / 2 - 128 * 2);
      spriteBatch.Draw(ContentLibrary.Sprites["ui:Untitled"],new Vector2(Game1.GameWidth/2-128*2, Game1.GameHeight/2 -128*2),new Rectangle(0,0,128,128),Color.White,0f,Vector2.Zero,4,SpriteEffects.None,Layer.Menu);
      for(int i = 0; i < Game1.Player.Inventory.Length; i++)
      {
        if(i != heldSlot && Game1.Player.Inventory[i] != null)
        {
          var item = Game1.Player.Inventory[i];
          Texture2D itemSprite = ContentLibrary.Sprites["item:" + item.Texture];
          Vector2 spriteSize = item.TexSource.HasValue ? new Vector2(item.TexSource.Value.Width, item.TexSource.Value.Height) : new Vector2(itemSprite.Width, itemSprite.Height);
          spriteSize *= item.Scale * 2;
          Vector2 itemPos = new Vector2((i % 4) * 128, (i / 4) * 128);
          itemPos = itemPos + new Vector2(64, 64) - new Vector2(spriteSize.X / 2, spriteSize.Y / 2);
          spriteBatch.Draw(itemSprite, invPos + itemPos, item.TexSource, Color.White, 0, Vector2.Zero, item.Scale * 2, SpriteEffects.None, Layer.Menu - 0.0001f);
        }
      }
      if(slotSelection.HasValue)
      {
        Rectangle ba = new Rectangle(slotSelection.Value.X, slotSelection.Value.Y, slotSelection.Value.Width, slotSelection.Value.Height);
        ba.Offset((int)invPos.X, (int)invPos.Y);
        spriteBatch.Draw(ContentLibrary.Sprites["ui:slotselect"], ba, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, Layer.Menu - 0.0002f);
      }
      if(this.heldSlot != -1)
      {
        var item = Game1.Player.Inventory[this.heldSlot];
        Texture2D itemSprite = ContentLibrary.Sprites["item:" + item.Texture];
        Vector2 spriteSize = item.TexSource.HasValue ? new Vector2(item.TexSource.Value.Width, item.TexSource.Value.Height) : new Vector2(itemSprite.Width, itemSprite.Height);
        spriteSize *= item.Scale * 2;
        Vector2 itemPos = MouseUtil.XandY - new Vector2(spriteSize.X / 2, spriteSize.Y / 2);
        spriteBatch.Draw(itemSprite, itemPos, item.TexSource, Color.White, 0, Vector2.Zero, item.Scale * 2, SpriteEffects.None, Layer.Menu - 0.0003f);
      }
      Texture2D trash = ContentLibrary.Sprites["ui:" + (overTrash ? "trash2" : "trash1")];
      spriteBatch.Draw(trash, invPos + GetSize() - new Vector2(0, trash.Height), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, Layer.Menu);
    }
  }
}
