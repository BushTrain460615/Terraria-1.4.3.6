// Decompiled with JetBrains decompiler
// Type: Terraria.Modules.AnchorTypesModule
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System;

namespace Terraria.Modules
{
  public class AnchorTypesModule
  {
    public int[] tileValid;
    public int[] tileInvalid;
    public int[] tileAlternates;
    public int[] wallValid;

    public AnchorTypesModule(AnchorTypesModule copyFrom = null)
    {
      if (copyFrom == null)
      {
        this.tileValid = (int[]) null;
        this.tileInvalid = (int[]) null;
        this.tileAlternates = (int[]) null;
        this.wallValid = (int[]) null;
      }
      else
      {
        if (copyFrom.tileValid == null)
        {
          this.tileValid = (int[]) null;
        }
        else
        {
          this.tileValid = new int[copyFrom.tileValid.Length];
          Array.Copy((Array) copyFrom.tileValid, (Array) this.tileValid, this.tileValid.Length);
        }
        if (copyFrom.tileInvalid == null)
        {
          this.tileInvalid = (int[]) null;
        }
        else
        {
          this.tileInvalid = new int[copyFrom.tileInvalid.Length];
          Array.Copy((Array) copyFrom.tileInvalid, (Array) this.tileInvalid, this.tileInvalid.Length);
        }
        if (copyFrom.tileAlternates == null)
        {
          this.tileAlternates = (int[]) null;
        }
        else
        {
          this.tileAlternates = new int[copyFrom.tileAlternates.Length];
          Array.Copy((Array) copyFrom.tileAlternates, (Array) this.tileAlternates, this.tileAlternates.Length);
        }
        if (copyFrom.wallValid == null)
        {
          this.wallValid = (int[]) null;
        }
        else
        {
          this.wallValid = new int[copyFrom.wallValid.Length];
          Array.Copy((Array) copyFrom.wallValid, (Array) this.wallValid, this.wallValid.Length);
        }
      }
    }
  }
}
