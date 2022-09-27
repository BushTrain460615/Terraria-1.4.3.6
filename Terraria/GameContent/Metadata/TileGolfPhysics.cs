// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Metadata.TileGolfPhysics
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Newtonsoft.Json;

namespace Terraria.GameContent.Metadata
{
  public class TileGolfPhysics
  {
    [JsonProperty]
    public float DirectImpactDampening { get; private set; }

    [JsonProperty]
    public float SideImpactDampening { get; private set; }

    [JsonProperty]
    public float ClubImpactDampening { get; private set; }

    [JsonProperty]
    public float PassThroughDampening { get; private set; }

    [JsonProperty]
    public float ImpactDampeningResistanceEfficiency { get; private set; }
  }
}
