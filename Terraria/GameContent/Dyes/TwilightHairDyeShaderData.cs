// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Dyes.TwilightHairDyeShaderData
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
  public class TwilightHairDyeShaderData : HairShaderData
  {
    public TwilightHairDyeShaderData(Ref<Effect> shader, string passName)
      : base(shader, passName)
    {
    }

    public override void Apply(Player player, DrawData? drawData = null)
    {
      if (drawData.HasValue)
        this.UseTargetPosition(Main.screenPosition + drawData.Value.position);
      base.Apply(player, drawData);
    }
  }
}
