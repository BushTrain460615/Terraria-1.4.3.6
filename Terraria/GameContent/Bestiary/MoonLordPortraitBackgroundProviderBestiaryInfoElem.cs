// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.MoonLordPortraitBackgroundProviderBestiaryInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class MoonLordPortraitBackgroundProviderBestiaryInfoElement : 
    IBestiaryInfoElement,
    IBestiaryBackgroundImagePathAndColorProvider
  {
    public Asset<Texture2D> GetBackgroundImage() => Main.Assets.Request<Texture2D>("Images/MapBG1", (AssetRequestMode) 1);

    public Color? GetBackgroundColor() => new Color?(Color.Black);

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => (UIElement) null;
  }
}
