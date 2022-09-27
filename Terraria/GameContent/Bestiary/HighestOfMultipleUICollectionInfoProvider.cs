// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.HighestOfMultipleUICollectionInfoProvider
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class HighestOfMultipleUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
  {
    private IBestiaryUICollectionInfoProvider[] _providers;
    private int _mainProviderIndex;

    public HighestOfMultipleUICollectionInfoProvider(
      params IBestiaryUICollectionInfoProvider[] providers)
    {
      this._providers = providers;
      this._mainProviderIndex = 0;
    }

    public BestiaryUICollectionInfo GetEntryUICollectionInfo()
    {
      BestiaryUICollectionInfo uiCollectionInfo1 = this._providers[this._mainProviderIndex].GetEntryUICollectionInfo();
      BestiaryEntryUnlockState unlockState = uiCollectionInfo1.UnlockState;
      for (int index = 0; index < this._providers.Length; ++index)
      {
        BestiaryUICollectionInfo uiCollectionInfo2 = this._providers[index].GetEntryUICollectionInfo();
        if (unlockState < uiCollectionInfo2.UnlockState)
          unlockState = uiCollectionInfo2.UnlockState;
      }
      uiCollectionInfo1.UnlockState = unlockState;
      return uiCollectionInfo1;
    }

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => (UIElement) null;
  }
}
