// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ObjectInteractions.PotionOfReturnGateInteractionChecker
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
  public class PotionOfReturnGateInteractionChecker : AHoverInteractionChecker
  {
    internal override bool? AttemptOverridingHoverStatus(Player player, Rectangle rectangle) => Main.SmartInteractPotionOfReturn ? new bool?(true) : new bool?();

    internal override void DoHoverEffect(Player player, Rectangle hitbox)
    {
      player.noThrow = 2;
      player.cursorItemIconEnabled = true;
      player.cursorItemIconID = 4870;
    }

    internal override bool ShouldBlockInteraction(Player player, Rectangle hitbox) => (uint) Player.BlockInteractionWithProjectiles > 0U;

    internal override void PerformInteraction(Player player, Rectangle hitbox) => player.DoPotionOfReturnReturnToOriginalUsePosition();
  }
}
