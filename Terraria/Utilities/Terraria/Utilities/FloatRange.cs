// Decompiled with JetBrains decompiler
// Type: Terraria.Utilities.Terraria.Utilities.FloatRange
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Newtonsoft.Json;

namespace Terraria.Utilities.Terraria.Utilities
{
  public struct FloatRange
  {
    [JsonProperty("Min")]
    public readonly float Minimum;
    [JsonProperty("Max")]
    public readonly float Maximum;

    public FloatRange(float minimum, float maximum)
    {
      this.Minimum = minimum;
      this.Maximum = maximum;
    }

    public static FloatRange operator *(FloatRange range, float scale) => new FloatRange(range.Minimum * scale, range.Maximum * scale);

    public static FloatRange operator *(float scale, FloatRange range) => range * scale;

    public static FloatRange operator /(FloatRange range, float scale) => new FloatRange(range.Minimum / scale, range.Maximum / scale);

    public static FloatRange operator /(float scale, FloatRange range) => range / scale;
  }
}
