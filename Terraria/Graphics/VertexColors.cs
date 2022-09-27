// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.VertexColors
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;

namespace Terraria.Graphics
{
  public struct VertexColors
  {
    public Color TopLeftColor;
    public Color TopRightColor;
    public Color BottomLeftColor;
    public Color BottomRightColor;

    public VertexColors(Color color)
    {
      this.TopLeftColor = color;
      this.TopRightColor = color;
      this.BottomRightColor = color;
      this.BottomLeftColor = color;
    }

    public VertexColors(Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
    {
      this.TopLeftColor = topLeft;
      this.TopRightColor = topRight;
      this.BottomLeftColor = bottomLeft;
      this.BottomRightColor = bottomRight;
    }
  }
}
