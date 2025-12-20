
using FortRise;
using HarmonyLib;
using TowerFall;

namespace TFModFortRiseGameModePlaytag;

public class MyRoundLogic : IHookable
{
    public static void Load(IHarmony harmony)
    {
        harmony.Patch(
            AccessTools.DeclaredMethod(typeof(RoundLogic), nameof(RoundLogic.FFACheckForAllButOneDead)),
            prefix: new HarmonyMethod(RoundLogic_FFACheckForAllButOneDead_Prefix)
        );
    }

    private static bool RoundLogic_FFACheckForAllButOneDead_Prefix(RoundLogic __instance, ref bool __result)
    {
        if (__instance is PlaytagRoundLogic)
        {
            __result = false;
            return false;
        }

        return true;
  }
}