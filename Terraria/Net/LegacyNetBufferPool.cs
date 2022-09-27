// Decompiled with JetBrains decompiler
// Type: Terraria.Net.LegacyNetBufferPool
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;
using System.Collections.Generic;

namespace Terraria.Net
{
  public class LegacyNetBufferPool
  {
    private const int SMALL_BUFFER_SIZE = 256;
    private const int MEDIUM_BUFFER_SIZE = 1024;
    private const int LARGE_BUFFER_SIZE = 16384;
    private static object bufferLock = new object();
    private static Queue<byte[]> _smallBufferQueue = new Queue<byte[]>();
    private static Queue<byte[]> _mediumBufferQueue = new Queue<byte[]>();
    private static Queue<byte[]> _largeBufferQueue = new Queue<byte[]>();
    private static int _smallBufferCount;
    private static int _mediumBufferCount;
    private static int _largeBufferCount;
    private static int _customBufferCount;

    public static byte[] RequestBuffer(int size)
    {
      lock (LegacyNetBufferPool.bufferLock)
      {
        if (size <= 256)
        {
          if (LegacyNetBufferPool._smallBufferQueue.Count != 0)
            return LegacyNetBufferPool._smallBufferQueue.Dequeue();
          ++LegacyNetBufferPool._smallBufferCount;
          return new byte[256];
        }
        if (size <= 1024)
        {
          if (LegacyNetBufferPool._mediumBufferQueue.Count != 0)
            return LegacyNetBufferPool._mediumBufferQueue.Dequeue();
          ++LegacyNetBufferPool._mediumBufferCount;
          return new byte[1024];
        }
        if (size <= 16384)
        {
          if (LegacyNetBufferPool._largeBufferQueue.Count != 0)
            return LegacyNetBufferPool._largeBufferQueue.Dequeue();
          ++LegacyNetBufferPool._largeBufferCount;
          return new byte[16384];
        }
        ++LegacyNetBufferPool._customBufferCount;
        return new byte[size];
      }
    }

    public static byte[] RequestBuffer(byte[] data, int offset, int size)
    {
      byte[] dst = LegacyNetBufferPool.RequestBuffer(size);
      Buffer.BlockCopy((Array) data, offset, (Array) dst, 0, size);
      return dst;
    }

    public static void ReturnBuffer(byte[] buffer)
    {
      int length = buffer.Length;
      lock (LegacyNetBufferPool.bufferLock)
      {
        if (length <= 256)
          LegacyNetBufferPool._smallBufferQueue.Enqueue(buffer);
        else if (length <= 1024)
        {
          LegacyNetBufferPool._mediumBufferQueue.Enqueue(buffer);
        }
        else
        {
          if (length > 16384)
            return;
          LegacyNetBufferPool._largeBufferQueue.Enqueue(buffer);
        }
      }
    }

    public static void DisplayBufferSizes()
    {
      lock (LegacyNetBufferPool.bufferLock)
      {
        int count = LegacyNetBufferPool._smallBufferQueue.Count;
        Main.NewText("Small Buffers:  " + count.ToString() + " queued of " + LegacyNetBufferPool._smallBufferCount.ToString());
        count = LegacyNetBufferPool._mediumBufferQueue.Count;
        Main.NewText("Medium Buffers: " + count.ToString() + " queued of " + LegacyNetBufferPool._mediumBufferCount.ToString());
        count = LegacyNetBufferPool._largeBufferQueue.Count;
        Main.NewText("Large Buffers:  " + count.ToString() + " queued of " + LegacyNetBufferPool._largeBufferCount.ToString());
        Main.NewText("Custom Buffers: 0 queued of " + LegacyNetBufferPool._customBufferCount.ToString());
      }
    }

    public static void PrintBufferSizes()
    {
      lock (LegacyNetBufferPool.bufferLock)
      {
        int count = LegacyNetBufferPool._smallBufferQueue.Count;
        Console.WriteLine("Small Buffers:  " + count.ToString() + " queued of " + LegacyNetBufferPool._smallBufferCount.ToString());
        count = LegacyNetBufferPool._mediumBufferQueue.Count;
        Console.WriteLine("Medium Buffers: " + count.ToString() + " queued of " + LegacyNetBufferPool._mediumBufferCount.ToString());
        count = LegacyNetBufferPool._largeBufferQueue.Count;
        Console.WriteLine("Large Buffers:  " + count.ToString() + " queued of " + LegacyNetBufferPool._largeBufferCount.ToString());
        Console.WriteLine("Custom Buffers: 0 queued of " + LegacyNetBufferPool._customBufferCount.ToString());
        Console.WriteLine("");
      }
    }
  }
}
