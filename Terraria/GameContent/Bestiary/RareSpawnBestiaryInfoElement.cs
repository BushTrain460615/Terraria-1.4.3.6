// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.RareSpawnBestiaryInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class RareSpawnBestiaryInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
  {
    public int RarityLevel { get; private set; }

    public RareSpawnBestiaryInfoElement(int rarityLevel) => this.RarityLevel = rarityLevel;

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => (UIElement) null;

    public string GetSearchString(ref BestiaryUICollectionInfo info) => info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0 ? (string) null : Language.GetText("BestiaryInfo.IsRare").Value;
  }
}
