﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.UI.Elements.UISlicedImage
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
  public class UISlicedImage : UIElement
  {
    private Asset<Texture2D> _texture;
    private Color _color;
    private int _leftSliceDepth;
    private int _rightSliceDepth;
    private int _topSliceDepth;
    private int _bottomSliceDepth;

    public Color Color
    {
      get => this._color;
      set => this._color = value;
    }

    public UISlicedImage(Asset<Texture2D> texture)
    {
      this._texture = texture;
      this.Width.Set((float) this._texture.Width(), 0.0f);
      this.Height.Set((float) this._texture.Height(), 0.0f);
    }

    public void SetImage(Asset<Texture2D> texture) => this._texture = texture;

    protected override void DrawSelf(SpriteBatch spriteBatch)
    {
      CalculatedStyle dimensions = this.GetDimensions();
      Utils.DrawSplicedPanel(spriteBatch, this._texture.Value, (int) dimensions.X, (int) dimensions.Y, (int) dimensions.Width, (int) dimensions.Height, this._leftSliceDepth, this._rightSliceDepth, this._topSliceDepth, this._bottomSliceDepth, this._color);
    }

    public void SetSliceDepths(int top, int bottom, int left, int right)
    {
      this._leftSliceDepth = left;
      this._rightSliceDepth = right;
      this._topSliceDepth = top;
      this._bottomSliceDepth = bottom;
    }

    public void SetSliceDepths(int fluff)
    {
      this._leftSliceDepth = fluff;
      this._rightSliceDepth = fluff;
      this._topSliceDepth = fluff;
      this._bottomSliceDepth = fluff;
    }
  }
}
