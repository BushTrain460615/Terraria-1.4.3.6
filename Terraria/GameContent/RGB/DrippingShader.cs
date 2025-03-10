﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.RGB.DrippingShader
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;

namespace Terraria.GameContent.RGB
{
  public class DrippingShader : ChromaShader
  {
    private readonly Vector4 _baseColor;
    private readonly Vector4 _liquidColor;
    private readonly float _viscosity;

    public DrippingShader(Color baseColor, Color liquidColor, float viscosity = 1f)
    {
      this._baseColor = baseColor.ToVector4();
      this._liquidColor = liquidColor.ToVector4();
      this._viscosity = viscosity;
    }

    [RgbProcessor]
    private void ProcessLowDetail(
      RgbDevice device,
      Fragment fragment,
      EffectDetailLevel quality,
      float time)
    {
      for (int index = 0; index < fragment.Count; ++index)
      {
        Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(index);
        Vector4 vector4 = Vector4.Lerp(this._baseColor, this._liquidColor, (float) (Math.Sin((double) time * 0.5 + (double) canvasPositionOfIndex.X) * 0.5 + 0.5));
        fragment.SetColor(index, vector4);
      }
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
        fragment.GetGridPositionOfIndex(index);
        Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(index);
        float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * new Vector2(0.7f * this._viscosity, 0.075f) + new Vector2(0.0f, time * -0.1f * this._viscosity));
        Vector4 vector4 = Vector4.Lerp(this._baseColor, this._liquidColor, Math.Max(0.0f, (float) (1.0 - ((double) canvasPositionOfIndex.Y * 4.5 + 0.5) * (double) staticNoise)));
        fragment.SetColor(index, vector4);
      }
    }
  }
}
