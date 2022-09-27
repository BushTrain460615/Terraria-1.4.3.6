// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Creative.IPowerSubcategoryElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.GameContent.UI.Elements;

namespace Terraria.GameContent.Creative
{
  public interface IPowerSubcategoryElement
  {
    GroupOptionButton<int> GetOptionButton(
      CreativePowerUIElementRequestInfo info,
      int optionIndex,
      int currentOptionIndex);
  }
}
