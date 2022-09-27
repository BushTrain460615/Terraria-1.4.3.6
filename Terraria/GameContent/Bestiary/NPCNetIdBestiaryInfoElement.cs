// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.NPCNetIdBestiaryInfoElement
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
  public class NPCNetIdBestiaryInfoElement : IBestiaryInfoElement, IBestiaryEntryDisplayIndex
  {
    public int NetId { get; private set; }

    public NPCNetIdBestiaryInfoElement(int npcNetId) => this.NetId = npcNetId;

    public UIElement ProvideUIElement(BestiaryUICollectionInfo info) => (UIElement) null;

    public int BestiaryDisplayIndex => ContentSamples.NpcBestiarySortingId[this.NetId];
  }
}
