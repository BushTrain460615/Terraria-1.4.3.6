﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.BigProgressBar.VortexPillarBigProgressBar
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.UI.BigProgressBar
{
  public class VortexPillarBigProgressBar : LunarPillarBigProgessBar
  {
    internal override float GetCurrentShieldValue() => (float) NPC.ShieldStrengthTowerVortex;

    internal override float GetMaxShieldValue() => (float) NPC.ShieldStrengthTowerMax;

    internal override bool IsPlayerInCombatArea() => Main.LocalPlayer.ZoneTowerVortex;
  }
}
