using System;
using System.Collections.Generic;
using FortRise;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Monocle;
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
    /// <summary>
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
        // En mode PlayTag c'est le delai du mode qui s'applique ; ailleurs, la bombe
        // vient de l'objet a ramasser, donc son propre delai.
        //
        // La comparaison etait inversee (!= au lieu de ==) : en mode PlayTag le jeu
        // lisait le delai du pickup, si bien que le reglage du mode - celui de la
        // popup comme celui des options - n'avait aucun effet. La ligne d'origine
        // conservee en commentaire ci-dessous utilisait bien ==.
        int delay;
        //if (__instance.Level.Session.MatchSettings.Mode == ModRegisters.GameModeType<PlayTag>()) {
        if (__instance.Level.Session.MatchSettings.Mode == PlayTagGameMode.PlayTagMode.Modes)
        {
          delay = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayModePlayTag;
        }
        else
        {
          delay = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayPickup;
        }
        MyPlayer.previousPlayTagCountDown[__instance.PlayerIndex] = MyPlayer.playTagCountDown[__instance.PlayerIndex];
        MyPlayer.playTagCountDown[__instance.PlayerIndex] = delay - (int)(DateTime.Now - MyPlayer.creationTime[__instance.PlayerIndex]).TotalSeconds + MyPlayer.pauseDuration[__instance.PlayerIndex];

        Expire(__instance);
      }
    }

    /// <summary>
    /// Le decompte est arrive a zero : la bombe explose sur le joueur marque.
    ///
    /// Ce code vivait dans le RENDU du decompte, avec l'aveu en commentaire que ce
    /// n'etait pas sa place. Ce n'etait pas qu'inelegant : un rendu qui ne part pas -
    /// et il ne partait plus - emportait la regle du jeu avec lui, l'archer ne mourait
    /// jamais et la manche ne se terminait pas. Dans Update, elle s'applique que
    /// quelqu'un regarde ou non.
    /// </summary>
    private static void Expire(Player player)
    {
      if (MyPlayer.playTagCountDown[player.PlayerIndex] > 0)
      {
        return;
      }

      // Seul le joueur MARQUE explose. Les autres voient juste leur decompte s'arreter.
      if (!MyPlayer.playTag[player.PlayerIndex])
      {
        return;
      }

      foreach (Player p in player.Level.Session.CurrentLevel[GameTags.Player])
      {
        MyPlayer.playTagCountDownOn[p.PlayerIndex] = false;
      }

      Player.ShootLock = false;
      Explosion.SpawnSuper(player.Level, player.Position, player.PlayerIndex, true);
    }

    /// <summary>
    /// Le decompte doit-il etre dessine au-dessus de cet archer ?
    ///
    /// Seul le joueur marque le porte : c'est l'indicateur du mode, il dit qui a le
    /// tag autant que le temps qui reste.
    /// </summary>
    public static bool ShowsCountdown(Player player)
    {
      if (player == null || player.Dead)
      {
        return false;
      }

      int index = player.PlayerIndex;

      return MyPlayer.playTagCountDownOn.TryGetValue(index, out bool running) && running
          && MyPlayer.playTag.TryGetValue(index, out bool tagged) && tagged;
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
