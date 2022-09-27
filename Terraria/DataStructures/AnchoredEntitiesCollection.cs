// Decompiled with JetBrains decompiler
// Type: Terraria.DataStructures.AnchoredEntitiesCollection
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
  public class AnchoredEntitiesCollection
  {
    private List<AnchoredEntitiesCollection.IndexPointPair> _anchoredNPCs;
    private List<AnchoredEntitiesCollection.IndexPointPair> _anchoredPlayers;

    public int AnchoredPlayersAmount => this._anchoredPlayers.Count;

    public AnchoredEntitiesCollection()
    {
      this._anchoredNPCs = new List<AnchoredEntitiesCollection.IndexPointPair>();
      this._anchoredPlayers = new List<AnchoredEntitiesCollection.IndexPointPair>();
    }

    public void ClearNPCAnchors() => this._anchoredNPCs.Clear();

    public void ClearPlayerAnchors() => this._anchoredPlayers.Clear();

    public void AddNPC(int npcIndex, Point coords) => this._anchoredNPCs.Add(new AnchoredEntitiesCollection.IndexPointPair()
    {
      index = npcIndex,
      coords = coords
    });

    public int GetNextPlayerStackIndexInCoords(Point coords) => this.GetEntitiesInCoords(coords);

    public void AddPlayerAndGetItsStackedIndexInCoords(
      int playerIndex,
      Point coords,
      out int stackedIndexInCoords)
    {
      stackedIndexInCoords = this.GetEntitiesInCoords(coords);
      this._anchoredPlayers.Add(new AnchoredEntitiesCollection.IndexPointPair()
      {
        index = playerIndex,
        coords = coords
      });
    }

    private int GetEntitiesInCoords(Point coords)
    {
      int entitiesInCoords = 0;
      for (int index = 0; index < this._anchoredNPCs.Count; ++index)
      {
        if (this._anchoredNPCs[index].coords == coords)
          ++entitiesInCoords;
      }
      for (int index = 0; index < this._anchoredPlayers.Count; ++index)
      {
        if (this._anchoredPlayers[index].coords == coords)
          ++entitiesInCoords;
      }
      return entitiesInCoords;
    }

    private struct IndexPointPair
    {
      public int index;
      public Point coords;
    }
  }
}
