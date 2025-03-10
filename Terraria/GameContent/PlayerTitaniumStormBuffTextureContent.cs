﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.PlayerTitaniumStormBuffTextureContent
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent
{
  public class PlayerTitaniumStormBuffTextureContent : ARenderTargetContentByRequest
  {
    private MiscShaderData _shaderData;

    public PlayerTitaniumStormBuffTextureContent()
    {
      this._shaderData = new MiscShaderData(Main.PixelShaderRef, "TitaniumStorm");
      this._shaderData.UseImage1("Images/Extra_" + (short) 156.ToString());
    }

    protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
    {
      Main.instance.LoadProjectile(908);
      Asset<Texture2D> asset = TextureAssets.Projectile[908];
      this.UpdateSettingsForRendering(0.6f, 0.0f, Main.GlobalTimeWrappedHourly, 0.3f);
      this.PrepareARenderTarget_AndListenToEvents(ref this._target, device, asset.Width(), asset.Height(), RenderTargetUsage.PreserveContents);
      device.SetRenderTarget(this._target);
      device.Clear(Color.Transparent);
      DrawData drawData = new DrawData(asset.Value, Vector2.Zero, Color.White);
      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
      this._shaderData.Apply(new DrawData?(drawData));
      drawData.Draw(spriteBatch);
      spriteBatch.End();
      device.SetRenderTarget((RenderTarget2D) null);
      this._wasPrepared = true;
    }

    public void UpdateSettingsForRendering(
      float gradientContributionFromOriginalTexture,
      float gradientScrollingSpeed,
      float flatGradientOffset,
      float gradientColorDominance)
    {
      this._shaderData.UseColor(gradientScrollingSpeed, gradientContributionFromOriginalTexture, gradientColorDominance);
      this._shaderData.UseOpacity(flatGradientOffset);
    }
  }
}
