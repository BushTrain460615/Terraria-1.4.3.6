// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes.FastConditionSetter
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
  public class FastConditionSetter : ISimulationConditionSetter
  {
    private Action<SimulatorInfo> _setup;
    private Action<SimulatorInfo> _tearDown;

    public FastConditionSetter(Action<SimulatorInfo> setup, Action<SimulatorInfo> tearDown)
    {
      this._setup = setup;
      this._tearDown = tearDown;
    }

    public void Setup(SimulatorInfo info)
    {
      if (this._setup == null)
        return;
      this._setup(info);
    }

    public void TearDown(SimulatorInfo info)
    {
      if (this._tearDown == null)
        return;
      this._tearDown(info);
    }

    public int GetTimesToRunMultiplier(SimulatorInfo info) => 1;
  }
}
