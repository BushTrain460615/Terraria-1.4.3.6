﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Shaders.ScreenShaderData
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Shaders
{
  public class ScreenShaderData : ShaderData
  {
    private Vector3 _uColor = Vector3.One;
    private Vector3 _uSecondaryColor = Vector3.One;
    private float _uOpacity = 1f;
    private float _globalOpacity = 1f;
    private float _uIntensity = 1f;
    private Vector2 _uTargetPosition = Vector2.One;
    private Vector2 _uDirection = new Vector2(0.0f, 1f);
    private float _uProgress;
    private Vector2 _uImageOffset = Vector2.Zero;
    private Asset<Texture2D>[] _uAssetImages = new Asset<Texture2D>[3];
    private Texture2D[] _uCustomImages = new Texture2D[3];
    private SamplerState[] _samplerStates = new SamplerState[3];
    private Vector2[] _imageScales = new Vector2[3]
    {
      Vector2.One,
      Vector2.One,
      Vector2.One
    };

    public float Intensity => this._uIntensity;

    public float CombinedOpacity => this._uOpacity * this._globalOpacity;

    public ScreenShaderData(string passName)
      : base(Main.ScreenShaderRef, passName)
    {
    }

    public ScreenShaderData(Ref<Effect> shader, string passName)
      : base(shader, passName)
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public override void Apply()
    {
      Vector2 vector2_1 = new Vector2((float) Main.offScreenRange, (float) Main.offScreenRange);
      Vector2 vector2_2 = new Vector2((float) Main.screenWidth, (float) Main.screenHeight) / Main.GameViewMatrix.Zoom;
      Vector2 vector2_3 = new Vector2((float) Main.screenWidth, (float) Main.screenHeight) * 0.5f;
      Vector2 vector2_4 = Main.screenPosition + vector2_3 * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
      this.Shader.Parameters["uColor"].SetValue(this._uColor);
      this.Shader.Parameters["uOpacity"].SetValue(this.CombinedOpacity);
      this.Shader.Parameters["uSecondaryColor"].SetValue(this._uSecondaryColor);
      this.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
      this.Shader.Parameters["uScreenResolution"].SetValue(vector2_2);
      this.Shader.Parameters["uScreenPosition"].SetValue(vector2_4 - vector2_1);
      this.Shader.Parameters["uTargetPosition"].SetValue(this._uTargetPosition - vector2_1);
      this.Shader.Parameters["uImageOffset"].SetValue(this._uImageOffset);
      this.Shader.Parameters["uIntensity"].SetValue(this._uIntensity);
      this.Shader.Parameters["uProgress"].SetValue(this._uProgress);
      this.Shader.Parameters["uDirection"].SetValue(this._uDirection);
      this.Shader.Parameters["uZoom"].SetValue(Main.GameViewMatrix.Zoom);
      for (int index = 0; index < this._uAssetImages.Length; ++index)
      {
        Texture2D uCustomImage = this._uCustomImages[index];
        if (this._uAssetImages[index] != null && this._uAssetImages[index].IsLoaded)
          uCustomImage = this._uAssetImages[index].Value;
        if (uCustomImage != null)
        {
          Main.graphics.GraphicsDevice.Textures[index + 1] = (Texture) uCustomImage;
          int width = uCustomImage.Width;
          int height = uCustomImage.Height;
          Main.graphics.GraphicsDevice.SamplerStates[index + 1] = this._samplerStates[index] == null ? (!Utils.IsPowerOfTwo(width) || !Utils.IsPowerOfTwo(height) ? SamplerState.AnisotropicClamp : SamplerState.LinearWrap) : this._samplerStates[index];
          this.Shader.Parameters["uImageSize" + (index + 1).ToString()].SetValue(new Vector2((float) width, (float) height) * this._imageScales[index]);
        }
      }
      base.Apply();
    }

    public ScreenShaderData UseImageOffset(Vector2 offset)
    {
      this._uImageOffset = offset;
      return this;
    }

    public ScreenShaderData UseIntensity(float intensity)
    {
      this._uIntensity = intensity;
      return this;
    }

    public ScreenShaderData UseColor(float r, float g, float b) => this.UseColor(new Vector3(r, g, b));

    public ScreenShaderData UseProgress(float progress)
    {
      this._uProgress = progress;
      return this;
    }

    public ScreenShaderData UseImage(
      Texture2D image,
      int index = 0,
      SamplerState samplerState = null)
    {
      this._samplerStates[index] = samplerState;
      this._uAssetImages[index] = (Asset<Texture2D>) null;
      this._uCustomImages[index] = image;
      return this;
    }

    public ScreenShaderData UseImage(
      string path,
      int index = 0,
      SamplerState samplerState = null)
    {
      this._uAssetImages[index] = Main.Assets.Request<Texture2D>(path, (AssetRequestMode) 1);
      this._uCustomImages[index] = (Texture2D) null;
      this._samplerStates[index] = samplerState;
      return this;
    }

    public ScreenShaderData UseColor(Color color) => this.UseColor(color.ToVector3());

    public ScreenShaderData UseColor(Vector3 color)
    {
      this._uColor = color;
      return this;
    }

    public ScreenShaderData UseDirection(Vector2 direction)
    {
      this._uDirection = direction;
      return this;
    }

    public ScreenShaderData UseGlobalOpacity(float opacity)
    {
      this._globalOpacity = opacity;
      return this;
    }

    public ScreenShaderData UseTargetPosition(Vector2 position)
    {
      this._uTargetPosition = position;
      return this;
    }

    public ScreenShaderData UseSecondaryColor(float r, float g, float b) => this.UseSecondaryColor(new Vector3(r, g, b));

    public ScreenShaderData UseSecondaryColor(Color color) => this.UseSecondaryColor(color.ToVector3());

    public ScreenShaderData UseSecondaryColor(Vector3 color)
    {
      this._uSecondaryColor = color;
      return this;
    }

    public ScreenShaderData UseOpacity(float opacity)
    {
      this._uOpacity = opacity;
      return this;
    }

    public ScreenShaderData UseImageScale(Vector2 scale, int index = 0)
    {
      this._imageScales[index] = scale;
      return this;
    }

    public virtual ScreenShaderData GetSecondaryShader(Player player) => this;
  }
}
