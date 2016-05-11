using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaoWorld2.Items
{
  public class Crafting
  {
    public static List<CraftingRecipe> Recipes = new List<CraftingRecipe>();

    public static void Init()
    {
      Recipes.Add(new CraftingRecipe(Item.LogPile, Item.Log, Item.Log, Item.Log));
    }

    public static Item Craft(List<Item> ingredients)
    {
      foreach (var i in Crafting.Recipes)
      {
        List<Item> input = new List<Item>(ingredients);
        List<Item> check = new List<Item>(i.Ingredients);
        foreach (var item in ingredients)
          if (check.Contains(item))
          {
            check.Remove(item);
            input.Remove(item);
          }
        if (input.Count == 0 && check.Count == 0)
          return i.Result;
      }
      return null;
    }
  }

  public class CraftingRecipe
  {
    public List<Item> Ingredients = new List<Item>();
    public Item Result;

    public CraftingRecipe(Item result, params Item[] ingredients)
    {
      this.Result = result;
      this.Ingredients = ingredients.ToList();
    }
  }
}
