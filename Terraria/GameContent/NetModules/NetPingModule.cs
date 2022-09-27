// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.NetModules.NetPingModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.IO;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
  public class NetPingModule : NetModule
  {
    public static NetPacket Serialize(Vector2 position)
    {
      NetPacket packet = NetModule.CreatePacket<NetPingModule>(8);
      packet.Writer.WriteVector2(position);
      return packet;
    }

    public override bool Deserialize(BinaryReader reader, int userId)
    {
      Vector2 position = reader.ReadVector2();
      Main.Pings.Add(position);
      return true;
    }
  }
}
