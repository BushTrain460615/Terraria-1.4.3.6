// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Effects.EffectManager`1
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Terraria.Graphics.Effects
{
  public abstract class EffectManager<T> where T : GameEffect
  {
    protected bool _isLoaded;
    protected Dictionary<string, T> _effects = new Dictionary<string, T>();

    public bool IsLoaded => this._isLoaded;

    public T this[string key]
    {
      get
      {
        T obj;
        return this._effects.TryGetValue(key, out obj) ? obj : default (T);
      }
      set => this.Bind(key, value);
    }

    public void Bind(string name, T effect)
    {
      this._effects[name] = effect;
      if (!this._isLoaded)
        return;
      effect.Load();
    }

    public void Load()
    {
      if (this._isLoaded)
        return;
      this._isLoaded = true;
      foreach (T obj in this._effects.Values)
        obj.Load();
    }

    public T Activate(string name, Vector2 position = default (Vector2), params object[] args)
    {
      T effect = this._effects.ContainsKey(name) ? this._effects[name] : throw new MissingEffectException("Unable to find effect named: " + name + ". Type: " + typeof (T)?.ToString() + ".");
      this.OnActivate(effect, position);
      effect.Activate(position, args);
      return effect;
    }

    public void Deactivate(string name, params object[] args)
    {
      T effect = this._effects.ContainsKey(name) ? this._effects[name] : throw new MissingEffectException("Unable to find effect named: " + name + ". Type: " + typeof (T)?.ToString() + ".");
      this.OnDeactivate(effect);
      effect.Deactivate(args);
    }

    public virtual void OnActivate(T effect, Vector2 position)
    {
    }

    public virtual void OnDeactivate(T effect)
    {
    }
  }
}
