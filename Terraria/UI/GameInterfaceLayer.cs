﻿// Decompiled with JetBrains decompiler
// Type: Terraria.UI.GameInterfaceLayer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.GameInput;

namespace Terraria.UI
{
  public class GameInterfaceLayer
  {
    public readonly string Name;
    public InterfaceScaleType ScaleType;

    public GameInterfaceLayer(string name, InterfaceScaleType scaleType)
    {
      this.Name = name;
      this.ScaleType = scaleType;
    }

    public bool Draw()
    {
      Matrix transformMatrix;
      if (this.ScaleType == InterfaceScaleType.Game)
      {
        PlayerInput.SetZoom_World();
        transformMatrix = Main.GameViewMatrix.ZoomMatrix;
      }
      else if (this.ScaleType == InterfaceScaleType.UI)
      {
        PlayerInput.SetZoom_UI();
        transformMatrix = Main.UIScaleMatrix;
      }
      else
      {
        PlayerInput.SetZoom_Unscaled();
        transformMatrix = Matrix.Identity;
      }
      bool flag = false;
      Main.spriteBatch.Begin(SpriteSortMode.Deferred, (BlendState) null, (SamplerState) null, (DepthStencilState) null, (RasterizerState) null, (Effect) null, transformMatrix);
      try
      {
        flag = this.DrawSelf();
      }
      catch (Exception ex)
      {
        TimeLogger.DrawException(ex);
      }
      Main.spriteBatch.End();
      return flag;
    }

    protected virtual bool DrawSelf() => true;
  }
}
