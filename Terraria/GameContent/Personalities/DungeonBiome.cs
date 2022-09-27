// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Personalities.DungeonBiome
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.Personalities
{
  public class DungeonBiome : AShoppingBiome
  {
    public DungeonBiome() => this.NameKey = "Dungeon";

    public override bool IsInBiome(Player player) => player.ZoneDungeon;
  }
}
