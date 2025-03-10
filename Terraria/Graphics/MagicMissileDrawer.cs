﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.MagicMissileDrawer
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
  public struct MagicMissileDrawer
  {
    private static VertexStrip _vertexStrip = new VertexStrip();

    public void Draw(Projectile proj)
    {
      MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
      miscShaderData.UseSaturation(-2.8f);
      miscShaderData.UseOpacity(2f);
      miscShaderData.Apply(new DrawData?());
      MagicMissileDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, new VertexStrip.StripColorFunction(this.StripColors), new VertexStrip.StripHalfWidthFunction(this.StripWidth), -Main.screenPosition + proj.Size / 2f);
      MagicMissileDrawer._vertexStrip.DrawTrail();
      Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    private Color StripColors(float progressOnStrip)
    {
      Color color = Color.Lerp(Color.White, Color.Violet, Utils.GetLerpValue(0.0f, 0.7f, progressOnStrip, true)) * (1f - Utils.GetLerpValue(0.0f, 0.98f, progressOnStrip, false));
      color.A /= (byte) 2;
      return color;
    }

    private float StripWidth(float progressOnStrip) => MathHelper.Lerp(26f, 32f, Utils.GetLerpValue(0.0f, 0.2f, progressOnStrip, true)) * Utils.GetLerpValue(0.0f, 0.07f, progressOnStrip, true);
  }
}
