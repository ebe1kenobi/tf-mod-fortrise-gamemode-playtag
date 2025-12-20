using FortRise;
using HarmonyLib;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class MyLevel : IHookable
  {

    public static void Load(IHarmony harmony)
    {
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(Level), nameof(Level.Update)),
          prefix: new HarmonyMethod(Update_patch)
      );
    }

    public static void Update_patch(Level __instance) {
      TFModFortRiseGameModePlaytagModule.Update();
    }

  }
}
