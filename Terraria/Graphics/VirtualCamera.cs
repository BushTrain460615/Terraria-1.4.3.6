// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.VirtualCamera
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.Graphics
{
  public struct VirtualCamera
  {
    public readonly Player Player;

    public VirtualCamera(Player player) => this.Player = player;

    public Vector2 Position => this.Center - this.Size * 0.5f;

    public Vector2 Size => new Vector2((float) Main.maxScreenW, (float) Main.maxScreenH);

    public Vector2 Center => this.Player.Center;
  }
}
