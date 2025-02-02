using System;
using Microsoft.Xna.Framework;
using TowerFall;
using static TowerFall.PauseMenu;

namespace TFModFortRiseGameModePlaytag
{
  public class MyPauseMenu
  {
    public static DateTime creationTime;
    public static Level mylevel;

    internal static void Load()
    {
      On.TowerFall.PauseMenu.ctor += ctor_patch;
      On.TowerFall.PauseMenu.Resume += Resume_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.PauseMenu.ctor -= ctor_patch;
      On.TowerFall.PauseMenu.Resume -= Resume_patch;
    }

    public MyPauseMenu() {}

    public static void ctor_patch(On.TowerFall.PauseMenu.orig_ctor origin, PauseMenu self, Level level, Vector2 position, MenuType menuType, int controllerDisconnected = -1) {
      origin(self, level, position, menuType, controllerDisconnected);
      mylevel = level; 
      creationTime = DateTime.Now;
    }

    public static void Resume_patch(On.TowerFall.PauseMenu.orig_Resume origin, PauseMenu self)
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
      origin(self);
    }
  }
}
