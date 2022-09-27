// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.IGroupOptionButton
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI.Elements
{
  public interface IGroupOptionButton
  {
    void SetColorsBasedOnSelectionState(
      Color pickedColor,
      Color unpickedColor,
      float opacityPicked,
      float opacityNotPicked);

    void SetBorderColor(Color color);
  }
}
