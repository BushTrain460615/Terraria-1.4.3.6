﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Dust
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;

namespace Terraria
{
  public class Dust
  {
    public static float dCount;
    public static int lavaBubbles;
    public static int SandStormCount;
    public int dustIndex;
    public Vector2 position;
    public Vector2 velocity;
    public float fadeIn;
    public bool noGravity;
    public float scale;
    public float rotation;
    public bool noLight;
    public bool noLightEmittence;
    public bool active;
    public int type;
    public Color color;
    public int alpha;
    public Rectangle frame;
    public ArmorShaderData shader;
    public object customData;
    public bool firstFrame;

    public static Dust NewDustPerfect(
      Vector2 Position,
      int Type,
      Vector2? Velocity = null,
      int Alpha = 0,
      Color newColor = default (Color),
      float Scale = 1f)
    {
      Dust dust = Main.dust[Dust.NewDust(Position, 0, 0, Type, Alpha: Alpha, newColor: newColor, Scale: Scale)];
      dust.position = Position;
      if (Velocity.HasValue)
        dust.velocity = Velocity.Value;
      return dust;
    }

    public static Dust NewDustDirect(
      Vector2 Position,
      int Width,
      int Height,
      int Type,
      float SpeedX = 0.0f,
      float SpeedY = 0.0f,
      int Alpha = 0,
      Color newColor = default (Color),
      float Scale = 1f)
    {
      Dust dust = Main.dust[Dust.NewDust(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale)];
      if (dust.velocity.HasNaNs())
        dust.velocity = Vector2.Zero;
      return dust;
    }

