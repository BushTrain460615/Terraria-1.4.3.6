// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes.StackedConditionSetter
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
  public class StackedConditionSetter : ISimulationConditionSetter
  {
    private ISimulationConditionSetter[] _setters;

    public StackedConditionSetter(params ISimulationConditionSetter[] setters) => this._setters = setters;

    public void Setup(SimulatorInfo info)
    {
      for (int index = 0; index < this._setters.Length; ++index)
        this._setters[index].Setup(info);
    }

    public void TearDown(SimulatorInfo info)
    {
      for (int index = 0; index < this._setters.Length; ++index)
        this._setters[index].TearDown(info);
    }

    public int GetTimesToRunMultiplier(SimulatorInfo info) => 1;
  }
}
