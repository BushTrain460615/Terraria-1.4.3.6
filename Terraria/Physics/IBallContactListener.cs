// Decompiled with JetBrains decompiler
// Type: Terraria.Physics.IBallContactListener
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.Physics
{
  public interface IBallContactListener
  {
    void OnCollision(
      PhysicsProperties properties,
      ref Vector2 position,
      ref Vector2 velocity,
      ref BallCollisionEvent collision);

    void OnPassThrough(
      PhysicsProperties properties,
      ref Vector2 position,
      ref Vector2 velocity,
      ref float angularVelocity,
      ref BallPassThroughEvent passThrough);
  }
}
