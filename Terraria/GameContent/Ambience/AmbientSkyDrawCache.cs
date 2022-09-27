// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Ambience.AmbientSkyDrawCache
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.GameContent.Ambience
{
  public class AmbientSkyDrawCache
  {
    public static AmbientSkyDrawCache Instance = new AmbientSkyDrawCache();
    public AmbientSkyDrawCache.UnderworldCache[] Underworld = new AmbientSkyDrawCache.UnderworldCache[5];
    public AmbientSkyDrawCache.OceanLineCache OceanLineInfo;

    public void SetUnderworldInfo(int drawIndex, float scale) => this.Underworld[drawIndex] = new AmbientSkyDrawCache.UnderworldCache()
    {
      Scale = scale
    };

    public void SetOceanLineInfo(float yScreenPosition, float oceanOpacity) => this.OceanLineInfo = new AmbientSkyDrawCache.OceanLineCache()
    {
      YScreenPosition = yScreenPosition,
      OceanOpacity = oceanOpacity
    };

    public struct UnderworldCache
    {
      public float Scale;
    }

    public struct OceanLineCache
    {
      public float YScreenPosition;
      public float OceanOpacity;
    }
  }
}
