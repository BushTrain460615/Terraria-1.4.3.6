// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.NPCKillAttempt
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public struct NPCKillAttempt
  {
    public readonly NPC npc;
    public readonly int netId;
    public readonly bool active;

    public NPCKillAttempt(NPC target)
    {
      this.npc = target;
      this.netId = target.netID;
      this.active = target.active;
    }

    public bool DidNPCDie() => !this.npc.active;

    public bool DidNPCDieOrTransform() => this.DidNPCDie() || this.npc.netID != this.netId;
  }
}
