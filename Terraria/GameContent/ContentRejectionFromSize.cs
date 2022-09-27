// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ContentRejectionFromSize
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent
{
  public class ContentRejectionFromSize : IRejectionReason
  {
    private int _neededWidth;
    private int _neededHeight;
    private int _actualWidth;
    private int _actualHeight;

    public ContentRejectionFromSize(
      int neededWidth,
      int neededHeight,
      int actualWidth,
      int actualHeight)
    {
      this._neededWidth = neededWidth;
      this._neededHeight = neededHeight;
      this._actualWidth = actualWidth;
      this._actualHeight = actualHeight;
    }

    public string GetReason() => Language.GetTextValueWith("AssetRejections.BadSize", (object) new
    {
      NeededWidth = this._neededWidth,
      NeededHeight = this._neededHeight,
      ActualWidth = this._actualWidth,
      ActualHeight = this._actualHeight
    });
  }
}
