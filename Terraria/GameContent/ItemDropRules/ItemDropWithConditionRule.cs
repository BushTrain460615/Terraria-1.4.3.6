// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemDropRules.ItemDropWithConditionRule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
  public class ItemDropWithConditionRule : CommonDrop
  {
    public IItemDropRuleCondition condition;

    public ItemDropWithConditionRule(
      int itemId,
      int chanceDenominator,
      int amountDroppedMinimum,
      int amountDroppedMaximum,
      IItemDropRuleCondition condition,
      int chanceNumerator = 1)
      : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum, chanceNumerator)
    {
      this.condition = condition;
    }

    public override bool CanDrop(DropAttemptInfo info) => this.condition.CanDrop(info);

    public override void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
    {
      DropRateInfoChainFeed ratesInfo1 = ratesInfo.With(1f);
      ratesInfo1.AddCondition(this.condition);
      float personalDropRate = (float) this.chanceNumerator / (float) this.chanceDenominator;
      float dropRate = personalDropRate * ratesInfo1.parentDroprateChance;
      drops.Add(new DropRateInfo(this.itemId, this.amountDroppedMinimum, this.amountDroppedMaximum, dropRate, ratesInfo1.conditions));
      Chains.ReportDroprates(this.ChainedRules, personalDropRate, drops, ratesInfo1);
    }
  }
}
