// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.NetModules.NetCreativeUnlocksModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.IO;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
  public class NetCreativeUnlocksModule : NetModule
  {
    public static NetPacket SerializeItemSacrifice(int itemId, int sacrificeCount)
    {
      NetPacket packet = NetModule.CreatePacket<NetCreativeUnlocksModule>(3);
      packet.Writer.Write((short) itemId);
      packet.Writer.Write((ushort) sacrificeCount);
      return packet;
    }

    public override bool Deserialize(BinaryReader reader, int userId)
    {
      short key = reader.ReadInt16();
      Main.LocalPlayerCreativeTracker.ItemSacrifices.SetSacrificeCountDirectly(ContentSamples.ItemPersistentIdsByNetIds[(int) key], (int) reader.ReadUInt16());
      return true;
    }
  }
}