    public static int NewDust(
      Vector2 Position,
      int Width,
      int Height,
      int Type,
      float SpeedX = 0.0f,
      float SpeedY = 0.0f,
      int Alpha = 0,
      Color newColor = default (Color),
      float Scale = 1f)
    {
      if (Main.gameMenu)
        return 6000;
      if (Main.rand == null)
        Main.rand = new UnifiedRandom((int) DateTime.Now.Ticks);
      if (Main.gamePaused || WorldGen.gen || Main.netMode == 2)
        return 6000;
      int num1 = (int) (400.0 * (1.0 - (double) Dust.dCount));
      if (!new Rectangle((int) ((double) Main.screenPosition.X - (double) num1), (int) ((double) Main.screenPosition.Y - (double) num1), Main.screenWidth + num1 * 2, Main.screenHeight + num1 * 2).Intersects(new Rectangle((int) Position.X, (int) Position.Y, 10, 10)))
        return 6000;
      int num2 = 6000;
      for (int index = 0; index < 6000; ++index)
      {
        Dust dust = Main.dust[index];
        if (!dust.active)
        {
          if ((double) index > (double) Main.maxDustToDraw * 0.9)
          {
            if (Main.rand.Next(4) != 0)
              return 6000;
          }
          else if ((double) index > (double) Main.maxDustToDraw * 0.8)
          {
            if (Main.rand.Next(3) != 0)
              return 6000;
          }
          else if ((double) index > (double) Main.maxDustToDraw * 0.7)
          {
            if (Main.rand.Next(2) == 0)
              return 6000;
          }
          else if ((double) index > (double) Main.maxDustToDraw * 0.6)
          {
            if (Main.rand.Next(4) == 0)
              return 6000;
          }
          else if ((double) index > (double) Main.maxDustToDraw * 0.5)
          {
            if (Main.rand.Next(5) == 0)
              return 6000;
          }
          else
            Dust.dCount = 0.0f;
          int num3 = Width;
          int num4 = Height;
          if (num3 < 5)
            num3 = 5;
          if (num4 < 5)
            num4 = 5;
          num2 = index;
          dust.fadeIn = 0.0f;
          dust.active = true;
          dust.type = Type;
          dust.noGravity = false;
          dust.color = newColor;
          dust.alpha = Alpha;
          dust.position.X = (float) ((double) Position.X + (double) Main.rand.Next(num3 - 4) + 4.0);
          dust.position.Y = (float) ((double) Position.Y + (double) Main.rand.Next(num4 - 4) + 4.0);
          dust.velocity.X = (float) Main.rand.Next(-20, 21) * 0.1f + SpeedX;
          dust.velocity.Y = (float) Main.rand.Next(-20, 21) * 0.1f + SpeedY;
          dust.frame.X = 10 * Type;
          dust.frame.Y = 10 * Main.rand.Next(3);
          dust.shader = (ArmorShaderData) null;
          dust.customData = (object) null;
          dust.noLightEmittence = false;
          int num5 = Type;
          while (num5 >= 100)
          {
            num5 -= 100;
            dust.frame.X -= 1000;
            dust.frame.Y += 30;
          }
          dust.frame.Width = 8;
          dust.frame.Height = 8;
          dust.rotation = 0.0f;
          dust.scale = (float) (1.0 + (double) Main.rand.Next(-20, 21) * 0.00999999977648258);
          dust.scale *= Scale;
          dust.noLight = false;
          dust.firstFrame = true;
          if (dust.type == 228 || dust.type == 279 || dust.type == 269 || dust.type == 135 || dust.type == 6 || dust.type == 242 || dust.type == 75 || dust.type == 169 || dust.type == 29 || dust.type >= 59 && dust.type <= 65 || dust.type == 158 || dust.type == 293 || dust.type == 294 || dust.type == 295 || dust.type == 296 || dust.type == 297 || dust.type == 298 || dust.type == 302)
          {
            dust.velocity.Y = (float) Main.rand.Next(-10, 6) * 0.1f;
            dust.velocity.X *= 0.3f;
            dust.scale *= 0.7f;
          }
          if (dust.type == (int) sbyte.MaxValue || dust.type == 187)
          {
            dust.velocity *= 0.3f;
            dust.scale *= 0.7f;
          }
          if (dust.type == 33 || dust.type == 52 || dust.type == 266 || dust.type == 98 || dust.type == 99 || dust.type == 100 || dust.type == 101 || dust.type == 102 || dust.type == 103 || dust.type == 104 || dust.type == 105)
          {
            dust.alpha = 170;
            dust.velocity *= 0.5f;
            ++dust.velocity.Y;
          }
          if (dust.type == 41)
            dust.velocity *= 0.0f;
          if (dust.type == 80)
            dust.alpha = 50;
          if (dust.type == 34 || dust.type == 35 || dust.type == 152)
          {
            dust.velocity *= 0.1f;
            dust.velocity.Y = -0.5f;
            if (dust.type == 34 && !Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y - 8f), 4, 4))
            {
              dust.active = false;
              break;
            }
            break;
          }
          break;
        }
      }
      return num2;
    }

    public static Dust CloneDust(int dustIndex) => Dust.CloneDust(Main.dust[dustIndex]);

    public static Dust CloneDust(Dust rf)
    {
      if (rf.dustIndex == Main.maxDustToDraw)
        return rf;
      int index = Dust.NewDust(rf.position, 0, 0, rf.type);
      Dust dust = Main.dust[index];
      dust.position = rf.position;
      dust.velocity = rf.velocity;
      dust.fadeIn = rf.fadeIn;
      dust.noGravity = rf.noGravity;
      dust.scale = rf.scale;
      dust.rotation = rf.rotation;
      dust.noLight = rf.noLight;
      dust.active = rf.active;
      dust.type = rf.type;
      dust.color = rf.color;
      dust.alpha = rf.alpha;
      dust.frame = rf.frame;
      dust.shader = rf.shader;
      dust.customData = rf.customData;
      return dust;
    }

    public static Dust QuickDust(int x, int y, Color color) => Dust.QuickDust(new Point(x, y), color);

    public static Dust QuickDust(Point tileCoords, Color color) => Dust.QuickDust(tileCoords.ToWorldCoordinates(), color);

    public static void QuickBox(
      Vector2 topLeft,
      Vector2 bottomRight,
      int divisions,
      Color color,
      Action<Dust> manipulator)
    {
      float num1 = (float) (divisions + 2);
      for (float num2 = 0.0f; (double) num2 <= (double) (divisions + 2); ++num2)
      {
        Dust dust1 = Dust.QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num1), topLeft.Y), color);
        if (manipulator != null)
          manipulator(dust1);
        Dust dust2 = Dust.QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num1), bottomRight.Y), color);
        if (manipulator != null)
          manipulator(dust2);
        Dust dust3 = Dust.QuickDust(new Vector2(topLeft.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num1)), color);
        if (manipulator != null)
          manipulator(dust3);
        Dust dust4 = Dust.QuickDust(new Vector2(bottomRight.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num1)), color);
        if (manipulator != null)
          manipulator(dust4);
      }
    }

    public static Dust QuickDust(Vector2 pos, Color color)
    {
      Dust dust = Main.dust[Dust.NewDust(pos, 0, 0, 267)];
      dust.position = pos;
      dust.velocity = Vector2.Zero;
      dust.fadeIn = 1f;
      dust.noLight = true;
      dust.noGravity = true;
      dust.color = color;
      return dust;
    }

    public static Dust QuickDustSmall(Vector2 pos, Color color, bool floorPositionValues = false)
    {
      Dust dust = Dust.QuickDust(pos, color);
      dust.fadeIn = 0.0f;
      dust.scale = 0.35f;
      if (floorPositionValues)
        dust.position = dust.position.Floor();
      return dust;
    }

    public static void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color)
    {
      Dust.QuickDust(start, color).scale = 2f;
      Dust.QuickDust(end, color).scale = 2f;
      float num = 1f / splits;
      for (float amount = 0.0f; (double) amount < 1.0; amount += num)
        Dust.QuickDust(Vector2.Lerp(start, end, amount), color).scale = 2f;
    }

    public static int dustWater()
    {
      switch (Main.waterStyle)
      {
        case 2:
          return 98;
        case 3:
          return 99;
        case 4:
          return 100;
        case 5:
          return 101;
        case 6:
          return 102;
        case 7:
          return 103;
        case 8:
          return 104;
        case 9:
          return 105;
        case 10:
          return 123;
        case 12:
          return 288;
        default:
          return 33;
      }
    }

    public static void UpdateDust()
    {
      int num1 = 0;
      Dust.lavaBubbles = 0;
      Main.snowDust = 0;
      Dust.SandStormCount = 0;
      bool flag = Sandstorm.ShouldSandstormDustPersist();
      for (int index1 = 0; index1 < 6000; ++index1)
      {
        Dust dust = Main.dust[index1];
        if (index1 < Main.maxDustToDraw)
        {
          if (dust.active)
          {
            ++Dust.dCount;
            if ((double) dust.scale > 10.0)
              dust.active = false;
            if (dust.firstFrame && !ChildSafety.Disabled && ChildSafety.DangerousDust(dust.type))
            {
              if (Main.rand.Next(2) == 0)
              {
                dust.firstFrame = false;
                dust.type = 16;
                dust.scale = (float) ((double) Main.rand.NextFloat() * 1.60000002384186 + 0.300000011920929);
                dust.color = Color.Transparent;
                dust.frame.X = 10 * dust.type;
                dust.frame.Y = 10 * Main.rand.Next(3);
                dust.shader = (ArmorShaderData) null;
                dust.customData = (object) null;
                int num2 = dust.type / 100;
                dust.frame.X -= 1000 * num2;
                dust.frame.Y += 30 * num2;
                dust.noGravity = true;
              }
              else
                dust.active = false;
            }
            switch (dust.type)
            {
              case 299:
              case 300:
              case 301:
                dust.scale *= 0.96f;
                dust.velocity.Y -= 0.01f;
                break;
            }
            if (dust.type == 35)
              ++Dust.lavaBubbles;
            dust.position += dust.velocity;
            if (dust.type == 258)
            {
              dust.noGravity = true;
              dust.scale += 0.015f;
            }
            if ((dust.type >= 86 && dust.type <= 92 || dust.type == 286) && !dust.noLight && !dust.noLightEmittence)
            {
              float num3 = dust.scale * 0.6f;
              if ((double) num3 > 1.0)
                num3 = 1f;
              int num4 = dust.type - 85;
              float num5 = num3;
              float num6 = num3;
              float num7 = num3;
              switch (num4)
              {
                case 1:
                  num5 *= 0.9f;
                  num6 *= 0.0f;
                  num7 *= 0.9f;
                  break;
                case 2:
                  num5 *= 0.9f;
                  num6 *= 0.9f;
                  num7 *= 0.0f;
                  break;
                case 3:
                  num5 *= 0.0f;
                  num6 *= 0.1f;
                  num7 *= 1.3f;
                  break;
                case 4:
                  num5 *= 0.0f;
                  num6 *= 1f;
                  num7 *= 0.1f;
                  break;
                case 5:
                  num5 *= 1f;
                  num6 *= 0.1f;
                  num7 *= 0.1f;
                  break;
                case 6:
                  num5 *= 1.3f;
                  num6 *= 1.3f;
                  num7 *= 1.3f;
                  break;
              }
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num3 * num5, num3 * num6, num3 * num7);
            }
            if (dust.type >= 86 && dust.type <= 92 || dust.type == 286)
            {
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
              else if (dust.customData != null && dust.customData is Projectile)
              {
                Projectile customData = (Projectile) dust.customData;
                if (customData.active)
                  dust.position += customData.position - customData.oldPosition;
              }
            }
            if (dust.type == 262 && !dust.noLight)
            {
              Vector3 rgb = new Vector3(0.9f, 0.6f, 0.0f) * dust.scale * 0.6f;
              Lighting.AddLight(dust.position, rgb);
            }
            if (dust.type == 240 && dust.customData != null && dust.customData is Projectile)
            {
              Projectile customData = (Projectile) dust.customData;
              if (customData.active)
                dust.position += customData.position - customData.oldPosition;
            }
            if ((dust.type == 259 || dust.type == 6 || dust.type == 158) && dust.customData != null && dust.customData is int)
            {
              if ((int) dust.customData == 0)
              {
                if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && (double) dust.fadeIn == 0.0)
                {
                  dust.scale *= 0.9f;
                  dust.velocity *= 0.25f;
                }
              }
              else if ((int) dust.customData == 1)
              {
                dust.scale *= 0.98f;
                dust.velocity.Y *= 0.98f;
                if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && (double) dust.fadeIn == 0.0)
                {
                  dust.scale *= 0.9f;
                  dust.velocity *= 0.25f;
                }
              }
            }
            if (dust.type == 263 || dust.type == 264)
            {
              if (!dust.noLight)
              {
                Vector3 rgb = dust.color.ToVector3() * dust.scale * 0.4f;
                Lighting.AddLight(dust.position, rgb);
              }
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
                dust.customData = (object) null;
              }
              else if (dust.customData != null && dust.customData is Projectile)
              {
                Projectile customData = (Projectile) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
            }
            if (dust.type == 230)
            {
              float num8 = dust.scale * 0.6f;
              float num9 = num8;
              float num10 = num8;
              float num11 = num8;
              float num12 = num9 * 0.5f;
              float num13 = num10 * 0.9f;
              float num14 = num11 * 1f;
              dust.scale += 0.02f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num8 * num12, num8 * num13, num8 * num14);
              if (dust.customData != null && dust.customData is Player)
              {
                Vector2 center = ((Entity) dust.customData).Center;
                Vector2 vector2_1 = dust.position - center;
                float val2 = vector2_1.Length();
                Vector2 vector2_2 = vector2_1 / val2;
                dust.scale = Math.Min(dust.scale, (float) ((double) val2 / 24.0 - 1.0));
                dust.velocity -= vector2_2 * (100f / Math.Max(50f, val2));
              }
            }
            if (dust.type == 154 || dust.type == 218)
            {
              dust.rotation += dust.velocity.X * 0.3f;
              dust.scale -= 0.03f;
            }
            if (dust.type == 172)
            {
              float num15 = dust.scale * 0.5f;
              if ((double) num15 > 1.0)
                num15 = 1f;
              float num16 = num15;
              float num17 = num15;
              float num18 = num15;
              float num19 = num16 * 0.0f;
              float num20 = num17 * 0.25f;
              float num21 = num18 * 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num15 * num19, num15 * num20, num15 * num21);
            }
            if (dust.type == 182)
            {
              ++dust.rotation;
              if (!dust.noLight)
              {
                float num22 = dust.scale * 0.25f;
                if ((double) num22 > 1.0)
                  num22 = 1f;
                float num23 = num22;
                float num24 = num22;
                float num25 = num22;
                float num26 = num23 * 1f;
                float num27 = num24 * 0.2f;
                float num28 = num25 * 0.1f;
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num22 * num26, num22 * num27, num22 * num28);
              }
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
                dust.customData = (object) null;
              }
            }
            if (dust.type == 261)
            {
              if (!dust.noLight && !dust.noLightEmittence)
              {
                float num29 = dust.scale * 0.3f;
                if ((double) num29 > 1.0)
                  num29 = 1f;
                Lighting.AddLight(dust.position, new Vector3(0.4f, 0.6f, 0.7f) * num29);
              }
              if (dust.noGravity)
              {
                dust.velocity *= 0.93f;
                if ((double) dust.fadeIn == 0.0)
                  dust.scale += 1f / 400f;
              }
              dust.velocity *= new Vector2(0.97f, 0.99f);
              dust.scale -= 1f / 400f;
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
            }
            if (dust.type == 254)
            {
              float num30 = dust.scale * 0.35f;
              if ((double) num30 > 1.0)
                num30 = 1f;
              float num31 = num30;
              float num32 = num30;
              float num33 = num30;
              float num34 = num31 * 0.9f;
              float num35 = num32 * 0.1f;
              float num36 = num33 * 0.75f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num30 * num34, num30 * num35, num30 * num36);
            }
            if (dust.type == (int) byte.MaxValue)
            {
              float num37 = dust.scale * 0.25f;
              if ((double) num37 > 1.0)
                num37 = 1f;
              float num38 = num37;
              float num39 = num37;
              float num40 = num37;
              float num41 = num38 * 0.9f;
              float num42 = num39 * 0.1f;
              float num43 = num40 * 0.75f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num37 * num41, num37 * num42, num37 * num43);
            }
            if (dust.type == 211 && dust.noLight && Collision.SolidCollision(dust.position, 4, 4))
              dust.active = false;
            if (dust.type == 284 && Collision.SolidCollision(dust.position - Vector2.One * 4f, 8, 8) && (double) dust.fadeIn == 0.0)
              dust.velocity *= 0.25f;
            if (dust.type == 213 || dust.type == 260)
            {
              dust.rotation = 0.0f;
              float num44 = (float) ((double) dust.scale / 2.5 * 0.200000002980232);
              Vector3 vector3_1 = Vector3.Zero;
              switch (dust.type)
              {
                case 213:
                  vector3_1 = new Vector3((float) byte.MaxValue, 217f, 48f);
                  break;
                case 260:
                  vector3_1 = new Vector3((float) byte.MaxValue, 48f, 48f);
                  break;
              }
              Vector3 vector3_2 = vector3_1 / (float) byte.MaxValue;
              if ((double) num44 > 1.0)
                num44 = 1f;
              Vector3 vector3_3 = vector3_2 * num44;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), vector3_3.X, vector3_3.Y, vector3_3.Z);
            }
            if (dust.type == 157)
            {
              float num45 = dust.scale * 0.2f;
              float num46 = num45;
              float num47 = num45;
              float num48 = num45;
              float num49 = num46 * 0.25f;
              float num50 = num47 * 1f;
              float num51 = num48 * 0.5f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num45 * num49, num45 * num50, num45 * num51);
            }
            if (dust.type == 206)
            {
              dust.scale -= 0.1f;
              float num52 = dust.scale * 0.4f;
              float num53 = num52;
              float num54 = num52;
              float num55 = num52;
              float num56 = num53 * 0.1f;
              float num57 = num54 * 0.6f;
              float num58 = num55 * 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num52 * num56, num52 * num57, num52 * num58);
            }
            if (dust.type == 163)
            {
              float num59 = dust.scale * 0.25f;
              float num60 = num59;
              float num61 = num59;
              float num62 = num59;
              float num63 = num60 * 0.25f;
              float num64 = num61 * 1f;
              float num65 = num62 * 0.05f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num59 * num63, num59 * num64, num59 * num65);
            }
            if (dust.type == 205)
            {
              float num66 = dust.scale * 0.25f;
              float num67 = num66;
              float num68 = num66;
              float num69 = num66;
              float num70 = num67 * 1f;
              float num71 = num68 * 0.05f;
              float num72 = num69 * 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num66 * num70, num66 * num71, num66 * num72);
            }
            if (dust.type == 170)
            {
              float num73 = dust.scale * 0.5f;
              float num74 = num73;
              float num75 = num73;
              float num76 = num73;
              float num77 = num74 * 1f;
              float num78 = num75 * 1f;
              float num79 = num76 * 0.05f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num73 * num77, num73 * num78, num73 * num79);
            }
            if (dust.type == 156)
            {
              float lightAmount = dust.scale * 0.6f;
              int type = dust.type;
              float num80 = lightAmount;
              float num81 = lightAmount;
              float num82 = num80 * 0.9f;
              float num83 = num81 * 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 12, lightAmount);
            }
            if (dust.type == 234)
            {
              float lightAmount = dust.scale * 0.6f;
              int type = dust.type;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 13, lightAmount);
            }
            if (dust.type == 175)
              dust.scale -= 0.05f;
            if (dust.type == 174)
            {
              dust.scale -= 0.01f;
              float r = dust.scale * 1f;
              if ((double) r > 0.600000023841858)
                r = 0.6f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, r * 0.4f, 0.0f);
            }
            if (dust.type == 235)
            {
              Vector2 vector2 = new Vector2((float) Main.rand.Next(-100, 101), (float) Main.rand.Next(-100, 101));
              vector2.Normalize();
              vector2 *= 15f;
              dust.scale -= 0.01f;
            }
            else if (dust.type == 228 || dust.type == 279 || dust.type == 229 || dust.type == 6 || dust.type == 242 || dust.type == 135 || dust.type == (int) sbyte.MaxValue || dust.type == 187 || dust.type == 75 || dust.type == 169 || dust.type == 29 || dust.type >= 59 && dust.type <= 65 || dust.type == 158 || dust.type == 293 || dust.type == 294 || dust.type == 295 || dust.type == 296 || dust.type == 297 || dust.type == 298 || dust.type == 302)
            {
              if (!dust.noGravity)
                dust.velocity.Y += 0.05f;
              if (dust.type == 229 || dust.type == 228 || dust.type == 279)
              {
                if (dust.customData != null && dust.customData is NPC)
                {
                  NPC customData = (NPC) dust.customData;
                  dust.position += customData.position - customData.oldPos[1];
                }
                else if (dust.customData != null && dust.customData is Player)
                {
                  Player customData = (Player) dust.customData;
                  dust.position += customData.position - customData.oldPosition;
                }
                else if (dust.customData != null && dust.customData is Vector2)
                {
                  Vector2 vector2 = (Vector2) dust.customData - dust.position;
                  if (vector2 != Vector2.Zero)
                    vector2.Normalize();
                  dust.velocity = (dust.velocity * 4f + vector2 * dust.velocity.Length()) / 5f;
                }
              }
              if (!dust.noLight && !dust.noLightEmittence)
              {
                float num84 = dust.scale * 1.4f;
                if (dust.type == 29)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num84 * 0.1f, num84 * 0.4f, num84);
                }
                else if (dust.type == 75)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 8, num84);
                }
                else if (dust.type == 169)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 11, num84);
                }
                else if (dust.type == 135)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 9, num84);
                }
                else if (dust.type == 158)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 10, num84);
                }
                else if (dust.type == 228)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num84 * 0.7f, num84 * 0.65f, num84 * 0.3f);
                }
                else if (dust.type == 229)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num84 * 0.3f, num84 * 0.65f, num84 * 0.7f);
                }
                else if (dust.type == 242)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 15, num84);
                }
                else if (dust.type == 293)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  float lightAmount = num84 * 0.95f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 16, lightAmount);
                }
                else if (dust.type == 294)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 17, num84);
                }
                else if (dust.type >= 59 && dust.type <= 65)
                {
                  if ((double) num84 > 0.800000011920929)
                    num84 = 0.8f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 1 + dust.type - 59, num84);
                }
                else if (dust.type == (int) sbyte.MaxValue)
                {
                  float r = num84 * 1.3f;
                  if ((double) r > 1.0)
                    r = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, r * 0.45f, r * 0.2f);
                }
                else if (dust.type == 187)
                {
                  float b = num84 * 1.3f;
                  if ((double) b > 1.0)
                    b = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.2f, b * 0.45f, b);
                }
                else if (dust.type == 295)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 18, num84);
                }
                else if (dust.type == 296)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 19, num84);
                }
                else if (dust.type == 297)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 20, num84);
                }
                else if (dust.type == 298)
                {
                  if ((double) num84 > 1.0)
                    num84 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 21, num84);
                }
                else
                {
                  if ((double) num84 > 0.600000023841858)
                    num84 = 0.6f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num84, num84 * 0.65f, num84 * 0.4f);
                }
              }
            }
            else if (dust.type == 269)
            {
              if (!dust.noLight)
              {
                float num85 = dust.scale * 1.4f;
                if ((double) num85 > 1.0)
                  num85 = 1f;
                Vector3 vector3 = new Vector3(0.7f, 0.65f, 0.3f);
                Lighting.AddLight(dust.position, vector3 * num85);
              }
              if (dust.customData != null && dust.customData is Vector2)
              {
                Vector2 vector2 = (Vector2) dust.customData - dust.position;
                dust.velocity.X += 1f * (float) Math.Sign(vector2.X) * dust.scale;
              }
            }
            else if (dust.type == 159)
            {
              float num86 = dust.scale * 1.3f;
              if ((double) num86 > 1.0)
                num86 = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num86, num86, num86 * 0.1f);
              if (dust.noGravity)
              {
                if ((double) dust.scale < 0.699999988079071)
                  dust.velocity *= 1.075f;
                else if (Main.rand.Next(2) == 0)
                  dust.velocity *= -0.95f;
                else
                  dust.velocity *= 1.05f;
                dust.scale -= 0.03f;
              }
              else
              {
                dust.scale += 0.005f;
                dust.velocity *= 0.9f;
                dust.velocity.X += (float) Main.rand.Next(-10, 11) * 0.02f;
                dust.velocity.Y += (float) Main.rand.Next(-10, 11) * 0.02f;
                if (Main.rand.Next(5) == 0)
                {
                  int index2 = Dust.NewDust(dust.position, 4, 4, dust.type);
                  Main.dust[index2].noGravity = true;
                  Main.dust[index2].scale = dust.scale * 2.5f;
                }
              }
            }
            else if (dust.type == 164)
            {
              float r = dust.scale;
              if ((double) r > 1.0)
                r = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, r * 0.1f, r * 0.8f);
              if (dust.noGravity)
              {
                if ((double) dust.scale < 0.699999988079071)
                  dust.velocity *= 1.075f;
                else if (Main.rand.Next(2) == 0)
                  dust.velocity *= -0.95f;
                else
                  dust.velocity *= 1.05f;
                dust.scale -= 0.03f;
              }
              else
              {
                dust.scale -= 0.005f;
                dust.velocity *= 0.9f;
                dust.velocity.X += (float) Main.rand.Next(-10, 11) * 0.02f;
                dust.velocity.Y += (float) Main.rand.Next(-10, 11) * 0.02f;
                if (Main.rand.Next(5) == 0)
                {
                  int index3 = Dust.NewDust(dust.position, 4, 4, dust.type);
                  Main.dust[index3].noGravity = true;
                  Main.dust[index3].scale = dust.scale * 2.5f;
                }
              }
            }
            else if (dust.type == 173)
            {
              float b = dust.scale;
              if ((double) b > 1.0)
                b = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.4f, b * 0.1f, b);
              if (dust.noGravity)
              {
                dust.velocity *= 0.8f;
                dust.velocity.X += (float) Main.rand.Next(-20, 21) * 0.01f;
                dust.velocity.Y += (float) Main.rand.Next(-20, 21) * 0.01f;
                dust.scale -= 0.01f;
              }
              else
              {
                dust.scale -= 0.015f;
                dust.velocity *= 0.8f;
                dust.velocity.X += (float) Main.rand.Next(-10, 11) * 0.005f;
                dust.velocity.Y += (float) Main.rand.Next(-10, 11) * 0.005f;
                if (Main.rand.Next(10) == 10)
                {
                  int index4 = Dust.NewDust(dust.position, 4, 4, dust.type);
                  Main.dust[index4].noGravity = true;
                  Main.dust[index4].scale = dust.scale;
                }
              }
            }
            else if (dust.type == 304)
            {
              dust.velocity.Y = (float) Math.Sin((double) dust.rotation) / 5f;
              dust.rotation += 0.015f;
              if ((double) dust.scale < 1.14999997615814)
              {
                dust.alpha = Math.Max(0, dust.alpha - 20);
                dust.scale += 0.0015f;
              }
              else
              {
                dust.alpha += 6;
                if (dust.alpha >= (int) byte.MaxValue)
                  dust.active = false;
              }
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                float num87 = Utils.Remap(dust.scale, 1f, 1.05f, 1f, 0.0f);
                if ((double) num87 > 0.0)
                  dust.position += customData.velocity * num87;
                float num88 = customData.Center.X - dust.position.X;
                if ((double) Math.Abs(num88) > 20.0)
                {
                  float num89 = num88 * 0.01f;
                  dust.velocity.X = MathHelper.Lerp(dust.velocity.X, num89, num87 * 0.2f);
                }
              }
            }
            else if (dust.type == 184)
            {
              if (!dust.noGravity)
              {
                dust.velocity *= 0.0f;
                dust.scale -= 0.01f;
              }
            }
            else if (dust.type == 160 || dust.type == 162)
            {
              float num90 = dust.scale * 1.3f;
              if ((double) num90 > 1.0)
                num90 = 1f;
              if (dust.type == 162)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num90, num90 * 0.7f, num90 * 0.1f);
              else
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num90 * 0.1f, num90, num90);
              if (dust.noGravity)
              {
                dust.velocity *= 0.8f;
                dust.velocity.X += (float) Main.rand.Next(-20, 21) * 0.04f;
                dust.velocity.Y += (float) Main.rand.Next(-20, 21) * 0.04f;
                dust.scale -= 0.1f;
              }
              else
              {
                dust.scale -= 0.1f;
                dust.velocity.X += (float) Main.rand.Next(-10, 11) * 0.02f;
                dust.velocity.Y += (float) Main.rand.Next(-10, 11) * 0.02f;
                if ((double) dust.scale > 0.3 && Main.rand.Next(50) == 0)
                {
                  int index5 = Dust.NewDust(new Vector2(dust.position.X - 4f, dust.position.Y - 4f), 1, 1, dust.type);
                  Main.dust[index5].noGravity = true;
                  Main.dust[index5].scale = dust.scale * 1.5f;
                }
              }
            }
            else if (dust.type == 168)
            {
              float r = dust.scale * 0.8f;
              if ((double) r > 0.55)
                r = 0.55f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, 0.0f, r * 0.8f);
              dust.scale += 0.03f;
              dust.velocity.X += (float) Main.rand.Next(-10, 11) * 0.02f;
              dust.velocity.Y += (float) Main.rand.Next(-10, 11) * 0.02f;
              dust.velocity *= 0.99f;
            }
            else if (dust.type >= 139 && dust.type < 143)
            {
              dust.velocity.X *= 0.98f;
              dust.velocity.Y *= 0.98f;
              if ((double) dust.velocity.Y < 1.0)
                dust.velocity.Y += 0.05f;
              dust.scale += 0.009f;
              dust.rotation -= dust.velocity.X * 0.4f;
              if ((double) dust.velocity.X > 0.0)
                dust.rotation += 0.005f;
              else
                dust.rotation -= 0.005f;
            }
            else if (dust.type == 14 || dust.type == 16 || dust.type == 31 || dust.type == 46 || dust.type == 124 || dust.type == 186 || dust.type == 188 || dust.type == 303)
            {
              dust.velocity.Y *= 0.98f;
              dust.velocity.X *= 0.98f;
              if (dust.type == 31)
              {
                if (dust.customData != null && dust.customData is NPC)
                {
                  NPC customData = (NPC) dust.customData;
                  dust.position += customData.position - customData.oldPosition;
                  if (dust.noGravity)
                    dust.velocity *= 1.02f;
                  dust.alpha -= 70;
                  if (dust.alpha < 0)
                    dust.alpha = 0;
                  dust.scale *= 0.97f;
                  if ((double) dust.scale <= 0.00999999977648258)
                  {
                    dust.scale = 0.0001f;
                    dust.alpha = (int) byte.MaxValue;
                  }
                }
                else if (dust.noGravity)
                {
                  dust.velocity *= 1.02f;
                  dust.scale += 0.02f;
                  dust.alpha += 4;
                  if (dust.alpha > (int) byte.MaxValue)
                  {
                    dust.scale = 0.0001f;
                    dust.alpha = (int) byte.MaxValue;
                  }
                }
              }
              if (dust.type == 303 && dust.noGravity)
              {
                dust.velocity *= 1.02f;
                dust.scale += 0.03f;
                if (dust.alpha < 90)
                  dust.alpha = 90;
                dust.alpha += 4;
                if (dust.alpha > (int) byte.MaxValue)
                {
                  dust.scale = 0.0001f;
                  dust.alpha = (int) byte.MaxValue;
                }
              }
            }
            else if (dust.type == 32)
            {
              dust.scale -= 0.01f;
              dust.velocity.X *= 0.96f;
              if (!dust.noGravity)
                dust.velocity.Y += 0.1f;
            }
            else if (dust.type >= 244 && dust.type <= 247)
            {
              dust.rotation += 0.1f * dust.scale;
              Color color = Lighting.GetColor((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0));
              int num91;
              float num92 = (float) (((double) (num91 = (int) (byte) (((int) color.R + (int) color.G + (int) color.B) / 3)) / 270.0 + 1.0) / 2.0);
              float num93 = (float) (((double) num91 / 270.0 + 1.0) / 2.0);
              float num94 = (float) (((double) num91 / 270.0 + 1.0) / 2.0);
              float num95 = num92 * (dust.scale * 0.9f);
              float num96 = num93 * (dust.scale * 0.9f);
              float num97 = num94 * (dust.scale * 0.9f);
              if (dust.alpha < (int) byte.MaxValue)
              {
                dust.scale += 0.09f;
                if ((double) dust.scale >= 1.0)
                {
                  dust.scale = 1f;
                  dust.alpha = (int) byte.MaxValue;
                }
              }
              else
              {
                if ((double) dust.scale < 0.8)
                  dust.scale -= 0.01f;
                if ((double) dust.scale < 0.5)
                  dust.scale -= 0.01f;
              }
              float num98 = 1f;
              if (dust.type == 244)
              {
                num95 *= 0.8862745f;
                num96 *= 0.4627451f;
                num97 *= 0.2980392f;
                num98 = 0.9f;
              }
              else if (dust.type == 245)
              {
                num95 *= 0.5137255f;
                num96 *= 0.6745098f;
                num97 *= 0.6784314f;
                num98 = 1f;
              }
              else if (dust.type == 246)
              {
                num95 *= 0.8f;
                num96 *= 0.7098039f;
                num97 *= 0.282353f;
                num98 = 1.1f;
              }
              else if (dust.type == 247)
              {
                num95 *= 0.6f;
                num96 *= 0.6745098f;
                num97 *= 0.7254902f;
                num98 = 1.2f;
              }
              float r = num95 * num98;
              float g = num96 * num98;
              float b = num97 * num98;
              if (!dust.noLightEmittence)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, g, b);
            }
            else if (dust.type == 43)
            {
              dust.rotation += 0.1f * dust.scale;
              Color color = Lighting.GetColor((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0));
              float num99 = (float) color.R / 270f;
              float num100 = (float) color.G / 270f;
              float num101 = (float) color.B / 270f;
              float num102 = (float) dust.color.R / (float) byte.MaxValue;
              float num103 = (float) dust.color.G / (float) byte.MaxValue;
              float num104 = (float) dust.color.B / (float) byte.MaxValue;
              float r = num99 * (dust.scale * 1.07f * num102);
              float g = num100 * (dust.scale * 1.07f * num103);
              float b = num101 * (dust.scale * 1.07f * num104);
              if (dust.alpha < (int) byte.MaxValue)
              {
                dust.scale += 0.09f;
                if ((double) dust.scale >= 1.0)
                {
                  dust.scale = 1f;
                  dust.alpha = (int) byte.MaxValue;
                }
              }
              else
              {
                if ((double) dust.scale < 0.8)
                  dust.scale -= 0.01f;
                if ((double) dust.scale < 0.5)
                  dust.scale -= 0.01f;
              }
              if ((double) r < 0.05 && (double) g < 0.05 && (double) b < 0.05)
                dust.active = false;
              else if (!dust.noLightEmittence)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, g, b);
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
            }
            else if (dust.type == 15 || dust.type == 57 || dust.type == 58 || dust.type == 274 || dust.type == 292)
            {
              dust.velocity.Y *= 0.98f;
              dust.velocity.X *= 0.98f;
              if (!dust.noLightEmittence)
              {
                float num105 = dust.scale;
                if (dust.type != 15)
                  num105 = dust.scale * 0.8f;
                if (dust.noLight)
                  dust.velocity *= 0.95f;
                if ((double) num105 > 1.0)
                  num105 = 1f;
                if (dust.type == 15)
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num105 * 0.45f, num105 * 0.55f, num105);
                else if (dust.type == 57)
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num105 * 0.95f, num105 * 0.95f, num105 * 0.45f);
                else if (dust.type == 58)
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num105, num105 * 0.55f, num105 * 0.75f);
              }
            }
            else if (dust.type == 204)
            {
              if ((double) dust.fadeIn > (double) dust.scale)
                dust.scale += 0.02f;
              else
                dust.scale -= 0.02f;
              dust.velocity *= 0.95f;
            }
            else if (dust.type == 110)
            {
              float g = dust.scale * 0.1f;
              if ((double) g > 1.0)
                g = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), g * 0.2f, g, g * 0.5f);
            }
            else if (dust.type == 111)
            {
              float b = dust.scale * 0.125f;
              if ((double) b > 1.0)
                b = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.2f, b * 0.7f, b);
            }
            else if (dust.type == 112)
            {
              float num106 = dust.scale * 0.1f;
              if ((double) num106 > 1.0)
                num106 = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num106 * 0.8f, num106 * 0.2f, num106 * 0.8f);
            }
            else if (dust.type == 113)
            {
              float num107 = dust.scale * 0.1f;
              if ((double) num107 > 1.0)
                num107 = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num107 * 0.2f, num107 * 0.3f, num107 * 1.3f);
            }
            else if (dust.type == 114)
            {
              float num108 = dust.scale * 0.1f;
              if ((double) num108 > 1.0)
                num108 = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num108 * 1.2f, num108 * 0.5f, num108 * 0.4f);
            }
            else if (dust.type == 66)
            {
              if ((double) dust.velocity.X < 0.0)
                --dust.rotation;
              else
                ++dust.rotation;
              dust.velocity.Y *= 0.98f;
              dust.velocity.X *= 0.98f;
              dust.scale += 0.02f;
              float num109 = dust.scale;
              if (dust.type != 15)
                num109 = dust.scale * 0.8f;
              if ((double) num109 > 1.0)
                num109 = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num109 * ((float) dust.color.R / (float) byte.MaxValue), num109 * ((float) dust.color.G / (float) byte.MaxValue), num109 * ((float) dust.color.B / (float) byte.MaxValue));
            }
            else if (dust.type == 267)
            {
              if ((double) dust.velocity.X < 0.0)
                --dust.rotation;
              else
                ++dust.rotation;
              dust.velocity.Y *= 0.98f;
              dust.velocity.X *= 0.98f;
              dust.scale += 0.02f;
              float num110 = dust.scale * 0.8f;
              if ((double) num110 > 1.0)
                num110 = 1f;
              if (dust.noLight)
                dust.noLight = false;
              if (!dust.noLight && !dust.noLightEmittence)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num110 * ((float) dust.color.R / (float) byte.MaxValue), num110 * ((float) dust.color.G / (float) byte.MaxValue), num110 * ((float) dust.color.B / (float) byte.MaxValue));
            }
            else if (dust.type == 20 || dust.type == 21 || dust.type == 231)
            {
              dust.scale += 0.005f;
              dust.velocity.Y *= 0.94f;
              dust.velocity.X *= 0.94f;
              float b1 = dust.scale * 0.8f;
              if ((double) b1 > 1.0)
                b1 = 1f;
              if (dust.type == 21 && !dust.noLightEmittence)
              {
                float b2 = dust.scale * 0.4f;
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b2 * 0.8f, b2 * 0.3f, b2);
              }
              else if (dust.type == 231)
              {
                float r = dust.scale * 0.4f;
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, r * 0.5f, r * 0.3f);
              }
              else
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b1 * 0.3f, b1 * 0.6f, b1);
            }
            else if (dust.type == 27 || dust.type == 45)
            {
              if (dust.type == 27 && (double) dust.fadeIn >= 100.0)
              {
                if ((double) dust.scale >= 1.5)
                  dust.scale -= 0.01f;
                else
                  dust.scale -= 0.05f;
                if ((double) dust.scale <= 0.5)
                  dust.scale -= 0.05f;
                if ((double) dust.scale <= 0.25)
                  dust.scale -= 0.05f;
              }
              dust.velocity *= 0.94f;
              dust.scale += 1f / 500f;
              float b = dust.scale;
              if (dust.noLight)
              {
                b *= 0.1f;
                dust.scale -= 0.06f;
                if ((double) dust.scale < 1.0)
                  dust.scale -= 0.06f;
                if (Main.player[Main.myPlayer].wet)
                  dust.position += Main.player[Main.myPlayer].velocity * 0.5f;
                else
                  dust.position += Main.player[Main.myPlayer].velocity;
              }
              if ((double) b > 1.0)
                b = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.6f, b * 0.2f, b);
            }
            else if (dust.type == 55 || dust.type == 56 || dust.type == 73 || dust.type == 74)
            {
              dust.velocity *= 0.98f;
              if (!dust.noLightEmittence)
              {
                float num111 = dust.scale * 0.8f;
                if (dust.type == 55)
                {
                  if ((double) num111 > 1.0)
                    num111 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num111, num111, num111 * 0.6f);
                }
                else if (dust.type == 73)
                {
                  if ((double) num111 > 1.0)
                    num111 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num111, num111 * 0.35f, num111 * 0.5f);
                }
                else if (dust.type == 74)
                {
                  if ((double) num111 > 1.0)
                    num111 = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num111 * 0.35f, num111, num111 * 0.5f);
                }
                else
                {
                  float b = dust.scale * 1.2f;
                  if ((double) b > 1.0)
                    b = 1f;
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.35f, b * 0.5f, b);
                }
              }
            }
            else if (dust.type == 71 || dust.type == 72)
            {
              dust.velocity *= 0.98f;
              float num112 = dust.scale;
              if ((double) num112 > 1.0)
                num112 = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num112 * 0.2f, 0.0f, num112 * 0.1f);
            }
            else if (dust.type == 76)
            {
              ++Main.snowDust;
              dust.scale += 0.009f;
              float y = Main.player[Main.myPlayer].velocity.Y;
              if ((double) y > 0.0 && (double) dust.fadeIn == 0.0 && (double) dust.velocity.Y < (double) y)
                dust.velocity.Y = MathHelper.Lerp(dust.velocity.Y, y, 0.04f);
              if (!dust.noLight && (double) y > 0.0)
                dust.position.Y += Main.player[Main.myPlayer].velocity.Y * 0.2f;
              if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && (double) dust.fadeIn == 0.0)
              {
                dust.scale *= 0.9f;
                dust.velocity *= 0.25f;
              }
            }
            else if (dust.type == 270)
            {
              dust.velocity *= 1.005025f;
              dust.scale += 0.01f;
              dust.rotation = 0.0f;
              if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && (double) dust.fadeIn == 0.0)
              {
                dust.scale *= 0.95f;
                dust.velocity *= 0.25f;
              }
              else
              {
                dust.velocity.Y = (float) Math.Sin((double) dust.position.X * 0.0043982295319438) * 2f;
                dust.velocity.Y -= 3f;
                dust.velocity.Y /= 20f;
              }
            }
            else if (dust.type == 271)
            {
              dust.velocity *= 1.005025f;
              dust.scale += 3f / 1000f;
              dust.rotation = 0.0f;
              dust.velocity.Y -= 4f;
              dust.velocity.Y /= 6f;
            }
            else if (dust.type == 268)
            {
              ++Dust.SandStormCount;
              dust.velocity *= 1.005025f;
              dust.scale += 0.01f;
              if (!flag)
                dust.scale -= 0.05f;
              dust.rotation = 0.0f;
              float y = Main.player[Main.myPlayer].velocity.Y;
              if ((double) y > 0.0 && (double) dust.fadeIn == 0.0 && (double) dust.velocity.Y < (double) y)
                dust.velocity.Y = MathHelper.Lerp(dust.velocity.Y, y, 0.04f);
              if (!dust.noLight && (double) y > 0.0)
                dust.position.Y += y * 0.2f;
              if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && (double) dust.fadeIn == 0.0)
              {
                dust.scale *= 0.9f;
                dust.velocity *= 0.25f;
              }
              else
              {
                dust.velocity.Y = (float) Math.Sin((double) dust.position.X * 0.0043982295319438) * 2f;
                dust.velocity.Y += 3f;
              }
            }
            else if (!dust.noGravity && dust.type != 41 && dust.type != 44)
            {
              if (dust.type == 107)
                dust.velocity *= 0.9f;
              else
                dust.velocity.Y += 0.1f;
            }
            if (dust.type == 5 || dust.type == 273 && dust.noGravity)
              dust.scale -= 0.04f;
            if (dust.type == 33 || dust.type == 52 || dust.type == 266 || dust.type == 98 || dust.type == 99 || dust.type == 100 || dust.type == 101 || dust.type == 102 || dust.type == 103 || dust.type == 104 || dust.type == 105 || dust.type == 123 || dust.type == 288)
            {
              if ((double) dust.velocity.X == 0.0)
              {
                if (Collision.SolidCollision(dust.position, 2, 2))
                  dust.scale = 0.0f;
                dust.rotation += 0.5f;
                dust.scale -= 0.01f;
              }
              if (Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y), 4, 4))
              {
                dust.alpha += 20;
                dust.scale -= 0.1f;
              }
              dust.alpha += 2;
              dust.scale -= 0.005f;
              if (dust.alpha > (int) byte.MaxValue)
                dust.scale = 0.0f;
              if ((double) dust.velocity.Y > 4.0)
                dust.velocity.Y = 4f;
              if (dust.noGravity)
              {
                if ((double) dust.velocity.X < 0.0)
                  dust.rotation -= 0.2f;
                else
                  dust.rotation += 0.2f;
                dust.scale += 0.03f;
                dust.velocity.X *= 1.05f;
                dust.velocity.Y += 0.15f;
              }
            }
            if (dust.type == 35 && dust.noGravity)
            {
              dust.scale += 0.03f;
              if ((double) dust.scale < 1.0)
                dust.velocity.Y += 0.075f;
              dust.velocity.X *= 1.08f;
              if ((double) dust.velocity.X > 0.0)
                dust.rotation += 0.01f;
              else
                dust.rotation -= 0.01f;
              float r = dust.scale * 0.6f;
              if ((double) r > 1.0)
                r = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0 + 1.0), r, r * 0.3f, r * 0.1f);
            }
            else if (dust.type == 152 && dust.noGravity)
            {
              dust.scale += 0.03f;
              if ((double) dust.scale < 1.0)
                dust.velocity.Y += 0.075f;
              dust.velocity.X *= 1.08f;
              if ((double) dust.velocity.X > 0.0)
                dust.rotation += 0.01f;
              else
                dust.rotation -= 0.01f;
            }
            else if (dust.type == 67 || dust.type == 92)
            {
              float b = dust.scale;
              if ((double) b > 1.0)
                b = 1f;
              if (dust.noLight)
                b *= 0.1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), 0.0f, b * 0.8f, b);
            }
            else if (dust.type == 185)
            {
              float b = dust.scale;
              if ((double) b > 1.0)
                b = 1f;
              if (dust.noLight)
                b *= 0.1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.1f, b * 0.7f, b);
            }
            else if (dust.type == 107)
            {
              float g = dust.scale * 0.5f;
              if ((double) g > 1.0)
                g = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), g * 0.1f, g, g * 0.4f);
            }
            else if (dust.type == 34 || dust.type == 35 || dust.type == 152)
            {
              if (!Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y - 8f), 4, 4))
              {
                dust.scale = 0.0f;
              }
              else
              {
                dust.alpha += Main.rand.Next(2);
                if (dust.alpha > (int) byte.MaxValue)
                  dust.scale = 0.0f;
                dust.velocity.Y = -0.5f;
                if (dust.type == 34)
                {
                  dust.scale += 0.005f;
                }
                else
                {
                  ++dust.alpha;
                  dust.scale -= 0.01f;
                  dust.velocity.Y = -0.2f;
                }
                dust.velocity.X += (float) Main.rand.Next(-10, 10) * (1f / 500f);
                if ((double) dust.velocity.X < -0.25)
                  dust.velocity.X = -0.25f;
                if ((double) dust.velocity.X > 0.25)
                  dust.velocity.X = 0.25f;
              }
              if (dust.type == 35)
              {
                float r = (float) ((double) dust.scale * 0.300000011920929 + 0.400000005960464);
                if ((double) r > 1.0)
                  r = 1f;
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), r, r * 0.5f, r * 0.3f);
              }
            }
            if (dust.type == 68)
            {
              float b = dust.scale * 0.3f;
              if ((double) b > 1.0)
                b = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.1f, b * 0.2f, b);
            }
            if (dust.type == 70)
            {
              float b = dust.scale * 0.3f;
              if ((double) b > 1.0)
                b = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.5f, 0.0f, b);
            }
            if (dust.type == 41)
            {
              dust.velocity.X += (float) Main.rand.Next(-10, 11) * 0.01f;
              dust.velocity.Y += (float) Main.rand.Next(-10, 11) * 0.01f;
              if ((double) dust.velocity.X > 0.75)
                dust.velocity.X = 0.75f;
              if ((double) dust.velocity.X < -0.75)
                dust.velocity.X = -0.75f;
              if ((double) dust.velocity.Y > 0.75)
                dust.velocity.Y = 0.75f;
              if ((double) dust.velocity.Y < -0.75)
                dust.velocity.Y = -0.75f;
              dust.scale += 0.007f;
              float b = dust.scale * 0.7f;
              if ((double) b > 1.0)
                b = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), b * 0.4f, b * 0.9f, b);
            }
            else if (dust.type == 44)
            {
              dust.velocity.X += (float) Main.rand.Next(-10, 11) * (3f / 1000f);
              dust.velocity.Y += (float) Main.rand.Next(-10, 11) * (3f / 1000f);
              if ((double) dust.velocity.X > 0.35)
                dust.velocity.X = 0.35f;
              if ((double) dust.velocity.X < -0.35)
                dust.velocity.X = -0.35f;
              if ((double) dust.velocity.Y > 0.35)
                dust.velocity.Y = 0.35f;
              if ((double) dust.velocity.Y < -0.35)
                dust.velocity.Y = -0.35f;
              dust.scale += 0.0085f;
              float g = dust.scale * 0.7f;
              if ((double) g > 1.0)
                g = 1f;
              Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), g * 0.7f, g, g * 0.8f);
            }
            else if (dust.type != 304)
              dust.velocity.X *= 0.99f;
            if (dust.type != 79 && dust.type != 268 && dust.type != 304)
              dust.rotation += dust.velocity.X * 0.5f;
            if ((double) dust.fadeIn > 0.0 && (double) dust.fadeIn < 100.0)
            {
              if (dust.type == 235)
              {
                dust.scale += 0.007f;
                int index6 = (int) dust.fadeIn - 1;
                if (index6 >= 0 && index6 <= (int) byte.MaxValue)
                {
                  Vector2 vector2_3 = dust.position - Main.player[index6].Center;
                  float num113 = 100f - vector2_3.Length();
                  if ((double) num113 > 0.0)
                    dust.scale -= num113 * 0.0015f;
                  vector2_3.Normalize();
                  float num114 = (float) ((1.0 - (double) dust.scale) * 20.0);
                  Vector2 vector2_4 = vector2_3 * -num114;
                  dust.velocity = (dust.velocity * 4f + vector2_4) / 5f;
                }
              }
              else if (dust.type == 46)
                dust.scale += 0.1f;
              else if (dust.type == 213 || dust.type == 260)
                dust.scale += 0.1f;
              else
                dust.scale += 0.03f;
              if ((double) dust.scale > (double) dust.fadeIn)
                dust.fadeIn = 0.0f;
            }
            else if (dust.type != 304)
            {
              if (dust.type == 213 || dust.type == 260)
                dust.scale -= 0.2f;
              else
                dust.scale -= 0.01f;
            }
            if (dust.type >= 130 && dust.type <= 134)
            {
              float num115 = dust.scale;
              if ((double) num115 > 1.0)
                num115 = 1f;
              if (dust.type == 130)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num115 * 1f, num115 * 0.5f, num115 * 0.4f);
              if (dust.type == 131)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num115 * 0.4f, num115 * 1f, num115 * 0.6f);
              if (dust.type == 132)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num115 * 0.3f, num115 * 0.5f, num115 * 1f);
              if (dust.type == 133)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num115 * 0.9f, num115 * 0.9f, num115 * 0.3f);
              if (dust.noGravity)
              {
                dust.velocity *= 0.93f;
                if ((double) dust.fadeIn == 0.0)
                  dust.scale += 1f / 400f;
              }
              else if (dust.type == 131)
              {
                dust.velocity *= 0.98f;
                dust.velocity.Y -= 0.1f;
                dust.scale += 1f / 400f;
              }
              else
              {
                dust.velocity *= 0.95f;
                dust.scale -= 1f / 400f;
              }
            }
            else if (dust.type == 278)
            {
              float num116 = dust.scale;
              if ((double) num116 > 1.0)
                num116 = 1f;
              if (!dust.noLight)
                Lighting.AddLight(dust.position, dust.color.ToVector3() * num116);
              if (dust.noGravity)
              {
                dust.velocity *= 0.93f;
                if ((double) dust.fadeIn == 0.0)
                  dust.scale += 1f / 400f;
              }
              else
              {
                dust.velocity *= 0.95f;
                dust.scale -= 1f / 400f;
              }
              if (WorldGen.SolidTile(Framing.GetTileSafely(dust.position)) && (double) dust.fadeIn == 0.0 && !dust.noGravity)
              {
                dust.scale *= 0.9f;
                dust.velocity *= 0.25f;
              }
            }
            else if (dust.type >= 219 && dust.type <= 223)
            {
              float num117 = dust.scale;
              if ((double) num117 > 1.0)
                num117 = 1f;
              if (!dust.noLight)
              {
                if (dust.type == 219)
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num117 * 1f, num117 * 0.5f, num117 * 0.4f);
                if (dust.type == 220)
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num117 * 0.4f, num117 * 1f, num117 * 0.6f);
                if (dust.type == 221)
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num117 * 0.3f, num117 * 0.5f, num117 * 1f);
                if (dust.type == 222)
                  Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num117 * 0.9f, num117 * 0.9f, num117 * 0.3f);
              }
              if (dust.noGravity)
              {
                dust.velocity *= 0.93f;
                if ((double) dust.fadeIn == 0.0)
                  dust.scale += 1f / 400f;
              }
              dust.velocity *= new Vector2(0.97f, 0.99f);
              dust.scale -= 1f / 400f;
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
            }
            else if (dust.type == 226)
            {
              float num118 = dust.scale;
              if ((double) num118 > 1.0)
                num118 = 1f;
              if (!dust.noLight)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num118 * 0.2f, num118 * 0.7f, num118 * 1f);
              if (dust.noGravity)
              {
                dust.velocity *= 0.93f;
                if ((double) dust.fadeIn == 0.0)
                  dust.scale += 1f / 400f;
              }
              dust.velocity *= new Vector2(0.97f, 0.99f);
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
              dust.scale -= 0.01f;
            }
            else if (dust.type == 272)
            {
              float num119 = dust.scale;
              if ((double) num119 > 1.0)
                num119 = 1f;
              if (!dust.noLight)
                Lighting.AddLight((int) ((double) dust.position.X / 16.0), (int) ((double) dust.position.Y / 16.0), num119 * 0.5f, num119 * 0.2f, num119 * 0.8f);
              if (dust.noGravity)
              {
                dust.velocity *= 0.93f;
                if ((double) dust.fadeIn == 0.0)
                  dust.scale += 1f / 400f;
              }
              dust.velocity *= new Vector2(0.97f, 0.99f);
              if (dust.customData != null && dust.customData is Player)
              {
                Player customData = (Player) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
              if (dust.customData != null && dust.customData is NPC)
              {
                NPC customData = (NPC) dust.customData;
                dust.position += customData.position - customData.oldPosition;
              }
              dust.scale -= 0.01f;
            }
            else if (dust.type != 304 && dust.noGravity)
            {
              dust.velocity *= 0.92f;
              if ((double) dust.fadeIn == 0.0)
                dust.scale -= 0.04f;
            }
            if ((double) dust.position.Y > (double) Main.screenPosition.Y + (double) Main.screenHeight)
              dust.active = false;
            float num120 = 0.1f;
            if ((double) Dust.dCount == 0.5)
              dust.scale -= 1f / 1000f;
            if ((double) Dust.dCount == 0.6)
              dust.scale -= 1f / 400f;
            if ((double) Dust.dCount == 0.7)
              dust.scale -= 0.005f;
            if ((double) Dust.dCount == 0.8)
              dust.scale -= 0.01f;
            if ((double) Dust.dCount == 0.9)
              dust.scale -= 0.02f;
            if ((double) Dust.dCount == 0.5)
              num120 = 0.11f;
            if ((double) Dust.dCount == 0.6)
              num120 = 0.13f;
            if ((double) Dust.dCount == 0.7)
              num120 = 0.16f;
            if ((double) Dust.dCount == 0.8)
              num120 = 0.22f;
            if ((double) Dust.dCount == 0.9)
              num120 = 0.25f;
            if ((double) dust.scale < (double) num120)
              dust.active = false;
          }
        }
        else
          dust.active = false;
      }
      int num121 = num1;
      if ((double) num121 > (double) Main.maxDustToDraw * 0.9)
        Dust.dCount = 0.9f;
      else if ((double) num121 > (double) Main.maxDustToDraw * 0.8)
        Dust.dCount = 0.8f;
      else if ((double) num121 > (double) Main.maxDustToDraw * 0.7)
        Dust.dCount = 0.7f;
      else if ((double) num121 > (double) Main.maxDustToDraw * 0.6)
        Dust.dCount = 0.6f;
      else if ((double) num121 > (double) Main.maxDustToDraw * 0.5)
        Dust.dCount = 0.5f;
      else
        Dust.dCount = 0.0f;
    }

    public Color GetAlpha(Color newColor)
    {
      float num1 = (float) ((int) byte.MaxValue - this.alpha) / (float) byte.MaxValue;
      switch (this.type)
      {
        case 299:
        case 300:
        case 301:
          Color alpha = new Color();
          switch (this.type)
          {
            case 299:
              alpha = new Color(50, (int) byte.MaxValue, 50, 200);
              break;
            case 300:
              alpha = new Color(50, 200, (int) byte.MaxValue, (int) byte.MaxValue);
              break;
            case 301:
              alpha = new Color((int) byte.MaxValue, 50, 125, 200);
              break;
            default:
              alpha = new Color((int) byte.MaxValue, 150, 150, 200);
              break;
          }
          return alpha;
        default:
          if (this.type == 304)
            return Color.White * num1;
          if (this.type == 292)
            return Color.White;
          if (this.type == 259)
            return new Color(230, 230, 230, 230);
          if (this.type == 261)
            return new Color(230, 230, 230, 115);
          if (this.type == 254 || this.type == (int) byte.MaxValue)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 258)
            return new Color(150, 50, 50, 0);
          if (this.type == 263 || this.type == 264)
            return new Color((int) this.color.R / 2 + (int) sbyte.MaxValue, (int) this.color.G + (int) sbyte.MaxValue, (int) this.color.B + (int) sbyte.MaxValue, (int) this.color.A / 8) * 0.5f;
          if (this.type == 235)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if ((this.type >= 86 && this.type <= 91 || this.type == 262 || this.type == 286) && !this.noLight)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 213 || this.type == 260)
          {
            int num2 = (int) ((double) this.scale / 2.5 * (double) byte.MaxValue);
            return new Color(num2, num2, num2, num2);
          }
          if (this.type == 64 && this.alpha == (int) byte.MaxValue && this.noLight)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 197)
            return new Color(250, 250, 250, 150);
          if (this.type >= 110 && this.type <= 114)
            return new Color(200, 200, 200, 0);
          if (this.type == 204)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 181)
            return new Color(200, 200, 200, 0);
          if (this.type == 182 || this.type == 206)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 159)
            return new Color(250, 250, 250, 50);
          if (this.type == 163 || this.type == 205)
            return new Color(250, 250, 250, 0);
          if (this.type == 170)
            return new Color(200, 200, 200, 100);
          if (this.type == 180)
            return new Color(200, 200, 200, 0);
          if (this.type == 175)
            return new Color(200, 200, 200, 0);
          if (this.type == 183)
            return new Color(50, 0, 0, 0);
          if (this.type == 172)
            return new Color(250, 250, 250, 150);
          if (this.type == 160 || this.type == 162 || this.type == 164 || this.type == 173)
          {
            int num3 = (int) (250.0 * (double) this.scale);
            return new Color(num3, num3, num3, 0);
          }
          if (this.type == 92 || this.type == 106 || this.type == 107)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 185)
            return new Color(200, 200, (int) byte.MaxValue, 125);
          if (this.type == (int) sbyte.MaxValue || this.type == 187)
            return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, 25);
          if (this.type == 156 || this.type == 230 || this.type == 234)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 270)
            return new Color((int) newColor.R / 2 + (int) sbyte.MaxValue, (int) newColor.G / 2 + (int) sbyte.MaxValue, (int) newColor.B / 2 + (int) sbyte.MaxValue, 25);
          if (this.type == 271)
            return new Color((int) newColor.R / 2 + (int) sbyte.MaxValue, (int) newColor.G / 2 + (int) sbyte.MaxValue, (int) newColor.B / 2 + (int) sbyte.MaxValue, (int) sbyte.MaxValue);
          if (this.type == 6 || this.type == 242 || this.type == 174 || this.type == 135 || this.type == 75 || this.type == 20 || this.type == 21 || this.type == 231 || this.type == 169 || this.type >= 130 && this.type <= 134 || this.type == 158 || this.type == 293 || this.type == 294 || this.type == 295 || this.type == 296 || this.type == 297 || this.type == 298)
            return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, 25);
          if (this.type == 278)
            return new Color(newColor.ToVector3() * this.color.ToVector3())
            {
              A = 25
            };
          if (this.type >= 219 && this.type <= 223)
          {
            newColor = Color.Lerp(newColor, Color.White, 0.5f);
            return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, 25);
          }
          if (this.type == 226 || this.type == 272)
          {
            newColor = Color.Lerp(newColor, Color.White, 0.8f);
            return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, 25);
          }
          if (this.type == 228)
          {
            newColor = Color.Lerp(newColor, Color.White, 0.8f);
            return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, 25);
          }
          if (this.type == 279)
          {
            int a = (int) newColor.A;
            newColor = Color.Lerp(newColor, Color.White, 0.8f);
            return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, a) * MathHelper.Min(this.scale, 1f);
          }
          if (this.type == 229 || this.type == 269)
          {
            newColor = Color.Lerp(newColor, Color.White, 0.6f);
            return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, 25);
          }
          if ((this.type == 68 || this.type == 70) && this.noGravity)
            return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
          if (this.type == 157)
          {
            int maxValue;
            int num4 = maxValue = (int) byte.MaxValue;
            int num5 = maxValue;
            int num6 = maxValue;
            float num7 = (float) ((double) Main.mouseTextColor / 100.0 - 1.60000002384186);
            int num8 = (int) ((double) num6 * (double) num7);
            int num9 = (int) ((double) num5 * (double) num7);
            int num10 = (int) ((double) num4 * (double) num7);
            int a = (int) (100.0 * (double) num7);
            int r = num8 + 50;
            if (r > (int) byte.MaxValue)
              r = (int) byte.MaxValue;
            int g = num9 + 50;
            if (g > (int) byte.MaxValue)
              g = (int) byte.MaxValue;
            int b = num10 + 50;
            if (b > (int) byte.MaxValue)
              b = (int) byte.MaxValue;
            return new Color(r, g, b, a);
          }
          if (this.type == 284)
            return new Color(newColor.ToVector4() * this.color.ToVector4())
            {
              A = this.color.A
            };
          if (this.type == 15 || this.type == 274 || this.type == 20 || this.type == 21 || this.type == 29 || this.type == 35 || this.type == 41 || this.type == 44 || this.type == 27 || this.type == 45 || this.type == 55 || this.type == 56 || this.type == 57 || this.type == 58 || this.type == 73 || this.type == 74)
            num1 = (float) (((double) num1 + 3.0) / 4.0);
          else if (this.type == 43)
          {
            num1 = (float) (((double) num1 + 9.0) / 10.0);
          }
          else
          {
            if (this.type >= 244 && this.type <= 247)
              return new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
            if (this.type == 66)
              return new Color((int) newColor.R, (int) newColor.G, (int) newColor.B, 0);
            if (this.type == 267)
              return new Color((int) this.color.R, (int) this.color.G, (int) this.color.B, 0);
            if (this.type == 71)
              return new Color(200, 200, 200, 0);
            if (this.type == 72)
              return new Color(200, 200, 200, 200);
          }
          int r1 = (int) ((double) newColor.R * (double) num1);
          int g1 = (int) ((double) newColor.G * (double) num1);
          int b1 = (int) ((double) newColor.B * (double) num1);
          int a1 = (int) newColor.A - this.alpha;
          if (a1 < 0)
            a1 = 0;
          if (a1 > (int) byte.MaxValue)
            a1 = (int) byte.MaxValue;
          return new Color(r1, g1, b1, a1);
      }
    }

    public Color GetColor(Color newColor)
    {
      if (this.type == 284)
        return Color.Transparent;
      int r = (int) this.color.R - ((int) byte.MaxValue - (int) newColor.R);
      int g = (int) this.color.G - ((int) byte.MaxValue - (int) newColor.G);
      int b = (int) this.color.B - ((int) byte.MaxValue - (int) newColor.B);
      int a = (int) this.color.A - ((int) byte.MaxValue - (int) newColor.A);
      if (r < 0)
        r = 0;
      if (r > (int) byte.MaxValue)
        r = (int) byte.MaxValue;
      if (g < 0)
        g = 0;
      if (g > (int) byte.MaxValue)
        g = (int) byte.MaxValue;
      if (b < 0)
        b = 0;
      if (b > (int) byte.MaxValue)
        b = (int) byte.MaxValue;
      if (a < 0)
        a = 0;
      if (a > (int) byte.MaxValue)
        a = (int) byte.MaxValue;
      return new Color(r, g, b, a);
    }

    public float GetVisualRotation() => this.type == 304 ? 0.0f : this.rotation;

    public float GetVisualScale() => this.type == 304 ? 1f : this.scale;
  }
}
