﻿// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Creative.CreativeUnlocksTracker
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.IO;

namespace Terraria.GameContent.Creative
{
  public class CreativeUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
  {
    public ItemsSacrificedUnlocksTracker ItemSacrifices = new ItemsSacrificedUnlocksTracker();

    public void Save(BinaryWriter writer) => this.ItemSacrifices.Save(writer);

    public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn) => this.ItemSacrifices.Load(reader, gameVersionSaveWasMadeOn);

    public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn) => this.ValidateWorld(reader, gameVersionSaveWasMadeOn);

    public void Reset() => this.ItemSacrifices.Reset();

    public void OnPlayerJoining(int playerIndex) => this.ItemSacrifices.OnPlayerJoining(playerIndex);
  }
}
