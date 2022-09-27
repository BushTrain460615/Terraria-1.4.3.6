// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes.LuckyCoinConditionSetter
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes
{
  public class LuckyCoinConditionSetter : ISimulationConditionSetter
  {
    private int _timesToRun;

    public LuckyCoinConditionSetter(int timesToRunMultiplier) => this._timesToRun = timesToRunMultiplier;

    public int GetTimesToRunMultiplier(SimulatorInfo info)
    {
      switch (info.npcVictim.netID)
      {
        case 216:
        case 491:
          return this._timesToRun;
        default:
          return 0;
      }
    }

    public void Setup(SimulatorInfo info)
    {
    }

    public void TearDown(SimulatorInfo info)
    {
    }
  }
}
