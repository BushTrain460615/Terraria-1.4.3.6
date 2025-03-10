﻿// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Shaders.ArmorShaderDataSet
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
  public class ArmorShaderDataSet
  {
    protected List<ArmorShaderData> _shaderData = new List<ArmorShaderData>();
    protected Dictionary<int, int> _shaderLookupDictionary = new Dictionary<int, int>();
    protected int _shaderDataCount;

    public T BindShader<T>(int itemId, T shaderData) where T : ArmorShaderData
    {
      this._shaderLookupDictionary[itemId] = ++this._shaderDataCount;
      this._shaderData.Add((ArmorShaderData) shaderData);
      return shaderData;
    }

    public void Apply(int shaderId, Entity entity, DrawData? drawData = null)
    {
      if (shaderId >= 1 && shaderId <= this._shaderDataCount)
        this._shaderData[shaderId - 1].Apply(entity, drawData);
      else
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    public void ApplySecondary(int shaderId, Entity entity, DrawData? drawData = null)
    {
      if (shaderId >= 1 && shaderId <= this._shaderDataCount)
        this._shaderData[shaderId - 1].GetSecondaryShader(entity).Apply(entity, drawData);
      else
        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
    }

    public ArmorShaderData GetShaderFromItemId(int type) => this._shaderLookupDictionary.ContainsKey(type) ? this._shaderData[this._shaderLookupDictionary[type] - 1] : (ArmorShaderData) null;

    public int GetShaderIdFromItemId(int type) => this._shaderLookupDictionary.ContainsKey(type) ? this._shaderLookupDictionary[type] : 0;

    public ArmorShaderData GetSecondaryShader(int id, Player player) => id != 0 && id <= this._shaderDataCount && this._shaderData[id - 1] != null ? this._shaderData[id - 1].GetSecondaryShader((Entity) player) : (ArmorShaderData) null;
  }
}
