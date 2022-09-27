// Decompiled with JetBrains decompiler
// Type: Terraria.Modules.TileObjectDrawModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Modules
{
  public class TileObjectDrawModule
  {
    public int xOffset;
    public int yOffset;
    public bool flipHorizontal;
    public bool flipVertical;
    public int stepDown;

    public TileObjectDrawModule(TileObjectDrawModule copyFrom = null)
    {
      if (copyFrom == null)
      {
        this.xOffset = 0;
        this.yOffset = 0;
        this.flipHorizontal = false;
        this.flipVertical = false;
        this.stepDown = 0;
      }
      else
      {
        this.xOffset = copyFrom.xOffset;
        this.yOffset = copyFrom.yOffset;
        this.flipHorizontal = copyFrom.flipHorizontal;
        this.flipVertical = copyFrom.flipVertical;
        this.stepDown = copyFrom.stepDown;
      }
    }
  }
}
