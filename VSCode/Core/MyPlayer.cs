using System;
using System.Collections.Generic;
using FortRise;
using HarmonyLib;
using Microsoft.Xna.Framework;
using TowerFall;
using static TowerFall.Player;

namespace TFModFortRiseGameModePlaytag
{
  public class MyPlayer : IHookable
  {
    public static void Load(IHarmony harmony)
    {
      harmony.Patch(
          AccessTools.DeclaredConstructor(typeof(Player), [
                                                            typeof(int),
                                                            typeof(Vector2),
                                                            typeof(Allegiance),
                                                            typeof(Allegiance),
                                                            typeof(PlayerInventory),
                                                            typeof(HatStates),
                                                            typeof(bool),
                                                            typeof(bool),
                                                            typeof(bool)
                                                                        ]),
          postfix: new HarmonyMethod(ctor_patch)
      );
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(Player), nameof(Player.HUDRender)),
          postfix: new HarmonyMethod(HUDRender_patch)
      );
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(Player), "PlayerOnPlayer"),
          postfix: new HarmonyMethod(PlayerOnPlayer_patch)
      );
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(Player), nameof(Player.HurtBouncedOn)),
          prefix: new HarmonyMethod(HurtBouncedOn_patch)
      );
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(Player), nameof(Player.Update)),
          postfix: new HarmonyMethod(Update)
      );
      //harmony.Patch(
      //    AccessTools.DeclaredMethod(typeof(Player), nameof(Player.Die), [
      //                                                       typeof(DeathCause),
      //                                                       typeof(int),
      //                                                       typeof(bool),
      //                                                       typeof(bool)
      //                                                                  ]),
      //    prefix: new HarmonyMethod(Die_DeathCause_int_bool_bool_prefix_patch)
      //);
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(Player), nameof(Player.Die), [
                                                             typeof(DeathCause),
                                                             typeof(int),
                                                             typeof(bool),
                                                             typeof(bool)
                                                                        ]),
          postfix: new HarmonyMethod(Die_DeathCause_int_bool_bool_postfix_patch)
      );
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

    public static void ctor_patch(Player __instance, int playerIndex, Vector2 position, Allegiance allegiance, Allegiance teamColor, global::TowerFall.PlayerInventory inventory, global::TowerFall.Player.HatStates hatState, bool frozen, bool flash, bool indicator) {
      MyPlayer.PlayTagHUD[playerIndex] = new PlayTagHUD();
      __instance.Add((Monocle.Component)(MyPlayer.PlayTagHUD[playerIndex]));
      MyPlayer.playTag[playerIndex] = false;
      MyPlayer.previousPlayTagCountDown[playerIndex] = 0;
      MyPlayer.playTagCountDown[playerIndex] = 0;
      MyPlayer.playTagCountDownOn[playerIndex] = false;
      MyPlayer.creationTime[playerIndex] = DateTime.Now;
      MyPlayer.pauseDuration[playerIndex] = 0;
    }

    public static void PlayerOnPlayer_patch(Player __instance, Player a, Player b)
    {
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
    public static void HUDRender_patch(Player __instance, bool wrapped)
    {
      if (!MyPlayer.playTagCountDownOn[__instance.PlayerIndex] && 
          //__instance.Level.Session.MatchSettings.Mode != ModRegisters.GameModeType<PlayTag>())
        __instance.Level.Session.MatchSettings.Mode != PlayTagGameMode.PlayTagMode.Modes)
      {
        return;
      }

      //if (__instance.Level.Session.MatchSettings.Mode == ModRegisters.GameModeType<PlayTag>() 
      if (__instance.Level.Session.MatchSettings.Mode != PlayTagGameMode.PlayTagMode.Modes
          && !MyPlayer.playTagCountDownOn[__instance.PlayerIndex] 
          && MyPlayer.previousPlayTagCountDown[__instance.PlayerIndex] > MyPlayer.playTagCountDown[__instance.PlayerIndex])
      {
        return;
      }
      //todo test without origin(self)!

      if (MyPlayer.playTag[__instance.PlayerIndex])
      {
        MyPlayer.PlayTagHUDRender(__instance);
      }
    }

    public static void PlayTagHUDRender(TowerFall.Player self)
    {
      MyPlayer.PlayTagHUD[self.PlayerIndex].Render();
      if (!(bool)(Monocle.Component)self.Indicator)
        return;
      self.Indicator.Render();
    }


    public static bool HurtBouncedOn_patch(Player __instance, int bouncerIndex)
    {
      if (MyPlayer.playTagCountDownOn[__instance.PlayerIndex])
        return false; //don't execute the original die function
      return true;
    }

    public static void Update(Player __instance)
    {
      if (MyPlayer.playTagCountDownOn[__instance.PlayerIndex])
      {
        __instance.Aiming = false; 
        int delay;
        //if (__instance.Level.Session.MatchSettings.Mode == ModRegisters.GameModeType<PlayTag>()) {
        if (__instance.Level.Session.MatchSettings.Mode != PlayTagGameMode.PlayTagMode.Modes) { 
            delay = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayModePlayTag;
        } else {
          delay = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayPickup;
        }
        MyPlayer.previousPlayTagCountDown[__instance.PlayerIndex] = MyPlayer.playTagCountDown[__instance.PlayerIndex];
        MyPlayer.playTagCountDown[__instance.PlayerIndex] = delay - (int)(DateTime.Now - MyPlayer.creationTime[__instance.PlayerIndex]).TotalSeconds + MyPlayer.pauseDuration[__instance.PlayerIndex];
      }
    }

    //public static bool Die_DeathCause_int_bool_bool_prefix_patch(Player __instance, DeathCause deathCause, int killerIndex, bool brambled, bool laser)
    //{
    //  // can't die when playtag
    //  if (deathCause == DeathCause.JumpedOn)
    //  {
    //    if (MyPlayer.playTagCountDownOn[__instance.PlayerIndex])
    //      return false;  //don't execute the original die function
    //  }

    //  return true;
    //}

    public static void Die_DeathCause_int_bool_bool_postfix_patch(Player __instance, DeathCause deathCause, int killerIndex, bool brambled, bool laser)
    {
      //stop playtag if Tag player is killed before countdown reach 0
      //test when killed by the explosion
      if (MyPlayer.playTag[__instance.PlayerIndex]){
        TFModFortRiseGameModePlaytagModule.EndPlayTag(__instance);
      }
    }
  }
}
