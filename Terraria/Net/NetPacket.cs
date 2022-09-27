// Decompiled with JetBrains decompiler
// Type: Terraria.Net.NetPacket
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.IO;
using Terraria.DataStructures;

namespace Terraria.Net
{
  public struct NetPacket
  {
    private const int HEADER_SIZE = 5;
    public readonly ushort Id;
    public readonly CachedBuffer Buffer;

    public int Length { get; private set; }

    public BinaryWriter Writer => this.Buffer.Writer;

    public BinaryReader Reader => this.Buffer.Reader;

    public NetPacket(ushort id, int size)
      : this()
    {
      this.Id = id;
      this.Buffer = BufferPool.Request(size + 5);
      this.Length = size + 5;
      this.Writer.Write((ushort) (size + 5));
      this.Writer.Write((byte) 82);
      this.Writer.Write(id);
    }

    public void Recycle() => this.Buffer.Recycle();

    public void ShrinkToFit()
    {
      if (this.Length == (int) this.Writer.BaseStream.Position)
        return;
      this.Length = (int) this.Writer.BaseStream.Position;
      this.Writer.Seek(0, SeekOrigin.Begin);
      this.Writer.Write((ushort) this.Length);
      this.Writer.Seek(this.Length, SeekOrigin.Begin);
    }
  }
}
