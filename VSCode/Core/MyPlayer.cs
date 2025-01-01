using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyPlayer : Player
  {
    //public static Monocle.Collider[] wasColliders = new Monocle.Collider[4];
    internal static void Load()
    {
      On.TowerFall.Player.HUDRender += HUDRender_patch;
      On.TowerFall.Player.PlayerOnPlayer += PlayerOnPlayer_patch;
      On.TowerFall.Player.HurtBouncedOn += HurtBouncedOn_patch;
      
      //hook_GetDodgeExitState = new Hook(
      //    typeof(Player).GetMethod("GetDodgeExitState", BindingFlags.NonPublic | BindingFlags.Instance),
      //    Player_GetDodgeExitState
      //);

      //public static int Player_GetDodgeExitState(orig_Player_GetDodgeExitState orig, Player self)
      //{
      //  /* New */
      //  if (VariantManager.GetCustomVariant("NoDodgeCooldowns")[self.PlayerIndex])
      //  {
      //    var dynData = new DynData<Player>(self);
      //    dynData.Set("dodgeCooldown", false);
      //    dynData.Dispose();
      //  }
      //  return orig(self);
      //}
    }

    internal static void Unload()
    {
      On.TowerFall.Player.HUDRender -= HUDRender_patch;
      On.TowerFall.Player.PlayerOnPlayer -= PlayerOnPlayer_patch;
      On.TowerFall.Player.HurtBouncedOn -= HurtBouncedOn_patch;
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
      this.Add((Monocle.Component)(MyPlayer.PlayTagHUD[playerIndex] = new PlayTagHUD()));
      MyPlayer.playTag[PlayerIndex] = false;
      MyPlayer.playTagDelay[PlayerIndex] = 10;
      MyPlayer.playTagDelayModePlayTag[PlayerIndex] = 15;
      MyPlayer.playTagCountDown[PlayerIndex] = 0;
      MyPlayer.playTagCountDownOn[PlayerIndex] = false;
      MyPlayer.creationTime[PlayerIndex] = DateTime.Now;
      MyPlayer.pauseDuration[PlayerIndex] = 0;
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
      //if (!MyPlayer.playTagCountDownOn[self.PlayerIndex] && self.Level.Session.MatchSettings.Mode != Modes.PlayTag)
      if (!MyPlayer.playTagCountDownOn[self.PlayerIndex] && self.Level.Session.MatchSettings.Mode != Modes.PlayTag)
      {
        //hide arrow
        orig(self, wrapped);
      }

      // Active the arrows just after the explosion in case the tag is a survivor
      if (self.Level.Session.MatchSettings.Mode == Modes.PlayTag && !MyPlayer.playTagCountDownOn[self.PlayerIndex] 
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

    public override void Update()
    {
      base.Update();
      if (MyPlayer.playTagCountDownOn[PlayerIndex])
      {
        this.Aiming = false; 
        int delay;
        //if (this.Level.Session.MatchSettings.Mode == Modes.PlayTag) {
        if (this.Level.Session.MatchSettings.Mode == Modes.PlayTag) {
          delay = MyPlayer.playTagDelayModePlayTag[PlayerIndex];
        } else {
          delay = MyPlayer.playTagDelay[PlayerIndex];
        }
        MyPlayer.previousPlayTagCountDown[PlayerIndex] = MyPlayer.playTagCountDown[PlayerIndex];
        MyPlayer.playTagCountDown[PlayerIndex] = delay - (int)(DateTime.Now - MyPlayer.creationTime[PlayerIndex]).TotalSeconds + MyPlayer.pauseDuration[PlayerIndex];
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
