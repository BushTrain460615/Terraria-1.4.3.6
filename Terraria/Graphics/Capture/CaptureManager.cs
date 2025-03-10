﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Capture.CaptureManager
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Terraria.Graphics.Capture
{
  public class CaptureManager : IDisposable
  {
    public static CaptureManager Instance = new CaptureManager();
    private CaptureInterface _interface;
    private CaptureCamera _camera;

    public bool IsCapturing => this._camera.IsCapturing;

    public CaptureManager()
    {
      this._interface = new CaptureInterface();
      this._camera = new CaptureCamera(Main.instance.GraphicsDevice);
    }

    public bool Active
    {
      get => this._interface.Active;
      set
      {
        if (Main.CaptureModeDisabled || this._interface.Active == value)
          return;
        this._interface.ToggleCamera(value);
      }
    }

    public bool UsingMap => this.Active && this._interface.UsingMap();

    public void Scrolling() => this._interface.Scrolling();

    public void Update() => this._interface.Update();

    public void Draw(SpriteBatch sb) => this._interface.Draw(sb);

    public float GetProgress() => this._camera.GetProgress();

    public void Capture() => this.Capture(new CaptureSettings()
    {
      Area = new Rectangle(2660, 100, 1000, 1000),
      UseScaling = false
    });

    public void Capture(CaptureSettings settings) => this._camera.Capture(settings);

    public void DrawTick() => this._camera.DrawTick();

    public void Dispose() => this._camera.Dispose();
  }
}
