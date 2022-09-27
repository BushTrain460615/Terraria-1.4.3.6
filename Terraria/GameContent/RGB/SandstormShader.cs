// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.RGB.SandstormShader
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
  public class SandstormShader : ChromaShader
  {
    private readonly Vector4 _backColor = new Vector4(0.2f, 0.0f, 0.0f, 1f);
    private readonly Vector4 _frontColor = new Vector4(1f, 0.5f, 0.0f, 1f);

    [RgbProcessor]
    private void ProcessHighDetail(
      RgbDevice device,
      Fragment fragment,
      EffectDetailLevel quality,
      float time)
    {
      if (quality == null)
        time *= 0.25f;
      for (int index = 0; index < fragment.Count; ++index)
      {
        Vector4 vector4 = Vector4.Lerp(this._backColor, this._frontColor, NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(index) * 0.3f + new Vector2(time, -time) * 0.5f));
        fragment.SetColor(index, vector4);
      }
    }
  }
}
