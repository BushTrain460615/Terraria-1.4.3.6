// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.Desert.DesertDescription
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Biomes.Desert
{
  public class DesertDescription
  {
    public static readonly DesertDescription Invalid = new DesertDescription()
    {
      IsValid = false
    };
    private static readonly Vector2 DefaultBlockScale = new Vector2(4f, 2f);
    private const int SCAN_PADDING = 5;

    public Rectangle CombinedArea { get; private set; }

    public Rectangle Desert { get; private set; }

    public Rectangle Hive { get; private set; }

    public Vector2 BlockScale { get; private set; }

    public int BlockColumnCount { get; private set; }

    public int BlockRowCount { get; private set; }

    public bool IsValid { get; private set; }

    public SurfaceMap Surface { get; private set; }

    private DesertDescription()
    {
    }

    public void UpdateSurfaceMap() => this.Surface = SurfaceMap.FromArea(this.CombinedArea.Left - 5, this.CombinedArea.Width + 10);

    public static DesertDescription CreateFromPlacement(Point origin)
    {
      Vector2 defaultBlockScale = DesertDescription.DefaultBlockScale;
      float num1 = (float) Main.maxTilesX / 4200f;
      int num2 = (int) (80.0 * (double) num1);
      int num3 = (int) (((double) WorldGen.genRand.NextFloat() + 1.0) * 170.0 * (double) num1);
      int width = (int) ((double) defaultBlockScale.X * (double) num2);
      int num4 = (int) ((double) defaultBlockScale.Y * (double) num3);
      origin.X -= width / 2;
      SurfaceMap surfaceMap = SurfaceMap.FromArea(origin.X - 5, width + 10);
      if (DesertDescription.RowHasInvalidTiles(origin.X, surfaceMap.Bottom, width))
        return DesertDescription.Invalid;
      int y = (int) ((double) surfaceMap.Average + (double) surfaceMap.Bottom) / 2;
      origin.Y = y + WorldGen.genRand.Next(40, 60);
      int num5 = 0;
      if (Main.tenthAnniversaryWorld)
        num5 = (int) (20.0 * (double) num1);
      return new DesertDescription()
      {
        CombinedArea = new Rectangle(origin.X, y, width, origin.Y + num4 - y),
        Hive = new Rectangle(origin.X, origin.Y + num5, width, num4 - num5),
        Desert = new Rectangle(origin.X, y, width, origin.Y + num4 / 2 - y + num5),
        BlockScale = defaultBlockScale,
        BlockColumnCount = num2,
        BlockRowCount = num3,
        Surface = surfaceMap,
        IsValid = true
      };
    }

    private static bool RowHasInvalidTiles(int startX, int startY, int width)
    {
      if (WorldGen.skipDesertTileCheck)
        return false;
      for (int index = startX; index < startX + width; ++index)
      {
        switch (Main.tile[index, startY].type)
        {
          case 59:
          case 60:
            return true;
          case 147:
          case 161:
            return true;
          default:
            continue;
        }
      }
      return false;
    }
  }
}
