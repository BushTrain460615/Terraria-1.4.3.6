﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.RGB.TempleShader
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;

namespace Terraria.GameContent.RGB
{
  public class TempleShader : ChromaShader
  {
    private readonly Vector4 _backgroundColor = new Vector4(0.05f, 0.025f, 0.0f, 1f);
    private readonly Vector4 _glowColor = Color.Orange.ToVector4();

    [RgbProcessor]
    private void ProcessHighDetail(
      RgbDevice device,
      Fragment fragment,
      EffectDetailLevel quality,
      float time)
    {
      for (int index = 0; index < fragment.Count; ++index)
      {
        Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(index);
        Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(index);
        Vector4 vector4 = this._backgroundColor;
        float num = (float) (((double) NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y * 7) * 10.0 + (double) time) % 10.0 - ((double) canvasPositionOfIndex.X + 2.0));
        if ((double) num > 0.0)
        {
          float amount = Math.Max(0.0f, 1.2f - num);
          if ((double) num < 0.200000002980232)
            amount = num * 5f;
          vector4 = Vector4.Lerp(vector4, this._glowColor, amount);
        }
        fragment.SetColor(index, vector4);
      }
    }
  }
}
