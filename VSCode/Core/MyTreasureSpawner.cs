using FortRise;
using Microsoft.Xna.Framework;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyTreasureSpawner // : TreasureSpawner
  {
    //private const float BIG_CHEST_CHANCE = 0.03f;
    //public static readonly float[][] ChestChances = new float[5][];
    //public static readonly bool[] DarkWorldTreasures;
    //public const float DEFAULT_ARROW_CHANCE = 0.6f;
    //public static readonly float[] DefaultTreasureChances = new float[20]; 
    //public static readonly int[] FullTreasureMask = new int[20];  
    //public static bool IsPlayTagSpawn = false;
    public static List<Pickups> listPickupArrow = new List<Pickups>();
    public static List<Pickups> listPickupReplacement = new List<Pickups>();

    //static MyTreasureSpawner()
    //{

    //  float[] numArray = new float[] { 0.9f, 0.9f, 0.2f, 0.1f };
    //  float[] numArray2 = new float[] { 0.9f, 0.9f, 0.8f, 0.2f, 0.1f };
    //  float[] numArray3 = new float[] { 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
    //  float[] numArray4 = new float[] { 0.9f, 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
    //  float[] numArray5 = new float[] { 0.9f, 0.9f, 0.9f, 0.9f, 0.6f, 0.8f, 0.2f, 0.1f };
    //  ChestChances[0] = numArray;
    //  ChestChances[1] = numArray2;
    //  ChestChances[2] = numArray3;
    //  ChestChances[3] = numArray4;
    //  ChestChances[4] = numArray5;
    //  DefaultTreasureChances = new float[] {
    //            0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f, 0.8f,
    //              0.5f, 0.5f, 0.5f, 0.25f, 0.15f, 0.15f,
    //            0.15f, 0.15f, 0.001f, 0.1f,
    //          };
    //  FullTreasureMask = new int[] {
    //            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
    //            1, 1, 1, 1, 1, 1, 1,
    //            1, 1, 1,
    //        };
    //bool[] flagArray = new bool[20];
    //  flagArray[8] = true;
    //  flagArray[9] = true;
    //  DarkWorldTreasures = flagArray;
    //}

    internal static void Load()
    {
      On.TowerFall.TreasureSpawner.ctor_Session_Int32Array_float_bool += ctor_patch;
      On.TowerFall.TreasureSpawner.GetChestSpawnsForLevel += GetChestSpawnsForLevel_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.TreasureSpawner.ctor_Session_Int32Array_float_bool -= ctor_patch;
      On.TowerFall.TreasureSpawner.GetChestSpawnsForLevel -= GetChestSpawnsForLevel_patch;
    }

    public static void ctor_patch(On.TowerFall.TreasureSpawner.orig_ctor_Session_Int32Array_float_bool orig, TowerFall.TreasureSpawner self, Session session, int[] mask, float arrowChance, bool arrowShuffle)
    {
      orig(self, session,mask, arrowChance, arrowShuffle);
      listPickupArrow.Add(Pickups.Arrows);
      listPickupArrow.Add(Pickups.BombArrows);
      listPickupArrow.Add(Pickups.SuperBombArrows);
      listPickupArrow.Add(Pickups.LaserArrows);
      listPickupArrow.Add(Pickups.BrambleArrows);
      listPickupArrow.Add(Pickups.DrillArrows);
      listPickupArrow.Add(Pickups.BoltArrows);
      listPickupArrow.Add(Pickups.FeatherArrows);
      listPickupArrow.Add(Pickups.TriggerArrows);
      listPickupArrow.Add(Pickups.PrismArrows);

      listPickupReplacement.Add(Pickups.Shield);
      listPickupReplacement.Add(Pickups.Wings);
      listPickupReplacement.Add(Pickups.SpeedBoots);
      listPickupReplacement.Add(Pickups.Mirror);
      listPickupReplacement.Add(Pickups.TimeOrb);
      listPickupReplacement.Add(Pickups.DarkOrb);
      listPickupReplacement.Add(Pickups.LavaOrb);
      listPickupReplacement.Add(Pickups.SpaceOrb);
      listPickupReplacement.Add(Pickups.ChaosOrb);
      listPickupReplacement.Add(Pickups.Bomb);
    }

    public static Pickups getPlayTagPickupFromRealPickup(Pickups pickup)
    {
      switch (pickup)
      {
        case Pickups.Arrows: return ModRegisters.PickupType<PlayTagArrows>();
        case Pickups.BombArrows: return ModRegisters.PickupType<PlayTagBombArrows>();
        case Pickups.SuperBombArrows: return ModRegisters.PickupType<PlayTagSuperBombArrows>();
        case Pickups.LaserArrows: return ModRegisters.PickupType<PlayTagLaserArrows>();
        case Pickups.BrambleArrows: return ModRegisters.PickupType<PlayTagBrambleArrows>();
        case Pickups.DrillArrows: return ModRegisters.PickupType<PlayTagDrillArrows>();
        case Pickups.BoltArrows: return ModRegisters.PickupType<PlayTagBoltArrows>();
        case Pickups.FeatherArrows: return ModRegisters.PickupType<PlayTagFeatherArrows>();
        case Pickups.TriggerArrows: return ModRegisters.PickupType<PlayTagTriggerArrows>();
        case Pickups.PrismArrows: return ModRegisters.PickupType<PlayTagPrismArrows>();
        case Pickups.Shield: return ModRegisters.PickupType<PlayTagShield>();
        case Pickups.Wings: return ModRegisters.PickupType<PlayTagWings>();
        case Pickups.SpeedBoots: return ModRegisters.PickupType<PlayTagSpeedBoots>();
        case Pickups.Mirror: return ModRegisters.PickupType<PlayTagMirror>();
        case Pickups.TimeOrb: return ModRegisters.PickupType<PlayTagTimeOrb>();
        case Pickups.DarkOrb: return ModRegisters.PickupType<PlayTagDarkOrb>();
        case Pickups.LavaOrb: return ModRegisters.PickupType<PlayTagLavaOrb>();
        case Pickups.SpaceOrb: return ModRegisters.PickupType<PlayTagSpaceOrb>();
        default: throw new Exception("Pickup type not authorized!");
      }
    }

    public static List<TreasureChest> GetChestSpawnsForLevel_patch(
      On.TowerFall.TreasureSpawner.orig_GetChestSpawnsForLevel orig, 
      TowerFall.TreasureSpawner self,
      List<Vector2> chestPositions,
      List<Vector2> bigChestPositions)
    {
      List<TreasureChest> chestSpawnsForLevel = orig(self,chestPositions, bigChestPositions);

      Logger.Info("GetChestSpawnsForLevel_patch MySession.NbPlayTagPickupActivated = " + MySession.NbPlayTagPickupActivated);

      if (
          //!IsPlayTagSpawn && 
          TFModFortRiseGameModePlaytagModule.Settings.playTagPickupActivated
          && chestSpawnsForLevel.Count > 0 
          && MySession.NbPlayTagPickupActivated == 0
          && self.Session.MatchSettings.Mode != ModRegisters.GameModeType<PlayTag>())
      {
        Logger.Info("GetChestSpawnsForLevel_patch ok in if chestSpawnsForLevel");

        Random rnd = new Random();
        int draw = 1;// rnd.Next(0, 3);
        for (var i = 0; draw == 1 && i < chestSpawnsForLevel.Count; i++)
        {
          var dynData = DynamicData.For(chestSpawnsForLevel[i]);
          List<Pickups> pickups = (List<Pickups>)dynData.Get("pickups");
          Logger.Info("GetChestSpawnsForLevel_patch pickups?count =" + pickups.Count);

          //if (!PlayTag.realPickupPossibleList.Contains(chestSpawnsForLevel[i].pickups[0]))
          if (!PlayTagPickup.realPickupPossibleList.Contains(pickups[0]))
          {
            continue;
          }

          pickups[0] = getPlayTagPickupFromRealPickup(pickups[0]);
          //chestSpawnsForLevel[i].pickups[0] = getPlayTagPickupFromRealPickup(chestSpawnsForLevel[i].pickups[0]);
          //IsPlayTagSpawn = true;
          break;
        }
      }
      else if (self.Session.MatchSettings.Mode == ModRegisters.GameModeType<PlayTag>())
      {
        Logger.Info("GetChestSpawnsForLevel_patch ok in if GameModeType");

        Random rnd = new Random();

        for (var i = 0; i < chestSpawnsForLevel.Count; i++)
        {
          var dynData = DynamicData.For(chestSpawnsForLevel[i]);
          List<Pickups> pickups = (List<Pickups>)dynData.Get("pickups");
          //if (!listPickupArrow.Contains(chestSpawnsForLevel[i].pickups[0]))
          if (!listPickupArrow.Contains(pickups[0]))
          {
            continue;
          }
          // No need for Arrow in this mode
          //chestSpawnsForLevel[i].pickups[0] = listPickupReplacement[rnd.Next(0, listPickupReplacement.Count - 1)];
          pickups[0] = listPickupReplacement[rnd.Next(0, listPickupReplacement.Count - 1)];
        }
      }
      return chestSpawnsForLevel;
    }

    //public override bool CanSpawnAnotherChest(int alreadySpawnedAmount)
    //{
    //  if (alreadySpawnedAmount >= ChestChances[Math.Min((TFGame.PlayerAmount - 2), 4)].Length)
    //  {
    //    return false;
    //  }
    //  if (this.Session.MatchSettings.Variants.MaxTreasure == null)
    //  {
    //    return this.Random.Chance(ChestChances[Math.Min((TFGame.PlayerAmount - 2), 4)][alreadySpawnedAmount]);
    //  }
    //  return true;
    //}

  }
}
