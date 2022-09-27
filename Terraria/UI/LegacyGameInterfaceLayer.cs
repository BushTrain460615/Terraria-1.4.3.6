// Decompiled with JetBrains decompiler
// Type: Terraria.UI.LegacyGameInterfaceLayer
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.UI
{
  public class LegacyGameInterfaceLayer : GameInterfaceLayer
  {
    private GameInterfaceDrawMethod _drawMethod;

    public LegacyGameInterfaceLayer(
      string name,
      GameInterfaceDrawMethod drawMethod,
      InterfaceScaleType scaleType = InterfaceScaleType.Game)
      : base(name, scaleType)
    {
      this._drawMethod = drawMethod;
    }

    protected override bool DrawSelf() => this._drawMethod();
  }
}
