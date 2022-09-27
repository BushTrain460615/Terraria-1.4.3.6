// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.RGB.LowLifeShader
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;

namespace Terraria.GameContent.RGB
{
  public class LowLifeShader : ChromaShader
  {
    private static Vector4 _baseColor = new Color(40, 0, 8, (int) byte.MaxValue).ToVector4();

    [RgbProcessor]
    private void ProcessAnyDetail(
      RgbDevice device,
      Fragment fragment,
      EffectDetailLevel quality,
      float time)
    {
      float num = (float) (Math.Cos((double) time * 3.14159274101257) * 0.300000011920929 + 0.699999988079071);
      Vector4 vector4 = (LowLifeShader._baseColor * num) with
      {
        W = LowLifeShader._baseColor.W
      };
      for (int index = 0; index < fragment.Count; ++index)
        fragment.SetColor(index, vector4);
    }
  }
}
