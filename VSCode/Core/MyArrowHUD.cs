using FortRise;
using HarmonyLib;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  /// <summary>
  /// Cache le compteur de fleches pendant un match PlayTag : le decompte de la bombe
  /// s'affiche au meme endroit, au-dessus de l'archer, et les deux se chevauchaient.
  ///
  /// C'est <c>ArrowHUD.Render</c> qui est saute, et non <c>Player.HUDRender</c> qui
  /// l'appelle. Cette derniere tient en trois lignes : elle est de celles que le JIT
  /// recopie dans son appelant, auquel cas le patch ne part jamais. C'est ce qui est
  /// arrive - le compteur de fleches revenait, mais surtout le decompte disparaissait
  /// et la manche ne finissait plus.
  ///
  /// Prendre la methode du dessous evite le pari. Elle laisse au passage l'indicateur
  /// de joueur s'afficher tout seul, alors qu'il fallait le redessiner a la main quand
  /// on sautait HUDRender en entier.
  ///
  /// Le mod Soccer fait exactement la meme chose sur la meme methode. Les deux
  /// cohabitent : chacun ne rend faux que dans SON mode, donc celui qui passe en
  /// premier laisse toujours l'autre s'exprimer.
  /// </summary>
  public class MyArrowHUD : IHookable
  {
    public static void Load(IHarmony harmony)
    {
      // Prefix, pour POUVOIR sauter le rendu vanilla - un postfix ne le permet pas.
      harmony.Patch(
          AccessTools.DeclaredMethod(typeof(ArrowHUD), nameof(ArrowHUD.Render)),
          prefix: new HarmonyMethod(Render_patch)
      );
    }

    public static bool Render_patch()
    {
      var level = Monocle.Engine.Instance?.Scene as Level;

      if (level?.Session?.MatchSettings == null || PlayTagGameMode.PlayTagMode == null)
      {
        return true;
      }

      return level.Session.MatchSettings.Mode != PlayTagGameMode.PlayTagMode.Modes;
    }
  }
}
