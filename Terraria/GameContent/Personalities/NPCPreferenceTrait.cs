// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Personalities.NPCPreferenceTrait
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.Personalities
{
  public class NPCPreferenceTrait : IShopPersonalityTrait
  {
    public AffectionLevel Level;
    public int NpcId;

    public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
    {
      if (!info.nearbyNPCsByType[this.NpcId])
        return;
      switch (this.Level)
      {
        case AffectionLevel.Hate:
          shopHelperInstance.HateNPC(this.NpcId);
          break;
        case AffectionLevel.Dislike:
          shopHelperInstance.DislikeNPC(this.NpcId);
          break;
        case AffectionLevel.Like:
          shopHelperInstance.LikeNPC(this.NpcId);
          break;
        case AffectionLevel.Love:
          shopHelperInstance.LoveNPC(this.NpcId);
          break;
      }
    }
  }
}
