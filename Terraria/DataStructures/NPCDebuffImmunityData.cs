// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.NPCDebuffImmunityData
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.ID;

namespace Terraria.DataStructures
{
  public class NPCDebuffImmunityData
  {
    public bool ImmuneToWhips;
    public bool ImmuneToAllBuffsThatAreNotWhips;
    public int[] SpecificallyImmuneTo;

    public void ApplyToNPC(NPC npc)
    {
      if (this.ImmuneToWhips || this.ImmuneToAllBuffsThatAreNotWhips)
      {
        for (int index = 1; index < 338; ++index)
        {
          bool flag1 = BuffID.Sets.IsAnNPCWhipDebuff[index];
          bool flag2 = ((((false ? 1 : 0) | (!flag1 ? 0 : (this.ImmuneToWhips ? 1 : 0))) != 0 ? 1 : 0) | (flag1 ? 0 : (this.ImmuneToAllBuffsThatAreNotWhips ? 1 : 0))) != 0;
          npc.buffImmune[index] = flag2;
        }
      }
      if (this.SpecificallyImmuneTo == null)
        return;
      for (int index1 = 0; index1 < this.SpecificallyImmuneTo.Length; ++index1)
      {
        int index2 = this.SpecificallyImmuneTo[index1];
        npc.buffImmune[index2] = true;
      }
    }
  }
}
