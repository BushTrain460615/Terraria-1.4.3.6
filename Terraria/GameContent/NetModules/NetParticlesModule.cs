// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.NetModules.NetParticlesModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.IO;
using Terraria.GameContent.Drawing;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
  public class NetParticlesModule : NetModule
  {
    public static NetPacket Serialize(
      ParticleOrchestraType particleType,
      ParticleOrchestraSettings settings)
    {
      NetPacket packet = NetModule.CreatePacket<NetParticlesModule>(22);
      packet.Writer.Write((byte) particleType);
      settings.Serialize(packet.Writer);
      return packet;
    }

    public override bool Deserialize(BinaryReader reader, int userId)
    {
      ParticleOrchestraType particleOrchestraType = (ParticleOrchestraType) reader.ReadByte();
      ParticleOrchestraSettings settings = new ParticleOrchestraSettings();
      settings.DeserializeFrom(reader);
      if (Main.netMode == 2)
        NetManager.Instance.Broadcast(NetParticlesModule.Serialize(particleOrchestraType, settings), userId);
      else
        ParticleOrchestrator.SpawnParticlesDirect(particleOrchestraType, settings);
      return true;
    }
  }
}
