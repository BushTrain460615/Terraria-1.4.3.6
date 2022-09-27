// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.IPersistentPerPlayerContent
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.IO;

namespace Terraria.GameContent
{
  public interface IPersistentPerPlayerContent
  {
    void Save(Player player, BinaryWriter writer);

    void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn);

    void ApplyLoadedDataToOutOfPlayerFields(Player player);

    void ResetDataForNewPlayer(Player player);

    void Reset();
  }
}
