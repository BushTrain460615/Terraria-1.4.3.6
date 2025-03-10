﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.RGB.PillarShader
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using System;

namespace Terraria.GameContent.RGB
{
  public class PillarShader : ChromaShader
  {
    private readonly Vector4 _primaryColor;
    private readonly Vector4 _secondaryColor;

    public PillarShader(Color primaryColor, Color secondaryColor)
    {
      this._primaryColor = primaryColor.ToVector4();
      this._secondaryColor = secondaryColor.ToVector4();
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
        Vector4 vector4 = Vector4.Lerp(this._primaryColor, this._secondaryColor, (float) (Math.Sin((double) time * 2.5 + (double) canvasPositionOfIndex.X) * 0.5 + 0.5));
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
      Vector2 vector2_1 = new Vector2(1.5f, 0.5f);
      time *= 4f;
      for (int index = 0; index < fragment.Count; ++index)
      {
        Vector2 vector2_2 = fragment.GetCanvasPositionOfIndex(index) - vector2_1;
        float num1 = vector2_2.Length() * 2f;
        float num2 = (float) Math.Atan2((double) vector2_2.Y, (double) vector2_2.X);
        Vector4 vector4 = Vector4.Lerp(this._primaryColor, this._secondaryColor, (float) (Math.Sin((double) num1 * 4.0 - (double) time - (double) num2) * 0.5 + 0.5));
        if ((double) num1 < 1.0)
        {
          float num3 = num1 / 1f;
          float num4 = num3 * (num3 * num3);
          vector4 = Vector4.Lerp(this._primaryColor, this._secondaryColor, (float) (Math.Sin(4.0 - (double) time - (double) num2) * 0.5 + 0.5)) * num4;
        }
        vector4.W = 1f;
        fragment.SetColor(index, vector4);
      }
    }
  }
}
