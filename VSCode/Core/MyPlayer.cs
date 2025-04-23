using System;
using System.Collections.Generic;
using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyPlayer
  {
    internal static void Load()
    {
      On.TowerFall.Player.ctor += ctor_patch;
      On.TowerFall.Player.HUDRender += HUDRender_patch;
      On.TowerFall.Player.PlayerOnPlayer += PlayerOnPlayer_patch;
      On.TowerFall.Player.HurtBouncedOn += HurtBouncedOn_patch;
      On.TowerFall.Player.Update += Update;
      On.TowerFall.Player.Die_DeathCause_int_bool_bool += Die_DeathCause_int_bool_bool_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Player.ctor -= ctor_patch;
      On.TowerFall.Player.HUDRender -= HUDRender_patch;
      On.TowerFall.Player.PlayerOnPlayer -= PlayerOnPlayer_patch;
      On.TowerFall.Player.HurtBouncedOn -= HurtBouncedOn_patch;
      On.TowerFall.Player.Update -= Update;
      On.TowerFall.Player.Die_DeathCause_int_bool_bool -= Die_DeathCause_int_bool_bool_patch;
    }

    // Play Tag var
    public static Dictionary<int, bool> playTag = new Dictionary<int, bool>(8);
    public static Dictionary<int, PlayTagHUD> PlayTagHUD = new Dictionary<int, PlayTagHUD>(8);
    public static Dictionary<int, int> playTagCountDown = new Dictionary<int, int>(8);
    public static Dictionary<int, int> previousPlayTagCountDown = new Dictionary<int, int>(8);
    public static Dictionary<int, bool> playTagCountDownOn = new Dictionary<int, bool>(8);
    public static Dictionary<int, DateTime> creationTime = new Dictionary<int, DateTime>(8);
    public static Dictionary<int, int> pauseDuration = new Dictionary<int, int>(8);
    // End Play Tag var

    public MyPlayer(
      )
    {
    }

    public static void ctor_patch(On.TowerFall.Player.orig_ctor orig, TowerFall.Player self, int playerIndex, Vector2 position, Allegiance allegiance, Allegiance teamColor, global::TowerFall.PlayerInventory inventory, global::TowerFall.Player.HatStates hatState, bool frozen, bool flash, bool indicator) {
      orig(self, playerIndex, position, allegiance, teamColor, inventory, hatState, frozen, flash, indicator);
      MyPlayer.PlayTagHUD[playerIndex] = new PlayTagHUD();
      self.Add((Monocle.Component)(MyPlayer.PlayTagHUD[playerIndex]));
      MyPlayer.playTag[playerIndex] = false;
      MyPlayer.previousPlayTagCountDown[playerIndex] = 0;
      MyPlayer.playTagCountDown[playerIndex] = 0;
      MyPlayer.playTagCountDownOn[playerIndex] = false;
      MyPlayer.creationTime[playerIndex] = DateTime.Now;
      MyPlayer.pauseDuration[playerIndex] = 0;
    }

    public static void PlayerOnPlayer_patch(On.TowerFall.Player.orig_PlayerOnPlayer orig, Player a, Player b)
    {
      orig(a, b);
      if (MyPlayer.playTag[a.PlayerIndex])
      {
        MyPlayer.playTag[b.PlayerIndex] = true;
        MyPlayer.playTagCountDown[b.PlayerIndex] = MyPlayer.playTagCountDown[a.PlayerIndex];
        MyPlayer.creationTime[b.PlayerIndex] = MyPlayer.creationTime[a.PlayerIndex];

        MyPlayer.playTag[a.PlayerIndex] = false;
      }
      else if (MyPlayer.playTag[b.PlayerIndex])
      {
        MyPlayer.playTag[a.PlayerIndex] = true;
        MyPlayer.playTagCountDownOn[a.PlayerIndex] = true;
        MyPlayer.playTagCountDown[a.PlayerIndex] = MyPlayer.playTagCountDown[b.PlayerIndex];
        MyPlayer.creationTime[a.PlayerIndex] = MyPlayer.creationTime[b.PlayerIndex];

        MyPlayer.playTag[b.PlayerIndex] = false;
      }
    }
    public static void HUDRender_patch(On.TowerFall.Player.orig_HUDRender orig, TowerFall.Player self, bool wrapped)
    {
      if (!MyPlayer.playTagCountDownOn[self.PlayerIndex] && self.Level.Session.MatchSettings.Mode != ModRegisters.GameModeType<PlayTag>())
      {
        orig(self, wrapped);
      }

      if (self.Level.Session.MatchSettings.Mode == ModRegisters.GameModeType<PlayTag>() && !MyPlayer.playTagCountDownOn[self.PlayerIndex] 
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


    public static void HurtBouncedOn_patch(On.TowerFall.Player.orig_HurtBouncedOn orig, TowerFall.Player self, int bouncerIndex)
    {
      if (MyPlayer.playTagCountDownOn[self.PlayerIndex])
        return;
      orig(self, bouncerIndex);
    }

    public static void Update(On.TowerFall.Player.orig_Update orig, global::TowerFall.Player self)
    {
      orig(self);
      if (MyPlayer.playTagCountDownOn[self.PlayerIndex])
      {
        self.Aiming = false; 
        int delay;
        if (self.Level.Session.MatchSettings.Mode == ModRegisters.GameModeType<PlayTag>()) {
          delay = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayModePlayTag;
          
        } else {
          delay = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayPickup;
        }
        MyPlayer.previousPlayTagCountDown[self.PlayerIndex] = MyPlayer.playTagCountDown[self.PlayerIndex];
        MyPlayer.playTagCountDown[self.PlayerIndex] = delay - (int)(DateTime.Now - MyPlayer.creationTime[self.PlayerIndex]).TotalSeconds + MyPlayer.pauseDuration[self.PlayerIndex];
      }
    }

    public static PlayerCorpse Die_DeathCause_int_bool_bool_patch(On.TowerFall.Player.orig_Die_DeathCause_int_bool_bool orig, global::TowerFall.Player self, DeathCause deathCause, int killerIndex, bool brambled, bool laser)
    {
      //stop playtag if Tag player is killed before countdown reach 0
      //test when killed by the explosion
      if (MyPlayer.playTag[self.PlayerIndex]){
        TFModFortRiseGameModePlaytagModule.EndPlayTag(self);
      }
      return orig(self, deathCause, killerIndex, brambled, laser);
    }
  }
}
