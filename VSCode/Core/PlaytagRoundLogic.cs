using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monocle;
using MonoMod.Utils;
using TowerFall;
using Microsoft.Xna.Framework;

namespace TFModFortRiseGameModePlaytag
{
  [CustomRoundLogic("PlaytagRoundLogic")]
  internal class PlaytagRoundLogic : CustomVersusRoundLogic
  {
    private RoundEndCounter roundEndCounter;
    private bool done;
    private static int[] playerOrder;
    private static int currentPlayerOrderIndex;
    private static int lastNumberOfPlayer = 0;


    public static RoundLogicInfo Create()
    {
      //private static IDetour hook_GetDodgeExitState;


      return new RoundLogicInfo
      {
        Name = "Playtag",
        Icon = TFGame.MenuAtlas["gameModes/warlord"],
        //Icon = TFModFortRiseGameModePlaytagModule.RespawnAtlas["gamemodes/respawn"], //TODO
        RoundType = RoundLogicType.HeadHunters
      };
    }
    internal static void Load()
    {
      On.TowerFall.RoundLogic.FFACheckForAllButOneDead += FFACheckForAllButOneDead_patch;

      
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
      On.TowerFall.RoundLogic.FFACheckForAllButOneDead -= FFACheckForAllButOneDead_patch;
    }

    private static bool FFACheckForAllButOneDead_patch(On.TowerFall.RoundLogic.orig_FFACheckForAllButOneDead orig, RoundLogic self)
    {
      if (self is PlaytagRoundLogic)
        return false;
      return orig(self);
    }

    public PlaytagRoundLogic(Session session) : base(session, true)
    {
      this.roundEndCounter = new RoundEndCounter(session);
    }

    private void resetPlayerAlreadyTag()
    {
      currentPlayerOrderIndex = -1;
      lastNumberOfPlayer = 0;
      List<int> listPlayerIndex = new List<int>();
      foreach (Player player in this.Session.CurrentLevel.Players)
      {

        listPlayerIndex.Add(player.PlayerIndex);
        lastNumberOfPlayer++;
      }
      listPlayerIndex.Shuffle();
      playerOrder = listPlayerIndex.ToArray();
    }


    public override void OnLevelLoadFinish()
    {
      base.OnLevelLoadFinish();
      this.Session.CurrentLevel.Add<VersusStart>(new VersusStart(this.Session));
      this.Players = this.SpawnPlayersFFA();
    }

    // Match start
    public override void OnRoundStart()
    {
      base.OnRoundStart();
      this.SpawnTreasureChestsVersus();
      initPlayTag(getNextTagPlayerIndex());
    }

    public override void OnUpdate()
    {
      SessionStats.TimePlayed += Engine.DeltaTicks;
      base.OnUpdate();

      if (!this.RoundStarted || this.done || !this.Session.CurrentLevel.Ending || !this.Session.CurrentLevel.CanEnd)
        return;
      if (!this.roundEndCounter.Finished)
      {
        this.roundEndCounter.Update();
      }
      else
      {
        this.done = true;
        if (this.Session.CurrentLevel.Players.Count > 0)
        {
          //add score for all living player
          foreach (Player player in this.Session.CurrentLevel[GameTags.Player])
          {
            if (player.Dead) continue;
            this.AddScore(player.PlayerIndex, 1);
          }
        }
        this.InsertCrownEvent();
        this.Session.EndRound();
      }
    }

    public override void OnPlayerDeath(
      Player player,
      PlayerCorpse corpse,
      int playerIndex,
      DeathCause deathType,
      Vector2 position,
      int killerIndex)
    {
      base.OnPlayerDeath(player, corpse, playerIndex, deathType, position, killerIndex);

      //if (player.playTag)
      //{
      //  this.Session.CurrentLevel.Ending = true;
      //  return;
      //}

      //if (!player.playTagCountDownOn)
      //{
      //  this.Session.CurrentLevel.Ending = true;
      //  return;
      //}

      if (this.Session.CurrentLevel.LivingPlayers == 1)
      {
        this.Session.CurrentLevel.Ending = true;
        return;
      }

      if (this.Session.CurrentLevel.LivingPlayers == 0)
      {
        this.Session.CurrentLevel.Ending = true;
        return;
      }
    }

    public bool OtherPlayerCouldWin(int playerIndex)
    {
      return false;
    }

    public void initPlayTag(int playerIndex)
    {
      Player.ShootLock = true;
      foreach (Player p in this.Session.CurrentLevel.Players)
      {
        //  p.playTagCountDown = p.playTagDelayModePlayTag;
        //  p.playTagCountDownOn = true;
        //  p.playTag = false;
        //  p.creationTime = DateTime.Now;

        //  if (p.PlayerIndex == playerIndex)
        //  {
        //    p.playTag = true;
        //  }
      }
    }

    private int getNextTagPlayerIndex()
    {
      if (playerOrder == null || currentPlayerOrderIndex == playerOrder.Length - 1 || lastNumberOfPlayer != this.Session.CurrentLevel.Players.Count)
      {
        resetPlayerAlreadyTag();
      }
      currentPlayerOrderIndex++;
      return playerOrder[currentPlayerOrderIndex];
    }
  }
}
