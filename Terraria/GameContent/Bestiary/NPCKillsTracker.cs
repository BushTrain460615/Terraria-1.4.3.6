// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Bestiary.NPCKillsTracker
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Bestiary
{
  public class NPCKillsTracker : IPersistentPerWorldContent, IOnPlayerJoining
  {
    private object _entryCreationLock = new object();
    public const int POSITIVE_KILL_COUNT_CAP = 9999;
    private Dictionary<string, int> _killCountsByNpcId;

    public NPCKillsTracker() => this._killCountsByNpcId = new Dictionary<string, int>();

    public void RegisterKill(NPC npc)
    {
      string bestiaryCreditId = npc.GetBestiaryCreditId();
      int num;
      this._killCountsByNpcId.TryGetValue(bestiaryCreditId, out num);
      int killcount = num + 1;
      lock (this._entryCreationLock)
        this._killCountsByNpcId[bestiaryCreditId] = Utils.Clamp<int>(killcount, 0, 9999);
      if (Main.netMode != 2)
        return;
      NetManager.Instance.Broadcast(NetBestiaryModule.SerializeKillCount(npc.netID, killcount));
    }

    public int GetKillCount(NPC npc) => this.GetKillCount(npc.GetBestiaryCreditId());

    public void SetKillCountDirectly(string persistentId, int killCount)
    {
      lock (this._entryCreationLock)
        this._killCountsByNpcId[persistentId] = Utils.Clamp<int>(killCount, 0, 9999);
    }

    public int GetKillCount(string persistentId)
    {
      int killCount;
      this._killCountsByNpcId.TryGetValue(persistentId, out killCount);
      return killCount;
    }

    public void Save(BinaryWriter writer)
    {
      lock (this._killCountsByNpcId)
      {
        writer.Write(this._killCountsByNpcId.Count);
        foreach (KeyValuePair<string, int> keyValuePair in this._killCountsByNpcId)
        {
          writer.Write(keyValuePair.Key);
          writer.Write(keyValuePair.Value);
        }
      }
    }

    public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
    {
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
        this._killCountsByNpcId[reader.ReadString()] = reader.ReadInt32();
    }

    public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
    {
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
      {
        reader.ReadString();
        reader.ReadInt32();
      }
    }

    public void Reset() => this._killCountsByNpcId.Clear();

    public void OnPlayerJoining(int playerIndex)
    {
      foreach (KeyValuePair<string, int> keyValuePair in this._killCountsByNpcId)
      {
        int idsByPersistentId = ContentSamples.NpcNetIdsByPersistentIds[keyValuePair.Key];
        NetManager.Instance.SendToClient(NetBestiaryModule.SerializeKillCount(idsByPersistentId, keyValuePair.Value), playerIndex);
      }
    }
  }
}
