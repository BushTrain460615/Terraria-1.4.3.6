﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.ItemPickupCondition
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
  public class ItemPickupCondition : AchievementCondition
  {
    private const string Identifier = "ITEM_PICKUP";
    private static Dictionary<short, List<ItemPickupCondition>> _listeners = new Dictionary<short, List<ItemPickupCondition>>();
    private static bool _isListenerHooked;
    private short[] _itemIds;

    private ItemPickupCondition(short itemId)
      : base("ITEM_PICKUP_" + itemId.ToString())
    {
      this._itemIds = new short[1]{ itemId };
      ItemPickupCondition.ListenForPickup(this);
    }

    private ItemPickupCondition(short[] itemIds)
      : base("ITEM_PICKUP_" + itemIds[0].ToString())
    {
      this._itemIds = itemIds;
      ItemPickupCondition.ListenForPickup(this);
    }

    private static void ListenForPickup(ItemPickupCondition condition)
    {
      if (!ItemPickupCondition._isListenerHooked)
      {
        AchievementsHelper.OnItemPickup += new AchievementsHelper.ItemPickupEvent(ItemPickupCondition.ItemPickupListener);
        ItemPickupCondition._isListenerHooked = true;
      }
      for (int index = 0; index < condition._itemIds.Length; ++index)
      {
        if (!ItemPickupCondition._listeners.ContainsKey(condition._itemIds[index]))
          ItemPickupCondition._listeners[condition._itemIds[index]] = new List<ItemPickupCondition>();
        ItemPickupCondition._listeners[condition._itemIds[index]].Add(condition);
      }
    }

    private static void ItemPickupListener(Player player, short itemId, int count)
    {
      if (player.whoAmI != Main.myPlayer || !ItemPickupCondition._listeners.ContainsKey(itemId))
        return;
      foreach (AchievementCondition achievementCondition in ItemPickupCondition._listeners[itemId])
        achievementCondition.Complete();
    }

    public static AchievementCondition Create(params short[] items) => (AchievementCondition) new ItemPickupCondition(items);

    public static AchievementCondition Create(short item) => (AchievementCondition) new ItemPickupCondition(item);

    public static AchievementCondition[] CreateMany(params short[] items)
    {
      AchievementCondition[] many = new AchievementCondition[items.Length];
      for (int index = 0; index < items.Length; ++index)
        many[index] = (AchievementCondition) new ItemPickupCondition(items[index]);
      return many;
    }
  }
}
