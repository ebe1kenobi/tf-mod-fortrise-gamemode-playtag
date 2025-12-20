using System;
using FortRise;
using HarmonyLib;
using Microsoft.Xna.Framework;
using TowerFall;
using static TowerFall.PauseMenu;

namespace TFModFortRiseGameModePlaytag
{
  public class MyPauseMenu : IHookable
  {
    public static DateTime creationTime;
    public static Level mylevel;

    public static void Load(IHarmony harmony)
    {
      harmony.Patch(
          AccessTools.DeclaredConstructor(typeof(PauseMenu), [
                                                              typeof(Level),
                                                              typeof(Vector2),
                                                              typeof(MenuType),
                                                              typeof(int)
                                                                    ]),
          postfix: new HarmonyMethod(ctor_patch)
      );
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(PauseMenu), "Resume"),
          prefix: new HarmonyMethod(Resume_patch)
      );

    }

    public MyPauseMenu() {}

    public static void ctor_patch(PauseMenu __instance, Level level, Vector2 position, MenuType menuType, int controllerDisconnected = -1) {
      mylevel = level; 
      creationTime = DateTime.Now;
    }

    public static void Resume_patch(PauseMenu __instance)
    {
      int pauseDuration = (int)(DateTime.Now - creationTime).TotalSeconds;

      for (var i = 0; i < TFGame.Players.Length; i++)
      {
        Player p = mylevel.Session.CurrentLevel.GetPlayer(i);
        if (p != null)
        {
          MyPlayer.pauseDuration[p.PlayerIndex] += pauseDuration;
        }
      }
    }
  }
}
