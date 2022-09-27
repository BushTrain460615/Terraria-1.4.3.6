// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.TileDrawInfo
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
  public class TileDrawInfo
  {
    public Tile tileCache;
    public ushort typeCache;
    public short tileFrameX;
    public short tileFrameY;
    public Texture2D drawTexture;
    public Color tileLight;
    public int tileTop;
    public int tileWidth;
    public int tileHeight;
    public int halfBrickHeight;
    public int addFrY;
    public int addFrX;
    public SpriteEffects tileSpriteEffect;
    public Texture2D glowTexture;
    public Rectangle glowSourceRect;
    public Color glowColor;
    public Vector3[] colorSlices = new Vector3[9];
    public Color finalColor;
    public Color colorTint;
  }
}
