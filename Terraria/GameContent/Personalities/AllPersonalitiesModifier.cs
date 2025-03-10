﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Personalities.AllPersonalitiesModifier
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
  public class AllPersonalitiesModifier : IShopPersonalityTrait
  {
    public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance) => AllPersonalitiesModifier.ModifyShopPrice_Relationships(info, shopHelperInstance);

    private static void ModifyShopPrice_Relationships(
      HelperInfo info,
      ShopHelper shopHelperInstance)
    {
      bool[] nearbyNpCsByType = info.nearbyNPCsByType;
      if (info.npc.type == 663)
      {
        List<int> intList = new List<int>();
        for (int index = 0; index < nearbyNpCsByType.Length; ++index)
        {
          if (nearbyNpCsByType[index])
            intList.Add(index);
        }
        for (int index1 = 0; index1 < 3 && intList.Count > 0; ++index1)
        {
          int index2 = Main.rand.Next(intList.Count);
          int npcType = intList[index2];
          intList.RemoveAt(index2);
          shopHelperInstance.LoveNPCByTypeName(npcType);
        }
      }
      if (info.npc.type != 663 && nearbyNpCsByType[663])
        shopHelperInstance.LikePrincess();
      switch (info.npc.type)
      {
        case 17:
          if (nearbyNpCsByType[588])
            shopHelperInstance.LikeNPC(588);
          if (nearbyNpCsByType[18])
            shopHelperInstance.LikeNPC(18);
          if (nearbyNpCsByType[441])
            shopHelperInstance.DislikeNPC(441);
          if (!nearbyNpCsByType[369])
            break;
          shopHelperInstance.HateNPC(369);
          break;
        case 18:
          if (nearbyNpCsByType[19])
            shopHelperInstance.LoveNPC(19);
          if (nearbyNpCsByType[108])
            shopHelperInstance.LikeNPC(108);
          if (nearbyNpCsByType[208])
            shopHelperInstance.DislikeNPC(208);
          if (nearbyNpCsByType[20])
            shopHelperInstance.DislikeNPC(20);
          if (!nearbyNpCsByType[633])
            break;
          shopHelperInstance.HateNPC(633);
          break;
        case 19:
          if (nearbyNpCsByType[18])
            shopHelperInstance.LoveNPC(18);
          if (nearbyNpCsByType[178])
            shopHelperInstance.LikeNPC(178);
          if (nearbyNpCsByType[588])
            shopHelperInstance.DislikeNPC(588);
          if (!nearbyNpCsByType[38])
            break;
          shopHelperInstance.HateNPC(38);
          break;
        case 20:
          if (nearbyNpCsByType[228])
            shopHelperInstance.LikeNPC(228);
          if (nearbyNpCsByType[160])
            shopHelperInstance.LikeNPC(160);
          if (nearbyNpCsByType[369])
            shopHelperInstance.DislikeNPC(369);
          if (!nearbyNpCsByType[588])
            break;
          shopHelperInstance.HateNPC(588);
          break;
        case 22:
          if (nearbyNpCsByType[54])
            shopHelperInstance.LikeNPC(54);
          if (nearbyNpCsByType[178])
            shopHelperInstance.DislikeNPC(178);
          if (nearbyNpCsByType[227])
            shopHelperInstance.HateNPC(227);
          if (!nearbyNpCsByType[633])
            break;
          shopHelperInstance.LikeNPC(633);
          break;
        case 38:
          if (nearbyNpCsByType[550])
            shopHelperInstance.LoveNPC(550);
          if (nearbyNpCsByType[124])
            shopHelperInstance.LikeNPC(124);
          if (nearbyNpCsByType[107])
            shopHelperInstance.DislikeNPC(107);
          if (!nearbyNpCsByType[19])
            break;
          shopHelperInstance.DislikeNPC(19);
          break;
        case 54:
          if (nearbyNpCsByType[160])
            shopHelperInstance.LoveNPC(160);
          if (nearbyNpCsByType[441])
            shopHelperInstance.LikeNPC(441);
          if (nearbyNpCsByType[18])
            shopHelperInstance.DislikeNPC(18);
          if (!nearbyNpCsByType[124])
            break;
          shopHelperInstance.HateNPC(124);
          break;
        case 107:
          if (nearbyNpCsByType[124])
            shopHelperInstance.LoveNPC(124);
          if (nearbyNpCsByType[207])
            shopHelperInstance.LikeNPC(207);
          if (nearbyNpCsByType[54])
            shopHelperInstance.DislikeNPC(54);
          if (!nearbyNpCsByType[353])
            break;
          shopHelperInstance.HateNPC(353);
          break;
        case 108:
          if (nearbyNpCsByType[588])
            shopHelperInstance.LoveNPC(588);
          if (nearbyNpCsByType[17])
            shopHelperInstance.LikeNPC(17);
          if (nearbyNpCsByType[228])
            shopHelperInstance.DislikeNPC(228);
          if (!nearbyNpCsByType[209])
            break;
          shopHelperInstance.HateNPC(209);
          break;
        case 124:
          if (nearbyNpCsByType[107])
            shopHelperInstance.LoveNPC(107);
          if (nearbyNpCsByType[209])
            shopHelperInstance.LikeNPC(209);
          if (nearbyNpCsByType[19])
            shopHelperInstance.DislikeNPC(19);
          if (!nearbyNpCsByType[54])
            break;
          shopHelperInstance.HateNPC(54);
          break;
        case 142:
          if (!nearbyNpCsByType[441])
            break;
          shopHelperInstance.HateNPC(441);
          break;
        case 160:
          if (nearbyNpCsByType[22])
            shopHelperInstance.LoveNPC(22);
          if (nearbyNpCsByType[20])
            shopHelperInstance.LikeNPC(20);
          if (nearbyNpCsByType[54])
            shopHelperInstance.DislikeNPC(54);
          if (!nearbyNpCsByType[228])
            break;
          shopHelperInstance.HateNPC(228);
          break;
        case 178:
          if (nearbyNpCsByType[209])
            shopHelperInstance.LoveNPC(209);
          if (nearbyNpCsByType[227])
            shopHelperInstance.LikeNPC(227);
          if (nearbyNpCsByType[208])
            shopHelperInstance.DislikeNPC(208);
          if (nearbyNpCsByType[108])
            shopHelperInstance.DislikeNPC(108);
          if (!nearbyNpCsByType[20])
            break;
          shopHelperInstance.DislikeNPC(20);
          break;
        case 207:
          if (nearbyNpCsByType[19])
            shopHelperInstance.LikeNPC(19);
          if (nearbyNpCsByType[227])
            shopHelperInstance.LikeNPC(227);
          if (nearbyNpCsByType[178])
            shopHelperInstance.DislikeNPC(178);
          if (!nearbyNpCsByType[229])
            break;
          shopHelperInstance.HateNPC(229);
          break;
        case 208:
          if (nearbyNpCsByType[108])
            shopHelperInstance.LoveNPC(108);
          if (nearbyNpCsByType[353])
            shopHelperInstance.LikeNPC(353);
          if (nearbyNpCsByType[17])
            shopHelperInstance.DislikeNPC(17);
          if (nearbyNpCsByType[441])
            shopHelperInstance.HateNPC(441);
          if (!nearbyNpCsByType[633])
            break;
          shopHelperInstance.LoveNPC(633);
          break;
        case 209:
          if (nearbyNpCsByType[353])
            shopHelperInstance.LikeNPC(353);
          if (nearbyNpCsByType[229])
            shopHelperInstance.LikeNPC(229);
          if (nearbyNpCsByType[178])
            shopHelperInstance.LikeNPC(178);
          if (nearbyNpCsByType[108])
            shopHelperInstance.HateNPC(108);
          if (!nearbyNpCsByType[633])
            break;
          shopHelperInstance.DislikeNPC(633);
          break;
        case 227:
          if (nearbyNpCsByType[20])
            shopHelperInstance.LoveNPC(20);
          if (nearbyNpCsByType[208])
            shopHelperInstance.LikeNPC(208);
          if (nearbyNpCsByType[209])
            shopHelperInstance.DislikeNPC(209);
          if (!nearbyNpCsByType[160])
            break;
          shopHelperInstance.DislikeNPC(160);
          break;
        case 228:
          if (nearbyNpCsByType[20])
            shopHelperInstance.LikeNPC(20);
          if (nearbyNpCsByType[22])
            shopHelperInstance.LikeNPC(22);
          if (nearbyNpCsByType[18])
            shopHelperInstance.DislikeNPC(18);
          if (!nearbyNpCsByType[160])
            break;
          shopHelperInstance.HateNPC(160);
          break;
        case 229:
          if (nearbyNpCsByType[369])
            shopHelperInstance.LoveNPC(369);
          if (nearbyNpCsByType[550])
            shopHelperInstance.LikeNPC(550);
          if (nearbyNpCsByType[353])
            shopHelperInstance.DislikeNPC(353);
          if (!nearbyNpCsByType[22])
            break;
          shopHelperInstance.HateNPC(22);
          break;
        case 353:
          if (nearbyNpCsByType[207])
            shopHelperInstance.LoveNPC(207);
          if (nearbyNpCsByType[229])
            shopHelperInstance.LikeNPC(229);
          if (nearbyNpCsByType[550])
            shopHelperInstance.DislikeNPC(550);
          if (!nearbyNpCsByType[107])
            break;
          shopHelperInstance.HateNPC(107);
          break;
        case 369:
          if (nearbyNpCsByType[208])
            shopHelperInstance.LikeNPC(208);
          if (nearbyNpCsByType[38])
            shopHelperInstance.LikeNPC(38);
          if (nearbyNpCsByType[441])
            shopHelperInstance.LikeNPC(441);
          if (!nearbyNpCsByType[550])
            break;
          shopHelperInstance.HateNPC(550);
          break;
        case 441:
          if (nearbyNpCsByType[17])
            shopHelperInstance.LoveNPC(17);
          if (nearbyNpCsByType[208])
            shopHelperInstance.LikeNPC(208);
          if (nearbyNpCsByType[38])
            shopHelperInstance.DislikeNPC(38);
          if (nearbyNpCsByType[124])
            shopHelperInstance.DislikeNPC(124);
          if (!nearbyNpCsByType[142])
            break;
          shopHelperInstance.HateNPC(142);
          break;
        case 550:
          if (nearbyNpCsByType[38])
            shopHelperInstance.LoveNPC(38);
          if (nearbyNpCsByType[107])
            shopHelperInstance.LikeNPC(107);
          if (nearbyNpCsByType[22])
            shopHelperInstance.DislikeNPC(22);
          if (!nearbyNpCsByType[207])
            break;
          shopHelperInstance.HateNPC(207);
          break;
        case 588:
          if (nearbyNpCsByType[227])
            shopHelperInstance.LikeNPC(227);
          if (nearbyNpCsByType[369])
            shopHelperInstance.LoveNPC(369);
          if (nearbyNpCsByType[17])
            shopHelperInstance.HateNPC(17);
          if (nearbyNpCsByType[229])
            shopHelperInstance.DislikeNPC(229);
          if (!nearbyNpCsByType[633])
            break;
          shopHelperInstance.LikeNPC(633);
          break;
        case 633:
          if (nearbyNpCsByType[369])
            shopHelperInstance.DislikeNPC(369);
          if (nearbyNpCsByType[19])
            shopHelperInstance.HateNPC(19);
          if (nearbyNpCsByType[228])
            shopHelperInstance.LoveNPC(228);
          if (!nearbyNpCsByType[588])
            break;
          shopHelperInstance.LikeNPC(588);
          break;
      }
    }
  }
}
