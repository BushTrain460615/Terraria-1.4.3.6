﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Map.MapTile
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.Map
{
  public struct MapTile
  {
    public ushort Type;
    public byte Light;
    private byte _extraData;

    public bool IsChanged
    {
      get => ((int) this._extraData & 128) == 128;
      set
      {
        if (value)
          this._extraData |= (byte) 128;
        else
          this._extraData &= (byte) 127;
      }
    }

    public byte Color
    {
      get => (byte) ((uint) this._extraData & (uint) sbyte.MaxValue);
      set => this._extraData = (byte) ((int) this._extraData & 128 | (int) value & (int) sbyte.MaxValue);
    }

    private MapTile(ushort type, byte light, byte extraData)
    {
      this.Type = type;
      this.Light = light;
      this._extraData = extraData;
    }

    public bool Equals(ref MapTile other) => (int) this.Light == (int) other.Light && (int) this.Type == (int) other.Type && (int) this.Color == (int) other.Color;

    public bool EqualsWithoutLight(ref MapTile other) => (int) this.Type == (int) other.Type && (int) this.Color == (int) other.Color;

    public void Clear()
    {
      this.Type = (ushort) 0;
      this.Light = (byte) 0;
      this._extraData = (byte) 0;
    }

    public MapTile WithLight(byte light) => new MapTile(this.Type, light, (byte) ((uint) this._extraData | 128U));

    public static MapTile Create(ushort type, byte light, byte color) => new MapTile(type, light, (byte) ((uint) color | 128U));
  }
}
