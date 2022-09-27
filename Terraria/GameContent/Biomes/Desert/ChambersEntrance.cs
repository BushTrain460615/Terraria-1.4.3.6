// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Biomes.Desert.ChambersEntrance
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
  public static class ChambersEntrance
  {
    public static void Place(DesertDescription description)
    {
      int num = description.Desert.Center.X + WorldGen.genRand.Next(-40, 41);
      Point position = new Point(num, (int) description.Surface[num]);
      ChambersEntrance.PlaceAt(description, position);
    }

    private static void PlaceAt(DesertDescription description, Point position)
    {
      ShapeData shapeData = new ShapeData();
      Point origin = new Point(position.X, position.Y + 2);
      WorldUtils.Gen(origin, (GenShape) new Shapes.Circle(24, 12), Actions.Chain((GenAction) new Modifiers.Blotches(), new Actions.SetTile((ushort) 53).Output(shapeData)));
      UnifiedRandom genRand = WorldGen.genRand;
      ShapeData data = new ShapeData();
      int num1 = description.Hive.Top - position.Y;
      int direction = genRand.Next(2) == 0 ? -1 : 1;
      List<ChambersEntrance.PathConnection> pathConnectionList = new List<ChambersEntrance.PathConnection>()
      {
        new ChambersEntrance.PathConnection(new Point(position.X + -direction * 26, position.Y - 8), direction)
      };
      int num2 = genRand.Next(2, 4);
      for (int index = 0; index < num2; ++index)
      {
        int y = (int) ((double) (index + 1) / (double) num2 * (double) num1) + genRand.Next(-8, 9);
        int x = direction * genRand.Next(20, 41);
        int num3 = genRand.Next(18, 29);
        WorldUtils.Gen(position, (GenShape) new Shapes.Circle(num3 / 2, 3), Actions.Chain((GenAction) new Modifiers.Offset(x, y), (GenAction) new Modifiers.Blotches(), new Actions.Clear().Output(data), (GenAction) new Actions.PlaceWall((ushort) 187)));
        pathConnectionList.Add(new ChambersEntrance.PathConnection(new Point(x + num3 / 2 * -direction + position.X, y + position.Y), -direction));
        direction *= -1;
      }
      WorldUtils.Gen(position, (GenShape) new ModShapes.OuterOutline(data), Actions.Chain((GenAction) new Modifiers.Expand(1), (GenAction) new Modifiers.OnlyTiles(new ushort[1]
      {
        (ushort) 53
      }), (GenAction) new Actions.SetTile((ushort) 397), (GenAction) new Actions.PlaceWall((ushort) 187)));
      GenShapeActionPair pair = new GenShapeActionPair((GenShape) new Shapes.Rectangle(2, 4), Actions.Chain((GenAction) new Modifiers.IsSolid(), (GenAction) new Modifiers.Blotches(), (GenAction) new Actions.Clear(), (GenAction) new Modifiers.Expand(1), (GenAction) new Actions.PlaceWall((ushort) 187), (GenAction) new Modifiers.OnlyTiles(new ushort[1]
      {
        (ushort) 53
      }), (GenAction) new Actions.SetTile((ushort) 397)));
      for (int index = 1; index < pathConnectionList.Count; ++index)
      {
        ChambersEntrance.PathConnection pathConnection1 = pathConnectionList[index - 1];
        ChambersEntrance.PathConnection pathConnection2 = pathConnectionList[index];
        float num4 = Math.Abs(pathConnection2.Position.X - pathConnection1.Position.X) * 1.5f;
        for (float amount1 = 0.0f; (double) amount1 <= 1.0; amount1 += 0.02f)
        {
          Vector2 vector2_1 = new Vector2(pathConnection1.Position.X + pathConnection1.Direction * num4 * amount1, pathConnection1.Position.Y);
          Vector2 vector2_2 = new Vector2(pathConnection2.Position.X + (float) ((double) pathConnection2.Direction * (double) num4 * (1.0 - (double) amount1)), pathConnection2.Position.Y);
          Vector2 vector2_3 = Vector2.Lerp(pathConnection1.Position, pathConnection2.Position, amount1);
          Vector2 vector2_4 = vector2_3;
          double amount2 = (double) amount1;
          WorldUtils.Gen(Vector2.Lerp(Vector2.Lerp(vector2_1, vector2_4, (float) amount2), Vector2.Lerp(vector2_3, vector2_2, amount1), amount1).ToPoint(), pair);
        }
      }
      WorldUtils.Gen(origin, (GenShape) new Shapes.Rectangle(new Microsoft.Xna.Framework.Rectangle(-29, -12, 58, 12)), Actions.Chain((GenAction) new Modifiers.NotInShape(shapeData), (GenAction) new Modifiers.Expand(1), (GenAction) new Actions.PlaceWall((ushort) 0)));
    }

    private struct PathConnection
    {
      public readonly Vector2 Position;
      public readonly float Direction;

      public PathConnection(Point position, int direction)
      {
        this.Position = new Vector2((float) position.X, (float) position.Y);
        this.Direction = (float) direction;
      }
    }
  }
}
