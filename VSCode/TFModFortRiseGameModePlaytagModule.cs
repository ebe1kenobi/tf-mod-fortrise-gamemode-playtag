using System;
using System.IO;
using System.Reflection;
using System.Xml;
using FortRise;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod.ModInterop;
using MonoMod.Utils;
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

    public override Type SettingsType => typeof(TFModFortRiseGameModePlaytagSettings);
    public static TFModFortRiseGameModePlaytagSettings Settings => (TFModFortRiseGameModePlaytagSettings)Instance.InternalSettings;

    public TFModFortRiseGameModePlaytagModule() 
    {
        Instance = this;
        Logger.Init("PlaytagLOG");
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
    }

    public override void Unload()
    {
      MyPlayer.Unload();
      PlaytagRoundLogic.Unload();
      MyTreasureSpawner.Unload();
      MyPickup.Unload();
      MySession.Unload();
      MyPauseMenu.Unload();
    }



    public static void StartPlayTagEffect(Player player)
    {
      currentPlayer = player;
      currentSong = Music.CurrentSong;
      Music.Stop();
      Sounds.boss_humanLaugh.Play(player.X);
      player.Level.LightingLayer.SetSpotlight((LevelEntity)player);
      //player.Level.Session.CurrentLevel.LightingLayer.SetSpotlight((LevelEntity)player);
      Engine.TimeRate = 0.1f;
      pause.Set(10);
    }
    public static void StopPlayTagEffect()
    {
      Music.Play(currentSong);
      currentPlayer.Level.LightingLayer.CancelSpotlight();
      //currentPlayer.Level.Session.CurrentLevel.LightingLayer.CancelSpotlight();
      Engine.TimeRate = 1f;
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
}
