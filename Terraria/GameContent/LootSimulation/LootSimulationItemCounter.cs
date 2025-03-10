﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.LootSimulation.LootSimulationItemCounter
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.GameContent.LootSimulation
{
  public class LootSimulationItemCounter
  {
    private long[] _itemCountsObtained = new long[5125];
    private long[] _itemCountsObtainedExpert = new long[5125];
    private long _totalTimesAttempted;
    private long _totalTimesAttemptedExpert;

    public void AddItem(int itemId, int amount, bool expert)
    {
      if (expert)
        this._itemCountsObtainedExpert[itemId] += (long) amount;
      else
        this._itemCountsObtained[itemId] += (long) amount;
    }

    public void Exclude(params int[] itemIds)
    {
      foreach (int itemId in itemIds)
      {
        this._itemCountsObtained[itemId] = 0L;
        this._itemCountsObtainedExpert[itemId] = 0L;
      }
    }

    public void IncreaseTimesAttempted(int amount, bool expert)
    {
      if (expert)
        this._totalTimesAttemptedExpert += (long) amount;
      else
        this._totalTimesAttempted += (long) amount;
    }

    public string PrintCollectedItems(bool expert)
    {
      long[] collectionToUse = this._itemCountsObtained;
      long totalDropsAttempted = this._totalTimesAttempted;
      if (expert)
      {
        collectionToUse = this._itemCountsObtainedExpert;
        this._totalTimesAttempted = this._totalTimesAttemptedExpert;
      }
      return string.Join(",\n", ((IEnumerable<long>) collectionToUse).Select((count, itemId) => new
      {
        itemId = itemId,
        count = count
      }).Where(entry => entry.count > 0L).Select(entry => entry.itemId).Select<int, string>((Func<int, string>) (itemId => string.Format("new ItemDropInfo(ItemID.{0}, {1}, {2})", (object) ItemID.Search.GetName(itemId), (object) collectionToUse[itemId], (object) totalDropsAttempted))));
    }
  }
}
