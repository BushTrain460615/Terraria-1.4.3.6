// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.NetModules.NetCreativeUnlocksPlayerReportModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.IO;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
  public class NetCreativeUnlocksPlayerReportModule : NetModule
  {
    private const byte _requestItemSacrificeId = 0;

    public static NetPacket SerializeSacrificeRequest(int itemId, int amount)
    {
      NetPacket packet = NetModule.CreatePacket<NetCreativeUnlocksPlayerReportModule>(5);
      packet.Writer.Write((byte) 0);
      packet.Writer.Write((ushort) itemId);
      packet.Writer.Write((ushort) amount);
      return packet;
    }

    public override bool Deserialize(BinaryReader reader, int userId)
    {
      if (reader.ReadByte() == (byte) 0)
      {
        int num1 = (int) reader.ReadUInt16();
        int num2 = (int) reader.ReadUInt16();
      }
      return true;
    }
  }
}
