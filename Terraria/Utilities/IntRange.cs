// Decompiled with JetBrains decompiler
// Type: Terraria.Utilities.IntRange
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Newtonsoft.Json;

namespace Terraria.Utilities
{
  public struct IntRange
  {
    [JsonProperty("Min")]
    public readonly int Minimum;
    [JsonProperty("Max")]
    public readonly int Maximum;

    public IntRange(int minimum, int maximum)
    {
      this.Minimum = minimum;
      this.Maximum = maximum;
    }

    public static IntRange operator *(IntRange range, float scale) => new IntRange((int) ((double) range.Minimum * (double) scale), (int) ((double) range.Maximum * (double) scale));

    public static IntRange operator *(float scale, IntRange range) => range * scale;

    public static IntRange operator /(IntRange range, float scale) => new IntRange((int) ((double) range.Minimum / (double) scale), (int) ((double) range.Maximum / (double) scale));

    public static IntRange operator /(float scale, IntRange range) => range / scale;
  }
}
