﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.FlameLashDrawer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
  public struct FlameLashDrawer
  {
    private static VertexStrip _vertexStrip = new VertexStrip();
    private float transitToDark;

    public void Draw(Projectile proj)
    {
      this.transitToDark = Utils.GetLerpValue(0.0f, 6f, proj.localAI[0], true);
      MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
      miscShaderData.UseSaturation(-2f);
      miscShaderData.UseOpacity(MathHelper.Lerp(4f, 8f, this.transitToDark));
      miscShaderData.Apply(new DrawData?());
      FlameLashDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f);
      FlameLashDrawer._vertexStrip.DrawTrail();
      Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    private Color StripColors(float progressOnStrip)
    {
      float lerpValue = Utils.GetLerpValue((float) (0.0 - 0.100000001490116 * (double) this.transitToDark), (float) (0.699999988079071 - 0.200000002980232 * (double) this.transitToDark), progressOnStrip, true);
      Color color = Color.Lerp(Color.Lerp(Color.White, Color.Orange, this.transitToDark * 0.5f), Color.Red, lerpValue) * (1f - Utils.GetLerpValue(0.0f, 0.98f, progressOnStrip, false));
      color.A /= (byte) 8;
      return color;
    }

    private float StripWidth(float progressOnStrip)
    {
      float lerpValue = Utils.GetLerpValue(0.0f, (float) (0.0599999986588955 + (double) this.transitToDark * 0.00999999977648258), progressOnStrip, true);
      float num = (float) (1.0 - (1.0 - (double) lerpValue) * (1.0 - (double) lerpValue));
      return MathHelper.Lerp((float) (24.0 + (double) this.transitToDark * 16.0), 8f, Utils.GetLerpValue(0.0f, 1f, progressOnStrip, true)) * num;
    }
  }
}
