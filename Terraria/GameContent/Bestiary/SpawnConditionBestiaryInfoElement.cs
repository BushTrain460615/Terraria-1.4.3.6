// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.SpawnConditionBestiaryInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
  public class SpawnConditionBestiaryInfoElement : 
    FilterProviderInfoElement,
    IBestiaryBackgroundImagePathAndColorProvider,
    IBestiaryPrioritizedElement
  {
    private string _backgroundImagePath;
    private Color? _backgroundColor;

    public float OrderPriority { get; set; }

    public SpawnConditionBestiaryInfoElement(
      string nameLanguageKey,
      int filterIconFrame,
      string backgroundImagePath = null,
      Color? backgroundColor = null)
      : base(nameLanguageKey, filterIconFrame)
    {
      this._backgroundImagePath = backgroundImagePath;
      this._backgroundColor = backgroundColor;
    }

    public Asset<Texture2D> GetBackgroundImage() => this._backgroundImagePath == null ? (Asset<Texture2D>) null : Main.Assets.Request<Texture2D>(this._backgroundImagePath, (AssetRequestMode) 1);

    public Color? GetBackgroundColor() => this._backgroundColor;
  }
}
