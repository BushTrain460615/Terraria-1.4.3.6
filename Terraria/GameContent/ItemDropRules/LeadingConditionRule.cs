// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemDropRules.LeadingConditionRule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
  public class LeadingConditionRule : IItemDropRule
  {
    public IItemDropRuleCondition condition;

    public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

    public LeadingConditionRule(IItemDropRuleCondition condition)
    {
      this.condition = condition;
      this.ChainedRules = new List<IItemDropRuleChainAttempt>();
    }

    public bool CanDrop(DropAttemptInfo info) => this.condition.CanDrop(info);

    public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
    {
      ratesInfo.AddCondition(this.condition);
      Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
    }

    public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info) => new ItemDropAttemptResult()
    {
      State = ItemDropAttemptResultState.Success
    };
  }
}
