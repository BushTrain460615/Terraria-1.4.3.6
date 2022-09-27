// Decompiled with JetBrains decompiler
// Type: Terraria.GameContent.Achievements.ProgressionEventCondition
// Assembly: Terraria, Version=1.4.3.6, Culture=neutral, PublicKeyToken=null
// MVID: F541F3E5-89DE-4E5D-868F-1B56DAAB46B2
// Assembly location: D:\SteamLibrary\steamapps\common\Terraria\Terraria.exe

using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
  public class ProgressionEventCondition : AchievementCondition
  {
    private const string Identifier = "PROGRESSION_EVENT";
    private static Dictionary<int, List<ProgressionEventCondition>> _listeners = new Dictionary<int, List<ProgressionEventCondition>>();
    private static bool _isListenerHooked;
    private int[] _eventIDs;

    private ProgressionEventCondition(int eventID)
      : base("PROGRESSION_EVENT_" + eventID.ToString())
    {
      this._eventIDs = new int[1]{ eventID };
      ProgressionEventCondition.ListenForPickup(this);
    }

    private ProgressionEventCondition(int[] eventIDs)
      : base("PROGRESSION_EVENT_" + eventIDs[0].ToString())
    {
      this._eventIDs = eventIDs;
      ProgressionEventCondition.ListenForPickup(this);
    }

    private static void ListenForPickup(ProgressionEventCondition condition)
    {
      if (!ProgressionEventCondition._isListenerHooked)
      {
        AchievementsHelper.OnProgressionEvent += new AchievementsHelper.ProgressionEventEvent(ProgressionEventCondition.ProgressionEventListener);
        ProgressionEventCondition._isListenerHooked = true;
      }
      for (int index = 0; index < condition._eventIDs.Length; ++index)
      {
        if (!ProgressionEventCondition._listeners.ContainsKey(condition._eventIDs[index]))
          ProgressionEventCondition._listeners[condition._eventIDs[index]] = new List<ProgressionEventCondition>();
        ProgressionEventCondition._listeners[condition._eventIDs[index]].Add(condition);
      }
    }

    private static void ProgressionEventListener(int eventID)
    {
      if (!ProgressionEventCondition._listeners.ContainsKey(eventID))
        return;
      foreach (AchievementCondition achievementCondition in ProgressionEventCondition._listeners[eventID])
        achievementCondition.Complete();
    }

    public static ProgressionEventCondition Create(params int[] eventIDs) => new ProgressionEventCondition(eventIDs);

    public static ProgressionEventCondition Create(int eventID) => new ProgressionEventCondition(eventID);

    public static ProgressionEventCondition[] CreateMany(
      params int[] eventIDs)
    {
      ProgressionEventCondition[] many = new ProgressionEventCondition[eventIDs.Length];
      for (int index = 0; index < eventIDs.Length; ++index)
        many[index] = new ProgressionEventCondition(eventIDs[index]);
      return many;
    }
  }
}
