// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemDropRules.IItemDropRuleChainAttempt
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
  public interface IItemDropRuleChainAttempt
  {
    IItemDropRule RuleToChain { get; }

    bool CanChainIntoRule(ItemDropAttemptResult parentResult);

    void ReportDroprates(
      float personalDropRate,
      List<DropRateInfo> drops,
      DropRateInfoChainFeed ratesInfo);
  }
}
