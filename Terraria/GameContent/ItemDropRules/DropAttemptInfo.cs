﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemDropRules.DropAttemptInfo
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.Utilities;

namespace Terraria.GameContent.ItemDropRules
{
  public struct DropAttemptInfo
  {
    public NPC npc;
    public Player player;
    public UnifiedRandom rng;
    public bool IsInSimulation;
    public bool IsExpertMode;
    public bool IsMasterMode;
  }
}
