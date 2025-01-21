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
      Pickup pickup;
      try
      {
        pickup = orig(position, targetPosition, type, playerIndex);
      } catch (Exception e) {
        try
        {
          if (type == ModRegisters.PickupType<PlayTagSpaceOrb>())
          {
            pickup = (Pickup)new PlayTagSpaceOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagLavaOrb>())
          {
            pickup = (Pickup)new PlayTagLavaOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagTimeOrb>())
          {
            pickup = (Pickup)new PlayTagTimeOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagDarkOrb>())
          {
            pickup = (Pickup)new PlayTagDarkOrb(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagShield>())
          {
            pickup = (Pickup)new PlayTagShield(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagWings>())
          {
            pickup = (Pickup)new PlayTagWings(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagSpeedBoots>())
          {
            pickup = (Pickup)new PlayTagSpeedBoots(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagMirror>())
          {
            pickup = (Pickup)new PlayTagMirror(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagArrows>())
          {
            pickup = (Pickup)new PlayTagArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagBombArrows>())
          {
            pickup = (Pickup)new PlayTagBombArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagSuperBombArrows>())
          {
            pickup = (Pickup)new PlayTagSuperBombArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagLaserArrows>())
          {
            pickup = (Pickup)new PlayTagLaserArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagBrambleArrows>())
          {
            pickup = (Pickup)new PlayTagBrambleArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagDrillArrows>())
          {
            pickup = (Pickup)new PlayTagDrillArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagBoltArrows>())
          {
            pickup = (Pickup)new PlayTagBoltArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagFeatherArrows>())
          {
            pickup = (Pickup)new PlayTagFeatherArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagTriggerArrows>())
          {
            pickup = (Pickup)new PlayTagTriggerArrows(position, targetPosition, type);
          }
          else if (type == ModRegisters.PickupType<PlayTagPrismArrows>())
          {
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
      }

      return pickup;
    }

    public MyPickup(Vector2 position, Vector2 targetPosition)
      : base(position, targetPosition) {}

  }
}
