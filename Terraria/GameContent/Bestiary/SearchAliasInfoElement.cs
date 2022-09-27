// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.SearchAliasInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class SearchAliasInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
  {
    private readonly string _alias;

    public SearchAliasInfoElement(string alias) => this._alias = alias;

    public string GetSearchString(ref BestiaryUICollectionInfo info) => info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0 ? (string) null : this._alias;

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => (UIElement) null;
  }
}
