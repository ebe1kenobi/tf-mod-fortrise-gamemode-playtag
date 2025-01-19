using System;
using System.Collections.Generic;
using System.Linq;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MySession// : Session
  {
    public static int NbPlayTagPickupActivated { get; set; }

    internal static void Load()
    {
      On.TowerFall.Session.StartGame += StartGame_patch;
      //On.TowerFall.Session.StartGame += LevelLoadStart_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Session.StartGame -= StartGame_patch;
      //On.TowerFall.Session.StartGame -= LevelLoadStart_patch;
    }
    public MySession()
    {
    }

    //public bool IsInOvertime //TODO override ?
    //{
    //  get
    //  {
    //    if (this.RoundIndex >= 0 && (this.MatchSettings.Mode == Modes.HeadHunters || this.MatchSettings.Mode == Modes.PlayTag))
    //    {
    //      for (int index = 0; index < this.Scores.Length; ++index)
    //      {
    //        if (this.Scores[index] >= this.MatchSettings.GoalScore)
    //          return true;
    //      }
    //    }
    //    return false;
    //  }
    //}

    
    //public static void LevelLoadStart_patch(On.TowerFall.Session.orig_StartGame orig, global::TowerFall.Session self)
    //{
    //  NbPlayTagPickupActivated = 0;
    //  Logger.Info("LevelLoadStart_patch MySession.NbPlayTagPickupActivated = " + MySession.NbPlayTagPickupActivated);
    //  orig(self);
    //}
    public static void StartGame_patch(On.TowerFall.Session.orig_StartGame orig, global::TowerFall.Session self)
    {
      NbPlayTagPickupActivated = 0;
      Logger.Info("StartGame_patch MySession.NbPlayTagPickupActivated = " + MySession.NbPlayTagPickupActivated);
      orig(self);
    }
  }
}
