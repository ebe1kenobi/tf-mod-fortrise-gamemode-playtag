using FortRise;
using Microsoft.Xna.Framework;
using System;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyPickup : Pickup
  {
    internal static void Load()
    {
      On.TowerFall.Pickup.CreatePickup += CreatePickup_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Pickup.CreatePickup -= CreatePickup_patch;
    }

    public static Pickup CreatePickup_patch(
      On.TowerFall.Pickup.orig_CreatePickup orig, Vector2 position, Vector2 targetPosition, Pickups type, int playerIndex)
    {
      Logger.Info("CreatePickup_patch type=" + type);

      Pickup pickup;
      try
      {
        pickup = orig(position, targetPosition, type, playerIndex);
      } catch (Exception e) {
        Logger.Info("CreatePickup_patch execption");

        //switch (type)
        //{
        //  case ModRegisters.PickupType<PlayTagSpaceOrb>():
        //  case ModRegisters.PickupType<PlayTagLavaOrb>():
        //  case ModRegisters.PickupType<PlayTagTimeOrb>():
        //  case ModRegisters.PickupType<PlayTagDarkOrb>():
        //  case ModRegisters.PickupType<PlayTagShield>():
        //  case ModRegisters.PickupType<PlayTagWings>():
        //  case ModRegisters.PickupType<PlayTagSpeedBoots>():
        //  case ModRegisters.PickupType<PlayTagMirror>():
        //  case ModRegisters.PickupType<PlayTagArrows>():
        //  case ModRegisters.PickupType<PlayTagBombArrows>():
        //  case ModRegisters.PickupType<PlayTagSuperBombArrows>():
        //  case ModRegisters.PickupType<PlayTagLaserArrows>():
        //  case ModRegisters.PickupType<PlayTagBrambleArrows>():
        //  case ModRegisters.PickupType<PlayTagDrillArrows>():
        //  case ModRegisters.PickupType<PlayTagBoltArrows>():
        //  case ModRegisters.PickupType<PlayTagFeatherArrows>():
        //  case ModRegisters.PickupType<PlayTagTriggerArrows>():
        //  case ModRegisters.PickupType<PlayTagPrismArrows>():
        try
        {
          if (type == ModRegisters.PickupType<PlayTagSpaceOrb>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=PlayTagSpaceOrb " + type);
            pickup = (Pickup)new PlayTagSpaceOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagLavaOrb>())
          {
            Logger.Info("CreatePickup_patch new PlayTag typePlayTagLavaOrb =" + type);
            pickup = (Pickup)new PlayTagLavaOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagTimeOrb>())
          {
            Logger.Info("CreatePickup_patch new PlayTag typePlayTagTimeOrb =" + type);
            pickup = (Pickup)new PlayTagTimeOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagDarkOrb>())
          {
            Logger.Info("CreatePickup_patch new PlayTag typePlayTagDarkOrb =" + type);
            pickup = (Pickup)new PlayTagDarkOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagShield>())
          {
            Logger.Info("CreatePickup_patch new PlayTag typPlayTagShield e=" + type);
            pickup = (Pickup)new PlayTagShield(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagWings>())
          {
            Logger.Info("CreatePickup_patch new PlayTag tyPlayTagWings pe=" + type);
            pickup = (Pickup)new PlayTagWings(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagSpeedBoots>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type= PlayTagSpeedBoots " + type);
            pickup = (Pickup)new PlayTagSpeedBoots(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagMirror>())
          {
            Logger.Info("CreatePickup_patch new PlayTag typPlayTagMirror e=" + type);
            pickup = (Pickup)new PlayTagMirror(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag typPlayTagArrows e=" + type);
            pickup = (Pickup)new PlayTagArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagBombArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=PlayTagBombArrows " + type);
            pickup = (Pickup)new PlayTagBombArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagSuperBombArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=typPlayTagSuperBombArrows " + type);
            pickup = (Pickup)new PlayTagSuperBombArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagLaserArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type= PlayTagLaserArrows " + type);
            pickup = (Pickup)new PlayTagLaserArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagBrambleArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=PlayTagBrambleArrows " + type);
            pickup = (Pickup)new PlayTagBrambleArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagDrillArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type= PlayTagDrillArrows" + type);
            pickup = (Pickup)new PlayTagDrillArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagBoltArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=PlayTagBoltArrows " + type);
            pickup = (Pickup)new PlayTagBoltArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagFeatherArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=tPlayTagFeatherArrows" + type);
            pickup = (Pickup)new PlayTagFeatherArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagTriggerArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=tPlayTagTriggerArrows " + type);
            pickup = (Pickup)new PlayTagTriggerArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagPrismArrows>())
          {
            Logger.Info("CreatePickup_patch new PlayTag type=PlayTagPrismArrows " + type);
            pickup = (Pickup)new PlayTagPrismArrows(position, targetPosition, type);
          } else {
            pickup = (Pickup)new PlayTagPrismArrows(position, targetPosition, type);
            throw new Exception();
          }
        }
        catch (Exception ExceptionPlayTag)
        {
          throw ExceptionPlayTag;
        }
            //break;
          //default:
          //  throw e;
        //}
        //pickup.PickupType = type;
      }

      return pickup;
    }

    public MyPickup(Vector2 position, Vector2 targetPosition)
      : base(position, targetPosition) {}

  }
}
