// Decompiled with JetBrains decompiler
// Type: Terraria.NPCSpawnParams
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.DataStructures;

namespace Terraria
{
  public struct NPCSpawnParams
  {
    public float? sizeScaleOverride;
    public int? playerCountForMultiplayerDifficultyOverride;
    public GameModeData gameModeData;
    public float? strengthMultiplierOverride;

    public NPCSpawnParams WithScale(float scaleOverride) => new NPCSpawnParams()
    {
      sizeScaleOverride = new float?(scaleOverride),
      playerCountForMultiplayerDifficultyOverride = this.playerCountForMultiplayerDifficultyOverride,
      gameModeData = this.gameModeData,
      strengthMultiplierOverride = this.strengthMultiplierOverride
    };
  }
}
