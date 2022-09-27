// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Shaders.SepiaScreenShaderData
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
  public class SepiaScreenShaderData : ScreenShaderData
  {
    public SepiaScreenShaderData(string passName)
      : base(passName)
    {
    }

    public override void Update(GameTime gameTime)
    {
      float x = (float) (((double) Main.screenPosition.Y + (double) (Main.screenHeight / 2)) / 16.0);
      float num1 = 1f - Utils.SmoothStep((float) Main.worldSurface, (float) Main.worldSurface + 30f, x);
      Vector3 vector3_1;
      Vector3 vector3_2 = vector3_1 = new Vector3(0.191f, -0.054f, -0.221f);
      Vector3 vector3_3 = vector3_1 * 0.5f;
      Vector3 vector3_4 = new Vector3(0.0f, -0.03f, 0.15f);
      Vector3 vector3_5 = new Vector3(-0.11f, 0.01f, 0.16f);
      float cloudAlpha = Main.cloudAlpha;
      float nightlightPower;
      float daylightPower;
      float moonPower;
      float dawnPower;
      SepiaScreenShaderData.GetDaylightPowers(out nightlightPower, out daylightPower, out moonPower, out dawnPower);
      float num2 = nightlightPower * 0.13f;
      if (!Main.dayTime)
      {
        if (Main.GetMoonPhase() == MoonPhase.Full)
        {
          vector3_2 = new Vector3(-0.19f, 0.01f, 0.22f);
          num2 += 0.07f * moonPower;
        }
        if (Main.bloodMoon)
        {
          vector3_2 = new Vector3(0.2f, -0.1f, -0.221f);
          num2 = 0.2f;
        }
      }
      float num3 = nightlightPower * num1;
      float num4 = daylightPower * num1;
      float amount1 = moonPower * num1;
      float amount2 = dawnPower * num1;
      this.UseOpacity(1f);
      this.UseIntensity((float) (1.39999997615814 - (double) num4 * 0.200000002980232));
      this.UseProgress(MathHelper.Lerp(MathHelper.Lerp((float) (0.300000011920929 - (double) num2 * (double) num3), 0.1f, cloudAlpha), 0.2f, 1f - num1));
      this.UseColor(Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(Vector3.Lerp(vector3_1, vector3_2, amount1), vector3_4, amount2), vector3_5, cloudAlpha), vector3_3, 1f - num1));
    }

    private static void GetDaylightPowers(
      out float nightlightPower,
      out float daylightPower,
      out float moonPower,
      out float dawnPower)
    {
      nightlightPower = 0.0f;
      daylightPower = 0.0f;
      moonPower = 0.0f;
      Vector2 directionIn24Hclock1 = Utils.GetDayTimeAsDirectionIn24HClock();
      Vector2 directionIn24Hclock2 = Utils.GetDayTimeAsDirectionIn24HClock(4.5f);
      float fromValue1 = Vector2.Dot(directionIn24Hclock1, Utils.GetDayTimeAsDirectionIn24HClock(0.0f));
      float fromValue2 = Vector2.Dot(directionIn24Hclock1, directionIn24Hclock2);
      nightlightPower = Utils.Remap(fromValue1, -0.2f, 0.1f, 0.0f, 1f);
      daylightPower = Utils.Remap(fromValue1, 0.1f, -1f, 0.0f, 1f);
      dawnPower = Utils.Remap(fromValue2, 0.66f, 1f, 0.0f, 1f);
      if (Main.dayTime)
        return;
      float fromValue3 = (float) (Main.time / 32400.0) * 2f;
      if ((double) fromValue3 > 1.0)
        fromValue3 = 2f - fromValue3;
      moonPower = Utils.Remap(fromValue3, 0.0f, 0.25f, 0.0f, 1f);
    }
  }
}
