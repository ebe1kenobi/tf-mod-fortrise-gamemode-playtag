using System;
using System.Collections.Generic;
using FortRise;
using HarmonyLib;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyTreasureSpawner : IHookable
  {
    public static List<Pickups> listPickupArrow = new List<Pickups>();
    public static List<Pickups> listPickupReplacement = new List<Pickups>();

    public static void Load(IHarmony harmony)
    {
      harmony.Patch(
          AccessTools.DeclaredConstructor(typeof(TreasureSpawner), [typeof(Session), typeof(VersusTowerData)]),
          postfix: new HarmonyMethod(TreasureSpawner_ctor_Postfix)
      );
    }

    public static void TreasureSpawner_ctor_Postfix(TreasureSpawner __instance)
    {
      Logger.Info("TreasureSpawner_ctor_Postfix");
      var PlayTag = PlayTagPickup.PlayTagMeta.Pickups;
      Logger.Info($"PlayTag != null {PlayTagPickup.PlayTagMeta.Name} {(int)PlayTag}");

      if (!TFModFortRiseGameModePlaytagModule.activated()
          || __instance.Session.MatchSettings.Mode == PlayTagGameMode.PlayTagMode.Modes)
      {
        __instance.TreasureRates[(int)PlayTag] = 0f;
        return;
      }

      Random rnd = new Random();
      int draw;
      if (TFModFortRiseGameModePlaytagModule.Settings.periodicity == "Test")
      {
        draw = 1;
      }
      else
      {
        draw = rnd.Next(0, TFModFortRiseGameModePlaytagModule.Settings.treasureRate);
      }
      Logger.Info($"draw = {draw}");

      if (draw == 1)
      {
        __instance.TreasureRates[(int)PlayTag] = 1f;
      }
    }

    //public static void ctor_patch(On.TowerFall.TreasureSpawner.orig_ctor_Session_Int32Array_float_bool orig, TowerFall.TreasureSpawner self, Session session, int[] mask, float arrowChance, bool arrowShuffle)
    //{
    //  orig(self, session,mask, arrowChance, arrowShuffle);
    //  listPickupArrow.Add(Pickups.Arrows);
    //  listPickupArrow.Add(Pickups.BombArrows);
    //  listPickupArrow.Add(Pickups.SuperBombArrows);
    //  listPickupArrow.Add(Pickups.LaserArrows);
    //  listPickupArrow.Add(Pickups.BrambleArrows);
    //  listPickupArrow.Add(Pickups.DrillArrows);
    //  listPickupArrow.Add(Pickups.BoltArrows);
    //  listPickupArrow.Add(Pickups.FeatherArrows);
    //  listPickupArrow.Add(Pickups.TriggerArrows);
    //  listPickupArrow.Add(Pickups.PrismArrows);

    //  listPickupReplacement.Add(Pickups.Shield);
    //  listPickupReplacement.Add(Pickups.Wings);
    //  listPickupReplacement.Add(Pickups.SpeedBoots);
    //  listPickupReplacement.Add(Pickups.Mirror);
    //  listPickupReplacement.Add(Pickups.TimeOrb);
    //  listPickupReplacement.Add(Pickups.DarkOrb);
    //  listPickupReplacement.Add(Pickups.LavaOrb);
    //  listPickupReplacement.Add(Pickups.SpaceOrb);
    //  listPickupReplacement.Add(Pickups.ChaosOrb);
    //  listPickupReplacement.Add(Pickups.Bomb);
    //}

    //public static Pickups getPlayTagPickupFromRealPickup(Pickups pickup)
    //{
    //  switch (pickup)
    //  {
    //    case Pickups.Arrows: return ModRegisters.PickupType<PlayTagArrows>();
    //    case Pickups.BombArrows: return ModRegisters.PickupType<PlayTagBombArrows>();
    //    case Pickups.SuperBombArrows: return ModRegisters.PickupType<PlayTagSuperBombArrows>();
    //    case Pickups.LaserArrows: return ModRegisters.PickupType<PlayTagLaserArrows>();
    //    case Pickups.BrambleArrows: return ModRegisters.PickupType<PlayTagBrambleArrows>();
    //    case Pickups.DrillArrows: return ModRegisters.PickupType<PlayTagDrillArrows>();
    //    case Pickups.BoltArrows: return ModRegisters.PickupType<PlayTagBoltArrows>();
    //    case Pickups.FeatherArrows: return ModRegisters.PickupType<PlayTagFeatherArrows>();
    //    case Pickups.TriggerArrows: return ModRegisters.PickupType<PlayTagTriggerArrows>();
    //    case Pickups.PrismArrows: return ModRegisters.PickupType<PlayTagPrismArrows>();
    //    case Pickups.Shield: return ModRegisters.PickupType<PlayTagShield>();
    //    case Pickups.Wings: return ModRegisters.PickupType<PlayTagWings>();
    //    case Pickups.SpeedBoots: return ModRegisters.PickupType<PlayTagSpeedBoots>();
    //    case Pickups.Mirror: return ModRegisters.PickupType<PlayTagMirror>();
    //    case Pickups.TimeOrb: return ModRegisters.PickupType<PlayTagTimeOrb>();
    //    case Pickups.DarkOrb: return ModRegisters.PickupType<PlayTagDarkOrb>();
    //    case Pickups.LavaOrb: return ModRegisters.PickupType<PlayTagLavaOrb>();
    //    case Pickups.SpaceOrb: return ModRegisters.PickupType<PlayTagSpaceOrb>();
    //    default: throw new Exception("Pickup type not authorized!");
    //  }
    //}

    //public static List<TreasureChest> GetChestSpawnsForLevel_patch(
    //  On.TowerFall.TreasureSpawner.orig_GetChestSpawnsForLevel orig, 
    //  TowerFall.TreasureSpawner self,
    //  List<Vector2> chestPositions,
    //  List<Vector2> bigChestPositions)
    //{
    //  List <TreasureChest> chestSpawnsForLevel = orig(self,chestPositions, bigChestPositions);

    //  if (chestSpawnsForLevel.Count == 0)
    //  {
    //    return chestSpawnsForLevel;
    //  }

    //  if (TFModFortRiseGameModePlaytagModule.activated()
    //      && MySession.NbPlayTagPickupActivated == 0
    //      && self.Session.MatchSettings.Mode != ModRegisters.GameModeType<PlayTag>())
    //  {
    //    Random rnd = new Random();
    //    int draw;
    //    if (TFModFortRiseGameModePlaytagModule.Settings.periodicity == "Test")
    //    {
    //      draw = 1;
    //    }
    //    else
    //    {
    //      draw = rnd.Next(0, TFModFortRiseGameModePlaytagModule.Settings.treasureRate); 
    //    }
    //    for (var i = 0; draw == 1 && i < chestSpawnsForLevel.Count; i++)
    //    {
    //      var dynData = DynamicData.For(chestSpawnsForLevel[i]);
    //      List<Pickups> pickups = (List<Pickups>)dynData.Get("pickups");
    //      if (!PlayTagPickup.realPickupPossibleList.Contains(pickups[0]))
    //      {
    //        continue;
    //      }

    //      pickups[0] = getPlayTagPickupFromRealPickup(pickups[0]);
    //      MySession.NbPlayTagPickupActivated++;
    //      break;
    //    }
    //  }
    //  return chestSpawnsForLevel;
    //}
  }
}
