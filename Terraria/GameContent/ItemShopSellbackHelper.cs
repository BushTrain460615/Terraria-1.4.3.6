﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemShopSellbackHelper
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria.GameContent
{
  public class ItemShopSellbackHelper
  {
    private List<ItemShopSellbackHelper.ItemMemo> _memos = new List<ItemShopSellbackHelper.ItemMemo>();

    public void Add(Item item)
    {
      ItemShopSellbackHelper.ItemMemo itemMemo = this._memos.Find((Predicate<ItemShopSellbackHelper.ItemMemo>) (x => x.Matches(item)));
      if (itemMemo != null)
        itemMemo.stack += item.stack;
      else
        this._memos.Add(new ItemShopSellbackHelper.ItemMemo(item));
    }

    public void Clear() => this._memos.Clear();

    public int GetAmount(Item item)
    {
      ItemShopSellbackHelper.ItemMemo itemMemo = this._memos.Find((Predicate<ItemShopSellbackHelper.ItemMemo>) (x => x.Matches(item)));
      return itemMemo != null ? itemMemo.stack : 0;
    }

    public int Remove(Item item)
    {
      ItemShopSellbackHelper.ItemMemo itemMemo = this._memos.Find((Predicate<ItemShopSellbackHelper.ItemMemo>) (x => x.Matches(item)));
      if (itemMemo == null)
        return 0;
      int stack = itemMemo.stack;
      itemMemo.stack -= item.stack;
      if (itemMemo.stack > 0)
        return stack - itemMemo.stack;
      this._memos.Remove(itemMemo);
      return stack;
    }

    private class ItemMemo
    {
      public readonly int itemNetID;
      public readonly int itemPrefix;
      public int stack;

      public ItemMemo(Item item)
      {
        this.itemNetID = item.netID;
        this.itemPrefix = (int) item.prefix;
        this.stack = item.stack;
      }

      public bool Matches(Item item) => item.netID == this.itemNetID && (int) item.prefix == this.itemPrefix;
    }
  }
}
