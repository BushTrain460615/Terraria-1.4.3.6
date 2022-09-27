// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.PortableStoolUsage
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public struct PortableStoolUsage
  {
    public bool HasAStool;
    public bool IsInUse;
    public int HeightBoost;
    public int VisualYOffset;
    public int MapYOffset;

    public void Reset()
    {
      this.HasAStool = false;
      this.IsInUse = false;
      this.HeightBoost = 0;
      this.VisualYOffset = 0;
      this.MapYOffset = 0;
    }

    public void SetStats(int heightBoost, int visualYOffset, int mapYOffset)
    {
      this.HasAStool = true;
      this.HeightBoost = heightBoost;
      this.VisualYOffset = visualYOffset;
      this.MapYOffset = mapYOffset;
    }
  }
}
