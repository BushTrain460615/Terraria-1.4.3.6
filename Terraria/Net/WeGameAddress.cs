// Decompiled with JetBrains decompiler
// Type: Terraria.Net.WeGameAddress
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using rail;

namespace Terraria.Net
{
  public class WeGameAddress : RemoteAddress
  {
    public readonly RailID rail_id;
    private string nickname;

    public WeGameAddress(RailID id, string name)
    {
      this.Type = AddressType.WeGame;
      this.rail_id = id;
      this.nickname = name;
    }

    public override string ToString() => "WEGAME_0:" + ((RailComparableID) this.rail_id).id_.ToString();

    public override string GetIdentifier() => this.ToString();

    public override bool IsLocalHost() => Program.LaunchParameters.ContainsKey("-localwegameid") && Program.LaunchParameters["-localwegameid"].Equals(((RailComparableID) this.rail_id).id_.ToString());

    public override string GetFriendlyName() => this.nickname;
  }
}
