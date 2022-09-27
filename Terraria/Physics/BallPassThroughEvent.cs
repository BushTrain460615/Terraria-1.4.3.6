// Decompiled with JetBrains decompiler
// Type: Terraria.Physics.BallPassThroughEvent
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Physics
{
  public struct BallPassThroughEvent
  {
    public readonly Tile Tile;
    public readonly Entity Entity;
    public readonly BallPassThroughType Type;
    public readonly float TimeScale;

    public BallPassThroughEvent(
      float timeScale,
      Tile tile,
      Entity entity,
      BallPassThroughType type)
    {
      this.Tile = tile;
      this.Entity = entity;
      this.Type = type;
      this.TimeScale = timeScale;
    }
  }
}
