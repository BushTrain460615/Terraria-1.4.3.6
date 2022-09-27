// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.RainbowRodDrawer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct RainbowRodDrawer
  {
    private static VertexStrip _vertexStrip = new VertexStrip();

    public void Draw(Projectile proj)
    {
      MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
      miscShaderData.UseSaturation(-2.8f);
      miscShaderData.UseOpacity(4f);
      miscShaderData.Apply(new DrawData?());
      RainbowRodDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f);
      RainbowRodDrawer._vertexStrip.DrawTrail();
      Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    private Color StripColors(float progressOnStrip) => (Color.Lerp(Color.White, Main.hslToRgb((float) (((double) progressOnStrip * 1.60000002384186 - (double) Main.GlobalTimeWrappedHourly) % 1.0), 1f, 0.5f), Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0.0f, 0.98f, progressOnStrip, false))) with
    {
      A = 0
    };

    private float StripWidth(float progressOnStrip)
    {
      float num = 1f;
      float lerpValue = Utils.GetLerpValue(0.0f, 0.2f, progressOnStrip, true);
      return MathHelper.Lerp(0.0f, 32f, num * (float) (1.0 - (1.0 - (double) lerpValue) * (1.0 - (double) lerpValue)));
    }
  }
}
