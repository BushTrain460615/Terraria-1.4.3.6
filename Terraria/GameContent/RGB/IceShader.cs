﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.RGB.IceShader
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;

namespace Terraria.GameContent.RGB
{
  public class IceShader : ChromaShader
  {
    private readonly Vector4 _baseColor;
    private readonly Vector4 _iceColor;
    private readonly Vector4 _shineColor = new Vector4(1f, 1f, 0.7f, 1f);

    public IceShader(Color baseColor, Color iceColor)
    {
      this._baseColor = baseColor.ToVector4();
      this._iceColor = iceColor.ToVector4();
    }

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
        float amount1 = Math.Max(0.0f, (float) (1.0 - (double) NoiseHelper.GetDynamicNoise(new Vector2((float) (((double) canvasPositionOfIndex.X - (double) canvasPositionOfIndex.Y) * 0.200000002980232), 0.0f), time / 5f) * 1.5));
        float amount2 = Math.Max(0.0f, (float) (1.0 - (double) NoiseHelper.GetDynamicNoise(new Vector2((float) (((double) canvasPositionOfIndex.X - (double) canvasPositionOfIndex.Y) * 0.300000011920929), 0.3f), time / 20f) * 5.0));
        Vector4 vector4 = Vector4.Lerp(Vector4.Lerp(this._baseColor, this._iceColor, amount1), this._shineColor, amount2);
        fragment.SetColor(index, vector4);
      }
    }
  }
}
