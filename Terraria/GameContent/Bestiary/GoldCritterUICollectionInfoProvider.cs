﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.GoldCritterUICollectionInfoProvider
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.ID;

namespace Terraria.GameContent.Bestiary
{
  public class GoldCritterUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
  {
    private string[] _normalCritterPersistentId;
    private string _goldCritterPersistentId;

    public GoldCritterUICollectionInfoProvider(
      int[] normalCritterPersistentId,
      string goldCritterPersistentId)
    {
      this._normalCritterPersistentId = new string[normalCritterPersistentId.Length];
      for (int index = 0; index < normalCritterPersistentId.Length; ++index)
        this._normalCritterPersistentId[index] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[normalCritterPersistentId[index]];
      this._goldCritterPersistentId = goldCritterPersistentId;
    }

    public BestiaryUICollectionInfo GetEntryUICollectionInfo()
    {
      BestiaryEntryUnlockState unlockStateForCritter1 = this.GetUnlockStateForCritter(this._goldCritterPersistentId);
      BestiaryEntryUnlockState entryUnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0;
      if (unlockStateForCritter1 > entryUnlockState)
        entryUnlockState = unlockStateForCritter1;
      foreach (string persistentId in this._normalCritterPersistentId)
      {
        BestiaryEntryUnlockState unlockStateForCritter2 = this.GetUnlockStateForCritter(persistentId);
        if (unlockStateForCritter2 > entryUnlockState)
          entryUnlockState = unlockStateForCritter2;
      }
      BestiaryUICollectionInfo uiCollectionInfo = new BestiaryUICollectionInfo()
      {
        UnlockState = entryUnlockState
      };
      if (entryUnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0 || this.TryFindingOneGoldCritterThatIsAlreadyUnlocked())
        return uiCollectionInfo;
      return new BestiaryUICollectionInfo()
      {
        UnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0
      };
    }

    private bool TryFindingOneGoldCritterThatIsAlreadyUnlocked()
    {
      for (int index = 0; index < NPCID.Sets.GoldCrittersCollection.Count; ++index)
      {
        int goldCritters = NPCID.Sets.GoldCrittersCollection[index];
        if (this.GetUnlockStateForCritter(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[goldCritters]) > BestiaryEntryUnlockState.NotKnownAtAll_0)
          return true;
      }
      return false;
    }

    private BestiaryEntryUnlockState GetUnlockStateForCritter(
      string persistentId)
    {
      return !Main.BestiaryTracker.Sights.GetWasNearbyBefore(persistentId) ? BestiaryEntryUnlockState.NotKnownAtAll_0 : BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
    }
  }
}
