// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.NPCStrengthHelper
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

namespace Terraria.DataStructures
{
  public struct NPCStrengthHelper
  {
    private float _strength;
    private GameModeData _gameModeData;

    public bool IsExpertMode => (double) this._strength >= 2.0 || this._gameModeData.IsExpertMode;

    public bool IsMasterMode => (double) this._strength >= 3.0 || this._gameModeData.IsMasterMode;

    public NPCStrengthHelper(GameModeData data, float strength)
    {
      this._strength = strength;
      this._gameModeData = data;
    }
  }
}
