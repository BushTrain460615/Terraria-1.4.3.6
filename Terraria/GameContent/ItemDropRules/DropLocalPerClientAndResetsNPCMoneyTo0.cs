// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.ItemDropRules.DropLocalPerClientAndResetsNPCMoneyTo0
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.ItemDropRules
{
  public class DropLocalPerClientAndResetsNPCMoneyTo0 : CommonDrop
  {
    public IItemDropRuleCondition condition;

    public DropLocalPerClientAndResetsNPCMoneyTo0(
      int itemId,
      int chanceDenominator,
      int amountDroppedMinimum,
      int amountDroppedMaximum,
      IItemDropRuleCondition optionalCondition)
      : base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum)
    {
      this.condition = optionalCondition;
    }

    public override bool CanDrop(DropAttemptInfo info) => this.condition == null || this.condition.CanDrop(info);

    public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
    {
      if (info.rng.Next(this.chanceDenominator) < this.chanceNumerator)
      {
        CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0(info.npc, this.itemId, info.rng.Next(this.amountDroppedMinimum, this.amountDroppedMaximum + 1));
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
  }
}
