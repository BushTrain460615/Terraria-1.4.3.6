// Decompiled with JetBrains decompiler
// Type: Terraria.Physics.BallStepResult
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Physics
{
  public struct BallStepResult
  {
    public readonly BallState State;

    private BallStepResult(BallState state) => this.State = state;

    public static BallStepResult OutOfBounds() => new BallStepResult(BallState.OutOfBounds);

    public static BallStepResult Moving() => new BallStepResult(BallState.Moving);

    public static BallStepResult Resting() => new BallStepResult(BallState.Resting);
  }
}
