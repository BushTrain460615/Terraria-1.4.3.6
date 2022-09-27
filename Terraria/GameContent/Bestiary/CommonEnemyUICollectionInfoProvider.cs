// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.CommonEnemyUICollectionInfoProvider
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class CommonEnemyUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
  {
    private string _persistentIdentifierToCheck;
    private bool _quickUnlock;

    public CommonEnemyUICollectionInfoProvider(string persistentId, bool quickUnlock)
    {
      this._persistentIdentifierToCheck = persistentId;
      this._quickUnlock = quickUnlock;
    }

    public BestiaryUICollectionInfo GetEntryUICollectionInfo()
    {
      BestiaryEntryUnlockState stateByKillCount = CommonEnemyUICollectionInfoProvider.GetUnlockStateByKillCount(Main.BestiaryTracker.Kills.GetKillCount(this._persistentIdentifierToCheck), this._quickUnlock);
      return new BestiaryUICollectionInfo()
      {
        UnlockState = stateByKillCount
      };
    }

    public static BestiaryEntryUnlockState GetUnlockStateByKillCount(
      int killCount,
      bool quickUnlock)
    {
      return !quickUnlock || killCount <= 0 ? (killCount < 50 ? (killCount < 25 ? (killCount < 10 ? (killCount < 1 ? BestiaryEntryUnlockState.NotKnownAtAll_0 : BestiaryEntryUnlockState.CanShowPortraitOnly_1) : BestiaryEntryUnlockState.CanShowStats_2) : BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3) : BestiaryEntryUnlockState.CanShowDropsWithDropRates_4) : BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
    }

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => (UIElement) null;
  }
}
