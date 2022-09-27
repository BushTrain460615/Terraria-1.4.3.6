// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.CameraModifiers.CameraInfo
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers
{
  public struct CameraInfo
  {
    public Vector2 CameraPosition;
    public Vector2 OriginalCameraCenter;
    public Vector2 OriginalCameraPosition;

    public CameraInfo(Vector2 position)
    {
      this.OriginalCameraPosition = position;
      this.OriginalCameraCenter = position + Main.ScreenSize.ToVector2() / 2f;
      this.CameraPosition = this.OriginalCameraPosition;
    }
  }
}
