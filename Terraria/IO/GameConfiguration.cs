// Decompiled with JetBrains decompiler
// Type: Terraria.IO.GameConfiguration
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Newtonsoft.Json.Linq;

namespace Terraria.IO
{
  public class GameConfiguration
  {
    private readonly JObject _root;

    public GameConfiguration(JObject configurationRoot) => this._root = configurationRoot;

    public T Get<T>(string entry) => this._root[entry].ToObject<T>();
  }
}
