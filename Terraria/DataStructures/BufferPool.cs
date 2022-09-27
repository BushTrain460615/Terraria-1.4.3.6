// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.BufferPool
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
  public static class BufferPool
  {
    private const int SMALL_BUFFER_SIZE = 32;
    private const int MEDIUM_BUFFER_SIZE = 256;
    private const int LARGE_BUFFER_SIZE = 16384;
    private static object bufferLock = new object();
    private static Queue<CachedBuffer> SmallBufferQueue = new Queue<CachedBuffer>();
    private static Queue<CachedBuffer> MediumBufferQueue = new Queue<CachedBuffer>();
    private static Queue<CachedBuffer> LargeBufferQueue = new Queue<CachedBuffer>();

    public static CachedBuffer Request(int size)
    {
      lock (BufferPool.bufferLock)
      {
        if (size <= 32)
          return BufferPool.SmallBufferQueue.Count == 0 ? new CachedBuffer(new byte[32]) : BufferPool.SmallBufferQueue.Dequeue().Activate();
        if (size <= 256)
          return BufferPool.MediumBufferQueue.Count == 0 ? new CachedBuffer(new byte[256]) : BufferPool.MediumBufferQueue.Dequeue().Activate();
        if (size > 16384)
          return new CachedBuffer(new byte[size]);
        return BufferPool.LargeBufferQueue.Count == 0 ? new CachedBuffer(new byte[16384]) : BufferPool.LargeBufferQueue.Dequeue().Activate();
      }
    }

    public static CachedBuffer Request(byte[] data, int offset, int size)
    {
      CachedBuffer cachedBuffer = BufferPool.Request(size);
      Buffer.BlockCopy((Array) data, offset, (Array) cachedBuffer.Data, 0, size);
      return cachedBuffer;
    }

    public static void Recycle(CachedBuffer buffer)
    {
      int length = buffer.Length;
      lock (BufferPool.bufferLock)
      {
        if (length <= 32)
          BufferPool.SmallBufferQueue.Enqueue(buffer);
        else if (length <= 256)
        {
          BufferPool.MediumBufferQueue.Enqueue(buffer);
        }
        else
        {
          if (length > 16384)
            return;
          BufferPool.LargeBufferQueue.Enqueue(buffer);
        }
      }
    }

    public static void PrintBufferSizes()
    {
      lock (BufferPool.bufferLock)
      {
        int count = BufferPool.SmallBufferQueue.Count;
        Console.WriteLine("SmallBufferQueue.Count: " + count.ToString());
        count = BufferPool.MediumBufferQueue.Count;
        Console.WriteLine("MediumBufferQueue.Count: " + count.ToString());
        count = BufferPool.LargeBufferQueue.Count;
        Console.WriteLine("LargeBufferQueue.Count: " + count.ToString());
        Console.WriteLine("");
      }
    }
  }
}
