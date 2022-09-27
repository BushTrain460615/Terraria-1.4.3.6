// Decompiled with JetBrains decompiler
// Type: Terraria.Initializers.NetworkInitializer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.Initializers
{
  public static class NetworkInitializer
  {
    public static void Load()
    {
      NetManager.Instance.Register<NetLiquidModule>();
      NetManager.Instance.Register<NetTextModule>();
      NetManager.Instance.Register<NetPingModule>();
      NetManager.Instance.Register<NetAmbienceModule>();
      NetManager.Instance.Register<NetBestiaryModule>();
      NetManager.Instance.Register<NetCreativeUnlocksModule>();
      NetManager.Instance.Register<NetCreativePowersModule>();
      NetManager.Instance.Register<NetCreativeUnlocksPlayerReportModule>();
      NetManager.Instance.Register<NetTeleportPylonModule>();
      NetManager.Instance.Register<NetParticlesModule>();
      NetManager.Instance.Register<NetCreativePowerPermissionsModule>();
    }
  }
}
