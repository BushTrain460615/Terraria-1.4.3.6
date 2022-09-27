// Decompiled with JetBrains decompiler
// Type: Terraria.UI.StyleDimension
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.UI
{
  public struct StyleDimension
  {
    public static StyleDimension Fill = new StyleDimension(0.0f, 1f);
    public static StyleDimension Empty = new StyleDimension(0.0f, 0.0f);
    public float Pixels;
    public float Precent;

    public StyleDimension(float pixels, float precent)
    {
      this.Pixels = pixels;
      this.Precent = precent;
    }

    public void Set(float pixels, float precent)
    {
      this.Pixels = pixels;
      this.Precent = precent;
    }

    public float GetValue(float containerSize) => this.Pixels + this.Precent * containerSize;

    public static StyleDimension FromPixels(float pixels) => new StyleDimension(pixels, 0.0f);

    public static StyleDimension FromPercent(float percent) => new StyleDimension(0.0f, percent);

    public static StyleDimension FromPixelsAndPercent(float pixels, float percent) => new StyleDimension(pixels, percent);
  }
}
