﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.WindowStateController
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Drawing;
using System.Windows.Forms;

namespace Terraria.Graphics
{
  public class WindowStateController
  {
    public bool CanMoveWindowAcrossScreens => true;

    public string ScreenDeviceName => Main.instance.Window.ScreenDeviceName;

    public void TryMovingToScreen(string screenDeviceName)
    {
      Rectangle bounds;
      if (!this.CanMoveWindowAcrossScreens || !this.TryGetBounds(screenDeviceName, out bounds) || !this.IsVisibleOnAnyScreen(bounds))
        return;
      Form form = (Form) Control.FromHandle(Main.instance.Window.Handle);
      if (!this.WouldViewFitInScreen(form.Bounds, bounds))
        return;
      form.Location = new Point(bounds.Width / 2 - form.Width / 2 + bounds.X, bounds.Height / 2 - form.Height / 2 + bounds.Y);
    }

    private bool TryGetBounds(string screenDeviceName, out Rectangle bounds)
    {
      bounds = new Rectangle();
      foreach (Screen allScreen in Screen.AllScreens)
      {
        if (allScreen.DeviceName == screenDeviceName)
        {
          bounds = allScreen.Bounds;
          return true;
        }
      }
      return false;
    }

    private bool WouldViewFitInScreen(Rectangle view, Rectangle screen) => view.Width <= screen.Width && view.Height <= screen.Height;

    private bool IsVisibleOnAnyScreen(Rectangle rect)
    {
      foreach (Screen allScreen in Screen.AllScreens)
      {
        if (allScreen.WorkingArea.IntersectsWith(rect))
          return true;
      }
      return false;
    }
  }
}
