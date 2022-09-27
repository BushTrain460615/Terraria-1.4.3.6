// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.ItemFromCatchingNPCBestiaryInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class ItemFromCatchingNPCBestiaryInfoElement : 
    IItemBestiaryInfoElement,
    IBestiaryInfoElement,
    IProvideSearchFilterString
  {
    private int _itemType;

    public ItemFromCatchingNPCBestiaryInfoElement(int itemId) => this._itemType = itemId;

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => info.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3 ? (UIElement) null : (UIElement) new UIBestiaryInfoLine<string>("catch item #" + this._itemType.ToString());

    public string GetSearchString(ref BestiaryUICollectionInfo info) => info.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3 ? (string) null : ContentSamples.ItemsByType[this._itemType].Name;
  }
}
