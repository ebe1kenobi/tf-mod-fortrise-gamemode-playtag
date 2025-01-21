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
  internal class PlaytagRoundLogic : RoundLogic
  {
    private RoundEndCounter roundEndCounter;
    private bool done;
    private static int[] playerOrder;
    private static int currentPlayerOrderIndex;
    private static int lastNumberOfPlayer = 0;


    internal static void Load()
    {
      On.TowerFall.RoundLogic.FFACheckForAllButOneDead += FFACheckForAllButOneDead_patch;
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

      if (MyPlayer.playTag[player.PlayerIndex])
      {
        this.Session.CurrentLevel.Ending = true;
        return;
      }

      if (!MyPlayer.playTagCountDownOn[player.PlayerIndex])
      {
        this.Session.CurrentLevel.Ending = true;
        return;
      }

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
        MyPlayer.playTagCountDown[p.PlayerIndex] = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayModePlayTag;
        MyPlayer.playTagCountDownOn[p.PlayerIndex] = true;
        MyPlayer.playTag[p.PlayerIndex] = false;
        MyPlayer.creationTime[p.PlayerIndex] = DateTime.Now;

        if (p.PlayerIndex == playerIndex)
        {
          MyPlayer.playTag[p.PlayerIndex] = true;
        }
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
