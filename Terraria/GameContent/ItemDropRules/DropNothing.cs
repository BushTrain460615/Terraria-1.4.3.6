// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemDropRules.DropNothing
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
  public class DropNothing : IItemDropRule
  {
    public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

    public DropNothing() => this.ChainedRules = new List<IItemDropRuleChainAttempt>();

    public bool CanDrop(DropAttemptInfo info) => false;

    public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info) => new ItemDropAttemptResult()
    {
      State = ItemDropAttemptResultState.DoesntFillConditions
    };

    public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo) => Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
  }
}
