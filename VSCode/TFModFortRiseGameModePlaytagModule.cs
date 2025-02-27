﻿using System;
using System.IO;
using FortRise;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  [Fort("com.ebe1.kenobi.tfmodfortrisegamemodeplaytag", "TFModFortRiseGameModePlaytag")]
  public class TFModFortRiseGameModePlaytagModule : FortModule
  {
    public static TFModFortRiseGameModePlaytagModule Instance;
    public static Counter pause = new Counter();
    public static String currentSong;
    public static Player currentPlayer;
    public static float currentTimerate;
    

    public override Type SettingsType => typeof(TFModFortRiseGameModePlaytagSettings);
    public static TFModFortRiseGameModePlaytagSettings Settings => (TFModFortRiseGameModePlaytagSettings)Instance.InternalSettings;

    public TFModFortRiseGameModePlaytagModule() 
    {
        Instance = this;
        //Logger.Init("PlaytagLOG");
    }

    public override void LoadContent()
    {
      
    }

    public override void Load()
    {
      MyPlayer.Load();
      PlaytagRoundLogic.Load();
      MyTreasureSpawner.Load();
      MyPickup.Load();
      MySession.Load();
      MyPauseMenu.Load();
      MyLevel.Load();

      typeof(ModExports).ModInterop();
    }

    public override void Unload()
    {
      MyPlayer.Unload();
      PlaytagRoundLogic.Unload();
      MyTreasureSpawner.Unload();
      MyPickup.Unload();
      MySession.Unload();
      MyPauseMenu.Unload();
      MyLevel.Unload();
    }

    public static void StartPlayTagEffect(Player player)
    {
      currentPlayer = player;
      currentSong = Music.CurrentSong;
      currentTimerate = Engine.TimeRate;
      Music.Stop();
      Sounds.boss_humanLaugh.Play(player.X);
      player.Level.LightingLayer.SetSpotlight((LevelEntity)player);
      for (int i = 0; i < TFGame.Players.Length; i++)
      {
        if (TFGame.Players[i])
        {
          TFGame.PlayerInputs[i].Rumble(1f, 20);
        }
      }

      Engine.TimeRate = 0.1f;
      pause.Set(30);
    }

    public static void StopPlayTagEffect()
    {
      Music.Play(currentSong);
      currentPlayer.Level.LightingLayer.CancelSpotlight();
      //Engine.TimeRate = currentTimerate;
      Engine.TimeRate = 1.0f;
    }

    public static void Update()
    {
      if ((bool)pause)
      {
        pause.Update();
        if (!(bool)pause)
        {
          StopPlayTagEffect();
        }
      }
    }
  }

  [ModExportName("com.fortrise.TFModFortRiseGameModePlaytag")]
  public static class ModExports
  {
    public static bool IsGameModePlayTag(Modes mode) => mode == ModRegisters.GameModeType<PlayTag>();
    public static bool IsPlayTagCountDownOn(int playerIndex) => MyPlayer.playTagCountDownOn[playerIndex];
    public static bool IsPlayerPlayTag(int playerIndex) => MyPlayer.playTag[playerIndex];
  }
}
