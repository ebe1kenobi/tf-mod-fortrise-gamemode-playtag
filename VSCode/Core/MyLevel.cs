using FortRise;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyLevel
  {

    internal static void Load()
    {
      On.TowerFall.Level.Update += Update_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Level.Update -= Update_patch;
    }

    public static void Update_patch(On.TowerFall.Level.orig_Update orig, global::TowerFall.Level self) {
      TFModFortRiseGameModePlaytagModule.Update();
      orig(self);
    }

  }
}
