using System;
using System.Collections.Generic;
using System.Linq;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyPlayer : Player
  {
    //public static Monocle.Collider[] wasColliders = new Monocle.Collider[4];
    internal static void Load()
    {
      On.TowerFall.Player.ctor += ctor_patch;
      On.TowerFall.Player.HUDRender += HUDRender_patch;
      On.TowerFall.Player.PlayerOnPlayer += PlayerOnPlayer_patch;
      On.TowerFall.Player.HurtBouncedOn += HurtBouncedOn_patch;
      On.TowerFall.Player.Update += Update;
    }

    internal static void Unload()
    {
      On.TowerFall.Player.ctor -= ctor_patch;
      On.TowerFall.Player.HUDRender -= HUDRender_patch;
      On.TowerFall.Player.PlayerOnPlayer -= PlayerOnPlayer_patch;
      On.TowerFall.Player.HurtBouncedOn -= HurtBouncedOn_patch;
      On.TowerFall.Player.Update -= Update;
    }

    // Play Tag var
    //public bool playTag = false;
    public static Dictionary<int, bool> playTag = new Dictionary<int, bool>(8);
    //public PlayTagHUD PlayTagHUD;
    public static Dictionary<int, PlayTagHUD> PlayTagHUD = new Dictionary<int, PlayTagHUD>(8);
    //public int playTagDelay = 10;
    public static Dictionary<int, int> playTagDelay = new Dictionary<int, int>(8);
    //public int playTagDelayModePlayTag = 15;
    public static Dictionary<int, int> playTagDelayModePlayTag = new Dictionary<int, int>(8);
    //public int playTagCountDown = 0;
    public static Dictionary<int, int> playTagCountDown = new Dictionary<int, int>(8);
    //public int previousPlayTagCountDown = 0;
    public static Dictionary<int, int> previousPlayTagCountDown = new Dictionary<int, int>(8);
    //public bool playTagCountDownOn = false;
    public static Dictionary<int, bool> playTagCountDownOn = new Dictionary<int, bool>(8);
    //public readonly DateTime creationTime;
    public static Dictionary<int, DateTime> creationTime = new Dictionary<int, DateTime>(8);
    //public int pauseDuration = 0;
    public static Dictionary<int, int> pauseDuration = new Dictionary<int, int>(8);
    // End Play Tag var

    public MyPlayer(
      int playerIndex,
      Vector2 position,
      Allegiance allegiance,
      Allegiance teamColor,
      PlayerInventory inventory,
      Player.HatStates hatState,
      bool frozen,
      bool flash,
      bool indicator)
      : base(playerIndex, position, allegiance, teamColor, inventory, hatState, frozen, flash, indicator)
    {
      //jamais appelé ?
      //Logger.Init("ttttttttttttttttttttttttttMyPlayer");
      //Logger.Info("MyPlayer" + playerIndex);
    }

    public static void ctor_patch(On.TowerFall.Player.orig_ctor orig, TowerFall.Player self, int playerIndex, Vector2 position, Allegiance allegiance, Allegiance teamColor, global::TowerFall.PlayerInventory inventory, global::TowerFall.Player.HatStates hatState, bool frozen, bool flash, bool indicator) {
      Logger.Init("ttttttttttttttttttttttttttctor_patch");
      Logger.Info("ctor_patch" + playerIndex);
      //todo log playerindex, est-Center que le code est  appelé ?
      orig(self, playerIndex, position, allegiance, teamColor, inventory, hatState, frozen, flash, indicator);
      MyPlayer.PlayTagHUD[playerIndex] = new PlayTagHUD();
      self.Add((Monocle.Component)(MyPlayer.PlayTagHUD[playerIndex]));
      MyPlayer.playTag[playerIndex] = false;
      MyPlayer.playTagDelay[playerIndex] = 10;
      MyPlayer.playTagDelayModePlayTag[playerIndex] = 15;
      MyPlayer.previousPlayTagCountDown[playerIndex] = 0;
      MyPlayer.playTagCountDown[playerIndex] = 0;
      MyPlayer.playTagCountDownOn[playerIndex] = false;
      MyPlayer.creationTime[playerIndex] = DateTime.Now;
      MyPlayer.pauseDuration[playerIndex] = 0;

      //Logger.Init("ttttttttttttttttttttttttttctor_patch");
      //Logger.Info("PlayTagHUD.count " + MyPlayer.PlayTagHUD.Count);

      //Logger.Info("MyPlayer.PlayTagHUD " + MyPlayer.PlayTagHUD.ToList());
      //Logger.Info("MyPlayer.playTag " + MyPlayer.playTag.ToList());
      //Logger.Info("MyPlayer.playTagDelay " + MyPlayer.playTagDelay.ToList());
      //Logger.Info("MyPlayer.playTagDelayModePlayTag " + MyPlayer.playTagDelayModePlayTag.ToList());
      //Logger.Info("MyPlayer.playTagCountDown " + MyPlayer.playTagCountDown.ToList());
      //Logger.Info("MyPlayer.playTagCountDownOn " + MyPlayer.playTagCountDownOn.ToList());
      //Logger.Info("MyPlayer.creationTime " + MyPlayer.creationTime.ToList());
      //Logger.Info("MyPlayer.pauseDuration " + MyPlayer.pauseDuration.ToList());
    }

    public static void PlayerOnPlayer_patch(On.TowerFall.Player.orig_PlayerOnPlayer orig, Player a, Player b)
    {
      orig(a, b);
      //if (a.playTag && !b.HasShield)
      if (MyPlayer.playTag[a.PlayerIndex] && !b.HasShield)
      {
        //b.playTag = true;
        MyPlayer.playTag[b.PlayerIndex] = true;
        //b.playTagCountDown = a.playTagCountDown;
        MyPlayer.playTagCountDown[b.PlayerIndex] = MyPlayer.playTagCountDown[a.PlayerIndex];
        //b.creationTime = a.creationTime;
        MyPlayer.creationTime[b.PlayerIndex] = MyPlayer.creationTime[a.PlayerIndex];

        //a.playTag = false;
        MyPlayer.playTag[a.PlayerIndex] = false;
      }
      //else if (b.playTag && !a.HasShield)
      else if (MyPlayer.playTag[b.PlayerIndex] && !a.HasShield)
      {
        //a.playTag = true;
        MyPlayer.playTag[a.PlayerIndex] = true;
        //a.playTagCountDownOn = true;
        MyPlayer.playTagCountDownOn[a.PlayerIndex] = true;
        //a.playTagCountDown = b.playTagCountDown;
        MyPlayer.playTagCountDown[a.PlayerIndex] = MyPlayer.playTagCountDown[b.PlayerIndex];
        //a.creationTime = b.creationTime;
        MyPlayer.creationTime[a.PlayerIndex] = MyPlayer.creationTime[b.PlayerIndex];

        //b.playTag = false;
        MyPlayer.playTag[b.PlayerIndex] = false;
      }
    }
    public static void HUDRender_patch(On.TowerFall.Player.orig_HUDRender orig, TowerFall.Player self, bool wrapped)
    {
      //Logger.Init("tttttttttttttttttttttttttHUDRender_patch");
      //Logger.Info("self.PlayerIndex = " + self.PlayerIndex);
      //Logger.Info("MyPlayer.playTag " + MyPlayer.playTag.Count);
      //Logger.Info("MyPlayer.playTag " + MyPlayer.playTag.ToList());
      //Logger.Info("MyPlayer.playTagDelay " + MyPlayer.playTagDelay.Count);
      //Logger.Info("MyPlayer.playTagDelay " + MyPlayer.playTagDelay.ToList());
      //Logger.Info("MyPlayer.playTagDelayModePlayTag " + MyPlayer.playTagDelayModePlayTag.Count);
      //Logger.Info("MyPlayer.playTagDelayModePlayTag " + MyPlayer.playTagDelayModePlayTag.ToList());
      //Logger.Info("MyPlayer.playTagCountDown " + MyPlayer.playTagCountDown.Count);
      //Logger.Info("MyPlayer.playTagCountDown " + MyPlayer.playTagCountDown.ToList());
      //Logger.Info("MyPlayer.playTagCountDownOn " + MyPlayer.playTagCountDownOn.Count);
      //Logger.Info("MyPlayer.playTagCountDownOn " + MyPlayer.playTagCountDownOn.ToList());
      //Logger.Info("MyPlayer.creationTime " + MyPlayer.creationTime.Count);
      //Logger.Info("MyPlayer.creationTime " + MyPlayer.creationTime.ToList());
      //Logger.Info("MyPlayer.pauseDuration " + MyPlayer.pauseDuration.Count);
      ////Logger.Info("MyPlayer.pauseDuration " + MyPlayer.pauseDuration.ToList());
      //if (MyPlayer.playTagCountDownOn[self.PlayerIndex]) {
      //  Logger.Info("MyPlayer.playTagCountDownOn " + true);
      //} else {
      //  Logger.Info("MyPlayer.playTagCountDownOn " + false);
      //}
      //if (!MyPlayer.playTagCountDownOn[self.PlayerIndex] && self.Level.Session.MatchSettings.Mode != Modes.PlayTag)
      if (!MyPlayer.playTagCountDownOn[self.PlayerIndex] && self.Level.Session.MatchSettings.Mode != ModRegisters.GameModeType<TFModFortRiseGameModePlaytag.PlaytagGameMode>())
      {
        //  //hide arrow
        orig(self, wrapped);
      }

      //// Active the arrows just after the explosion in case the tag is a survivor
      if (self.Level.Session.MatchSettings.Mode == ModRegisters.GameModeType<TFModFortRiseGameModePlaytag.PlaytagGameMode>() && !MyPlayer.playTagCountDownOn[self.PlayerIndex] 
      //if (self.Level.Session.MatchSettings.Mode == Modes.PlayTag && !MyPlayer.playTagCountDownOn[self.PlayerIndex] 
          && MyPlayer.previousPlayTagCountDown[self.PlayerIndex] > MyPlayer.playTagCountDown[self.PlayerIndex])
      {
        orig(self, wrapped);
      }

      if (MyPlayer.playTag[self.PlayerIndex])
      {
        MyPlayer.PlayTagHUDRender(self);
      }
    }

    public static void PlayTagHUDRender(TowerFall.Player self)
    {
      MyPlayer.PlayTagHUD[self.PlayerIndex].Render();
      if (!(bool)(Monocle.Component)self.Indicator)
        return;
      self.Indicator.Render();
    }

    //public override void ShootArrow() 
    //{
    //  base.ShootArrow();
    //}

    public static void HurtBouncedOn_patch(On.TowerFall.Player.orig_HurtBouncedOn orig, TowerFall.Player self, int bouncerIndex)
    {
      if (MyPlayer.playTagCountDownOn[self.PlayerIndex])
        return;
      // When MatchSettings.Mode == Modes.PlayTag we can Hurt people juste after the bomb explose ^^, it's a feature!
      orig(self, bouncerIndex);
    }

    public static void Update(On.TowerFall.Player.orig_Update orig, global::TowerFall.Player self)
    //public override void Update()
    {
      Logger.Info("Update PlayerIndex = " + self.PlayerIndex);
      orig(self);
      //base.Update();
      Logger.Info("MyPlayer.playTagCountDownOn[PlayerIndex] : " + MyPlayer.playTagCountDownOn[self.PlayerIndex]);
      if (MyPlayer.playTagCountDownOn[self.PlayerIndex])
      {
        Logger.Info("true");
        self.Aiming = false; 
        int delay;
        //if (this.Level.Session.MatchSettings.Mode == Modes.PlayTag) {
        if (self.Level.Session.MatchSettings.Mode == ModRegisters.GameModeType<TFModFortRiseGameModePlaytag.PlaytagGameMode>()) {
          delay = MyPlayer.playTagDelayModePlayTag[self.PlayerIndex];
        } else {
          delay = MyPlayer.playTagDelay[self.PlayerIndex];
        }
        MyPlayer.previousPlayTagCountDown[self.PlayerIndex] = MyPlayer.playTagCountDown[self.PlayerIndex];
        MyPlayer.playTagCountDown[self.PlayerIndex] = delay - (int)(DateTime.Now - MyPlayer.creationTime[self.PlayerIndex]).TotalSeconds + MyPlayer.pauseDuration[self.PlayerIndex];
      } else {
        Logger.Info("false");

      }
    }

    public void addPauseDuration(int pauseDuration) {
      MyPlayer.pauseDuration[PlayerIndex] += pauseDuration;
    }
    
    public void resetPauseDuration()
    {
      MyPlayer.pauseDuration[PlayerIndex] = 0;
    }
  }
}
