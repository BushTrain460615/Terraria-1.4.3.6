// Decompiled with JetBrains decompiler
// Type: Terraria.ShoppingSettings
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria
{
  public struct ShoppingSettings
  {
    public double PriceAdjustment;
    public string HappinessReport;

    public static ShoppingSettings NotInShop => new ShoppingSettings()
    {
      PriceAdjustment = 1.0,
      HappinessReport = ""
    };
  }
}
