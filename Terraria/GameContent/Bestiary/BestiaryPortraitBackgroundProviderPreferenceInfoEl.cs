// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.BestiaryPortraitBackgroundProviderPreferenceInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class BestiaryPortraitBackgroundProviderPreferenceInfoElement : 
    IPreferenceProviderElement,
    IBestiaryInfoElement
  {
    private IBestiaryBackgroundImagePathAndColorProvider _preferredProvider;

    public BestiaryPortraitBackgroundProviderPreferenceInfoElement(
      IBestiaryBackgroundImagePathAndColorProvider preferredProvider)
    {
      this._preferredProvider = preferredProvider;
    }

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => (UIElement) null;

    public bool Matches(
      IBestiaryBackgroundImagePathAndColorProvider provider)
    {
      return provider == this._preferredProvider;
    }

    public IBestiaryBackgroundImagePathAndColorProvider GetPreferredProvider() => this._preferredProvider;
  }
}
