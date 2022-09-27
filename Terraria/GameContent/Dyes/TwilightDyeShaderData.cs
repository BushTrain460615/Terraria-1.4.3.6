// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Dyes.TwilightDyeShaderData
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
  public class TwilightDyeShaderData : ArmorShaderData
  {
    public TwilightDyeShaderData(Ref<Effect> shader, string passName)
      : base(shader, passName)
    {
    }

    public override void Apply(Entity entity, DrawData? drawData)
    {
      if (drawData.HasValue)
      {
        switch (entity)
        {
          case Player player when !player.isDisplayDollOrInanimate && !player.isHatRackDoll:
            this.UseTargetPosition(Main.screenPosition + drawData.Value.position);
            break;
          case Projectile _:
            this.UseTargetPosition(Main.screenPosition + drawData.Value.position);
            break;
          default:
            this.UseTargetPosition(drawData.Value.position);
            break;
        }
      }
      base.Apply(entity, drawData);
    }
  }
}
