// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.TreePaintingSettings
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
  public class TreePaintingSettings
  {
    public float SpecialGroupMinimalHueValue;
    public float SpecialGroupMaximumHueValue;
    public float SpecialGroupMinimumSaturationValue;
    public float SpecialGroupMaximumSaturationValue;
    public float HueTestOffset;
    public bool UseSpecialGroups;
    public bool UseWallShaderHacks;
    public bool InvertSpecialGroupResult;

    public void ApplyShader(int paintColor, Effect shader)
    {
      shader.Parameters["leafHueTestOffset"].SetValue(this.HueTestOffset);
      shader.Parameters["leafMinHue"].SetValue(this.SpecialGroupMinimalHueValue);
      shader.Parameters["leafMaxHue"].SetValue(this.SpecialGroupMaximumHueValue);
      shader.Parameters["leafMinSat"].SetValue(this.SpecialGroupMinimumSaturationValue);
      shader.Parameters["leafMaxSat"].SetValue(this.SpecialGroupMaximumSaturationValue);
      shader.Parameters["invertSpecialGroupResult"].SetValue(this.InvertSpecialGroupResult);
      int tileShaderIndex = Main.ConvertPaintIdToTileShaderIndex(paintColor, this.UseSpecialGroups, this.UseWallShaderHacks);
      shader.CurrentTechnique.Passes[tileShaderIndex].Apply();
    }
  }
}
