// Decompiled with JetBrains decompiler
// Type: Terraria.Graphics.Renderers.ParticlePool`1
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;

namespace Terraria.Graphics.Renderers
{
  public class ParticlePool<T> where T : IPooledParticle
  {
    private ParticlePool<T>.ParticleInstantiator _instantiator;
    private List<T> _particles;

    public ParticlePool(int initialPoolSize, ParticlePool<T>.ParticleInstantiator instantiator)
    {
      this._particles = new List<T>(initialPoolSize);
      this._instantiator = instantiator;
    }

    public T RequestParticle()
    {
      int count = this._particles.Count;
      for (int index = 0; index < count; ++index)
      {
        T particle = this._particles[index];
        if (particle.IsRestingInPool)
        {
          particle = this._particles[index];
          particle.FetchFromPool();
          return this._particles[index];
        }
      }
      T obj = this._instantiator();
      this._particles.Add(obj);
      obj.FetchFromPool();
      return obj;
    }

    public delegate T ParticleInstantiator() where T : IPooledParticle;
  }
}
