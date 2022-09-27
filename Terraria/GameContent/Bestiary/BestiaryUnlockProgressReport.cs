// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.BestiaryUnlockProgressReport
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.Bestiary
{
  public struct BestiaryUnlockProgressReport
  {
    public int EntriesTotal;
    public float CompletionAmountTotal;

    public float CompletionPercent => this.EntriesTotal == 0 ? 1f : this.CompletionAmountTotal / (float) this.EntriesTotal;
  }
}
