// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Drawing.ParticleOrchestraSettings
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.IO;

namespace Terraria.GameContent.Drawing
{
  public struct ParticleOrchestraSettings
  {
    public Vector2 PositionInWorld;
    public Vector2 MovementVector;
    public int PackedShaderIndex;
    public byte IndexOfPlayerWhoInvokedThis;
    public const int SerializationSize = 21;

    public void Serialize(BinaryWriter writer)
    {
      writer.WriteVector2(this.PositionInWorld);
      writer.WriteVector2(this.MovementVector);
      writer.Write(this.PackedShaderIndex);
      writer.Write(this.IndexOfPlayerWhoInvokedThis);
    }

    public void DeserializeFrom(BinaryReader reader)
    {
      this.PositionInWorld = reader.ReadVector2();
      this.MovementVector = reader.ReadVector2();
      this.PackedShaderIndex = reader.ReadInt32();
      this.IndexOfPlayerWhoInvokedThis = reader.ReadByte();
    }
  }
}
