// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Events.CreditsRollEvent
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Events
{
  public class CreditsRollEvent
  {
    private const int MAX_TIME_FOR_CREDITS_ROLL_IN_FRAMES = 28800;
    private static int _creditsRollRemainingTime;

    public static bool IsEventOngoing => CreditsRollEvent._creditsRollRemainingTime > 0;

    public static void TryStartingCreditsRoll()
    {
      CreditsRollEvent._creditsRollRemainingTime = 28800;
      if (SkyManager.Instance["CreditsRoll"] is CreditsRollSky creditsRollSky)
        CreditsRollEvent._creditsRollRemainingTime = creditsRollSky.AmountOfTimeNeededForFullPlay;
      if (Main.netMode != 2)
        return;
      NetMessage.SendData(140, number2: ((float) CreditsRollEvent._creditsRollRemainingTime));
    }

    public static void SendCreditsRollRemainingTimeToPlayer(int playerIndex)
    {
      if (CreditsRollEvent._creditsRollRemainingTime == 0 || Main.netMode != 2)
        return;
      NetMessage.SendData(140, playerIndex, number2: ((float) CreditsRollEvent._creditsRollRemainingTime));
    }

    public static void UpdateTime() => CreditsRollEvent._creditsRollRemainingTime = Utils.Clamp<int>(CreditsRollEvent._creditsRollRemainingTime - 1, 0, 28800);

    public static void Reset() => CreditsRollEvent._creditsRollRemainingTime = 0;

    public static void SetRemainingTimeDirect(int time) => CreditsRollEvent._creditsRollRemainingTime = Utils.Clamp<int>(time, 0, 28800);
  }
}
