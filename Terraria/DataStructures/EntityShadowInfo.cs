// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.EntityShadowInfo
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
  public struct EntityShadowInfo
  {
    public Vector2 Position;
    public float Rotation;
    public Vector2 Origin;
    public int Direction;
    public int GravityDirection;
    public int BodyFrameIndex;

    public void CopyPlayer(Player player)
    {
      this.Position = player.position;
      this.Rotation = player.fullRotation;
      this.Origin = player.fullRotationOrigin;
      this.Direction = player.direction;
      this.GravityDirection = (int) player.gravDir;
      this.BodyFrameIndex = player.bodyFrame.Y / player.bodyFrame.Height;
    }

    public Vector2 HeadgearOffset => Main.OffsetsPlayerHeadgear[this.BodyFrameIndex];
  }
}
