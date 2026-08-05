using System;
using System.Diagnostics;
using System.IO;
using FortRise;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.ModInterop;
using TowerFall;


//todo when tag pass on other player, count is not the same
namespace TFModFortRiseGameModePlaytag
{
  public class TFModFortRiseGameModePlaytagModule : Mod
  {
    public static TFModFortRiseGameModePlaytagModule Instance;

    private static Type[] Registerables = [
        typeof(PlayTagPickup),
        typeof(PlayTagGameMode),
        typeof(TextureRegistry),
        typeof(Variants)

    ];
    internal Type[] Hookables = [
        typeof(MyLevel),
        typeof(MyVersusModeButton),
        typeof(MyPauseMenu),
        //typeof(MyPickup),
        typeof(MyPlayer),
        typeof(MyTreasureSpawner),
        typeof(MyRoundLogic),
    ];

    public static Counter pause = new Counter();
    public static String currentSong;
    public static Player currentPlayer;
    public static float currentTimerate;

    public static TFModFortRiseGameModePlaytagSettings Settings => Instance.GetSettings<TFModFortRiseGameModePlaytagSettings>()!;

    public TFModFortRiseGameModePlaytagModule(IModContent content, IModuleContext context, ILogger logger) : base(content, context, logger)
    {
      if (!Debugger.IsAttached)
      {
        //Debugger.Launch(); // Proposera d’attacher Visual Studio
      }
      Instance = this;
      TFModFortRiseGameModePlaytag.Logger.Init("Playtag");

      foreach (var hookable in Hookables)
      {
        hookable.GetMethod(nameof(IHookable.Load))!.Invoke(null, [context.Harmony]);
      }

      foreach (var registerable in Registerables)
      {
        registerable.GetMethod(nameof(IRegisterable.Register))!.Invoke(null, [content, context.Registry]);
      }
    }

    public override ModuleSettings CreateSettings()
    {
      return new TFModFortRiseGameModePlaytagSettings();
    }

    public static bool activated()
    {
      return Variants.PlayTag.IsActive() || Settings.playTagPickupActivated;
    }

    //public override void OnVariantsRegister(VariantManager manager, bool noPerPlayer = false)
    //{
      
    //  var icon = new CustomVariantInfo(
    //      "PlayTag", TFGame.MenuAtlas["variants/stealthArchers"],
    //      CustomVariantFlags.None
    //      );
    //  manager.AddVariant(icon);
    //}

    //public override void Load()
    //{
    //  MyPlayer.Load();
    //  PlaytagRoundLogic.Load();
    //  MyTreasureSpawner.Load();
    //  MyPickup.Load();
    //  MySession.Load();
    //  MyPauseMenu.Load();
    //  MyLevel.Load();

    //  typeof(ModExports).ModInterop();
    //}

    //public override void Unload()
    //{
    //  MyPlayer.Unload();
    //  PlaytagRoundLogic.Unload();
    //  MyTreasureSpawner.Unload();
    //  MyPickup.Unload();
    //  MySession.Unload();
    //  MyPauseMenu.Unload();
    //  MyLevel.Unload();
    //}
    //public override void OnVariantsRegister(VariantManager manager, bool noPerPlayer = false)
    //{
    //  var info1x1 = new CustomVariantInfo(
    //      "SpeedGamex1.1", VariantManager.GetVariantIconFromName("SpeedGamex1.1", SpeedAtlas),
    //      CustomVariantFlags.None
    //      );
    //  manager.AddVariant(info1x1);
    //}

    public static void EndPlayTag(Player player) {
      pause.Set(0);
      foreach (Player p in player.Level.Session.CurrentLevel[GameTags.Player])
      {
        MyPlayer.playTagCountDownOn[p.PlayerIndex] = false;
      }
      Player.ShootLock = false;
      // no currentSong on GameMode playtag
      if (currentSong != null) {
        Music.Play(currentSong);
      }
      Engine.TimeRate = 1.0f;
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

  //[ModExportName("com.fortrise.TFModFortRiseGameModePlaytag")]  //TODO
  //public static class ModExports
  //{
  //  public static bool IsGameModePlayTag(Modes mode) => mode == ModRegisters.GameModeType<PlayTag>();
  //  public static bool IsPlayTagCountDownOn(int playerIndex) => MyPlayer.playTagCountDownOn[playerIndex];
  //  public static bool IsPlayerPlayTag(int playerIndex) => MyPlayer.playTag[playerIndex];
  //}
}
