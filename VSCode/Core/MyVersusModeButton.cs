using FortRise;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  /// <summary>
  /// Ouvre la popup de reglage du delai (Y sur le bouton de mode) quand PlayTag est
  /// selectionne, sur le meme principe que la popup handicap.
  /// </summary>
  public class MyVersusModeButton : IHookable
  {
    public static void Load(IHarmony harmony)
    {
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(VersusModeButton), nameof(VersusModeButton.Update)),
          prefix: new HarmonyMethod(Update_patch)
      );
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(VersusModeButton), nameof(VersusModeButton.Render)),
          postfix: new HarmonyMethod(Render_patch)
      );
      // Tant que la popup est ouverte, on ne demarre pas le match : la scene menu
      // serait remplacee sans que la popup soit fermee.
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(VersusMapButton), nameof(VersusMapButton.OnConfirm)),
          prefix: new HarmonyMethod(MapConfirm_patch)
      );
    }

    /// <summary>
    /// PlayTag est un mode enregistre par le mod : FortRise lui attribue une valeur
    /// de Modes, on compare dessus plutot que sur un nom.
    /// </summary>
    private static bool IsPlayTagMode(MatchSettings settings)
    {
      return settings != null
          && PlayTagGameMode.PlayTagMode != null
          && settings.Mode == PlayTagGameMode.PlayTagMode.Modes;
    }

    private static bool MapConfirm_patch(VersusMapButton __instance)
    {
      return !UIVersusPlayTagPopup.IsOpen;
    }

    private static bool AnyPlayerArrowsPressed()
    {
      for (int i = 0; i < TFGame.PlayerInputs.Length; i++)
      {
        PlayerInput input = TFGame.PlayerInputs[i];
        if (input != null && input.GetState().ArrowsPressed)
          return true;
      }

      return false;
    }

    private static bool Update_patch(VersusModeButton __instance)
    {
      if (!IsPlayTagMode(MainMenu.VersusMatchSettings))
        return true;

      if (__instance.Selected && !UIVersusPlayTagPopup.IsOpen && AnyPlayerArrowsPressed())
      {
        if (__instance.Scene != null)
        {
          Sounds.ui_click.Play(160f, 1f);
          __instance.Scene.Add(new UIVersusPlayTagPopup(__instance));
        }
        return false;
      }

      return true;
    }

    private static void Render_patch(VersusModeButton __instance)
    {
      if (!__instance.Selected || UIVersusPlayTagPopup.IsOpen)
        return;

      if (!IsPlayTagMode(MainMenu.VersusMatchSettings))
        return;

      Vector2 hintPos = __instance.Position + new Vector2(0f, 22f);
      Draw.OutlineTextCentered(TFGame.Font, "Y: DELAY", hintPos, Calc.HexToColor("FFEC5E"), 1f);
    }
  }
}
