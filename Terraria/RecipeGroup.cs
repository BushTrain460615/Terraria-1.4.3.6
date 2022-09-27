// Decompiled with JetBrains decompiler
// Type: Terraria.RecipeGroup
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria
{
  public class RecipeGroup
  {
    public Func<string> GetText;
    public HashSet<int> ValidItems;
    public int IconicItemId;
    public static Dictionary<int, RecipeGroup> recipeGroups = new Dictionary<int, RecipeGroup>();
    public static Dictionary<string, int> recipeGroupIDs = new Dictionary<string, int>();
    public static int nextRecipeGroupIndex;

    public RecipeGroup(Func<string> getName, params int[] validItems)
    {
      this.GetText = getName;
      this.ValidItems = new HashSet<int>((IEnumerable<int>) validItems);
      this.IconicItemId = validItems[0];
    }

    public static int RegisterGroup(string name, RecipeGroup rec)
    {
      int key = RecipeGroup.nextRecipeGroupIndex++;
      RecipeGroup.recipeGroups.Add(key, rec);
      RecipeGroup.recipeGroupIDs.Add(name, key);
      return key;
    }
  }
}
