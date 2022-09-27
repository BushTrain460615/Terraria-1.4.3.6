// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemDropRules.OneFromOptionsNotScaledWithLuckDropRule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
  public class OneFromOptionsNotScaledWithLuckDropRule : IItemDropRule
  {
    public int[] dropIds;
    public int chanceDenominator;
    public int chanceNumerator;

    public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

    public OneFromOptionsNotScaledWithLuckDropRule(
      int chanceDenominator,
      int chanceNumerator,
      params int[] options)
    {
      this.chanceDenominator = chanceDenominator;
      this.dropIds = options;
      this.chanceNumerator = chanceNumerator;
      this.ChainedRules = new List<IItemDropRuleChainAttempt>();
    }

    public bool CanDrop(DropAttemptInfo info) => true;

    public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
    {
      if (info.rng.Next(this.chanceDenominator) < this.chanceNumerator)
      {
        CommonCode.DropItemFromNPC(info.npc, this.dropIds[info.rng.Next(this.dropIds.Length)], 1);
        return new ItemDropAttemptResult()
        {
          State = ItemDropAttemptResultState.Success
        };
      }
      return new ItemDropAttemptResult()
      {
        State = ItemDropAttemptResultState.FailedRandomRoll
      };
    }

    public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
    {
      float personalDropRate = (float) this.chanceNumerator / (float) this.chanceDenominator;
      float dropRate = 1f / (float) this.dropIds.Length * (personalDropRate * ratesInfo.parentDroprateChance);
      for (int index = 0; index < this.dropIds.Length; ++index)
        drops.Add(new DropRateInfo(this.dropIds[index], 1, 1, dropRate, ratesInfo.conditions));
      Chains.ReportDroprates(this.ChainedRules, personalDropRate, drops, ratesInfo);
    }
  }
}
