// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.DontStarveDarknessDamageDealer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent
{
  public class DontStarveDarknessDamageDealer
  {
    public const int DARKNESS_DAMAGE_PER_HIT = 50;
    public const int DARKNESS_TIMER_MAX_BEFORE_STARTING_HITS = 300;
    public static int darknessTimer = -1;
    public const int DARKNESS_HIT_TIMER_MAX_BEFORE_HIT = 60;
    public const int DARKNESS_MESSAGE_TIME = 180;
    public static int darknessHitTimer = 0;
    public static bool saidMessage = false;
    public static bool lastFrameWasTooBright = true;

    public static void Reset()
    {
      DontStarveDarknessDamageDealer.ResetTimer();
      DontStarveDarknessDamageDealer.saidMessage = false;
      DontStarveDarknessDamageDealer.lastFrameWasTooBright = true;
    }

    private static void ResetTimer()
    {
      DontStarveDarknessDamageDealer.darknessTimer = -1;
      DontStarveDarknessDamageDealer.darknessHitTimer = 0;
    }

    public static void Update(Player player)
    {
      if (player.DeadOrGhost)
      {
        DontStarveDarknessDamageDealer.ResetTimer();
      }
      else
      {
        DontStarveDarknessDamageDealer.UpdateDarknessState(player);
        if (DontStarveDarknessDamageDealer.darknessTimer < 300)
          return;
        DontStarveDarknessDamageDealer.darknessTimer = 300;
        ++DontStarveDarknessDamageDealer.darknessHitTimer;
        if (DontStarveDarknessDamageDealer.darknessHitTimer <= 60 || player.immune)
          return;
        SoundEngine.PlaySound(SoundID.Item1, player.Center);
        player.Hurt(PlayerDeathReason.ByOther(17), 50, 0);
        DontStarveDarknessDamageDealer.darknessHitTimer = 0;
      }
    }

    private static void UpdateDarknessState(Player player)
    {
      if (DontStarveDarknessDamageDealer.lastFrameWasTooBright = DontStarveDarknessDamageDealer.IsPlayerSafe(player))
      {
        if (DontStarveDarknessDamageDealer.saidMessage)
        {
          Main.NewText(Language.GetTextValue("Game.DarknessSafe"), (byte) 50, (byte) 200, (byte) 50);
          DontStarveDarknessDamageDealer.saidMessage = false;
        }
        DontStarveDarknessDamageDealer.ResetTimer();
      }
      else
      {
        if (DontStarveDarknessDamageDealer.darknessTimer >= 180 && !DontStarveDarknessDamageDealer.saidMessage)
        {
          Main.NewText(Language.GetTextValue("Game.DarknessDanger"), (byte) 200, (byte) 50, (byte) 50);
          DontStarveDarknessDamageDealer.saidMessage = true;
        }
        ++DontStarveDarknessDamageDealer.darknessTimer;
      }
    }

    private static bool IsPlayerSafe(Player player)
    {
      Vector3 vector3 = Lighting.GetColor((int) player.Center.X / 16, (int) player.Center.Y / 16).ToVector3();
      return Main.LocalGolfState == null || !Main.LocalGolfState.ShouldCameraTrackBallLastKnownLocation && !Main.LocalGolfState.IsTrackingBall ? (double) vector3.Length() >= 0.150000005960464 : DontStarveDarknessDamageDealer.lastFrameWasTooBright;
    }
  }
}
