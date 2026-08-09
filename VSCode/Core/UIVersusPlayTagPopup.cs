using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  /// <summary>
  /// Popup de reglage du delai avant explosion, ouverte avec Y depuis le bouton de
  /// mode quand PlayTag est selectionne. Meme principe que la popup handicap : le
  /// reglage existe deja dans les options du mod, mais on veut pouvoir l'ajuster
  /// juste avant de lancer le match, sans quitter le menu versus.
  ///
  /// Elle ecrit directement dans le reglage playTagDelayModePlayTag, celui que lit
  /// PlaytagRoundLogic au demarrage de chaque round.
  /// </summary>
  public class UIVersusPlayTagPopup : Entity
  {
    // Memes bornes que le reglage expose dans les options du mod.
    private const int MinDelay = 0;
    private const int MaxDelay = 60;

    private readonly BorderButton ownerButton;

    public static UIVersusPlayTagPopup Current;
    public static bool IsOpen => Current != null && Current.Scene == Engine.Instance.Scene;

    public UIVersusPlayTagPopup(BorderButton ownerButton)
    {
      this.ownerButton = ownerButton;
      Position = new Vector2(160f, 120f);
    }

    public override void Added()
    {
      base.Added();
      Current = this;
      if (ownerButton != null)
        ownerButton.Selected = false;

      // Bloque les entrees du menu en arriere-plan tant que la popup est ouverte :
      // sans cela, Back reviendrait a l'ecran precedent et Start lancerait le match
      // pendant qu'on regle le delai. Meme pattern que la popup handicap.
      MainMenu menu = Scene as MainMenu;
      if (menu != null)
        menu.CanAct = false;

      Sounds.ui_pause.Play(160f);
    }

    public override void Removed()
    {
      base.Removed();

      // Les reglages ne sont ecrits sur disque qu'en sortant du menu Options du
      // jeu : sans cet appel, une valeur changee ici serait perdue en quittant.
      TFModFortRiseGameModePlaytagModule.SaveSettingsNow();

      if (Current == this)
        Current = null;

      Sounds.ui_unpause.Play(160f);
      MenuInput.Clear();

      MainMenu menu = Scene as MainMenu;
      if (menu != null)
        menu.CanAct = true;

      if (ownerButton != null)
        ownerButton.Selected = true;
    }

    private static void AdjustDelay(int step)
    {
      var settings = TFModFortRiseGameModePlaytagModule.Settings;
      int value = settings.playTagDelayModePlayTag + step;

      if (value < MinDelay) value = MinDelay;
      if (value > MaxDelay) value = MaxDelay;

      if (value == settings.playTagDelayModePlayTag)
      {
        Sounds.ui_invalid.Play(160f, 1f);
        return;
      }

      settings.playTagDelayModePlayTag = value;
      Sounds.ui_click.Play(160f, 1f);
    }

    public override void Update()
    {
      base.Update();
      MenuInput.Update();

      if (MenuInput.Left)
      {
        AdjustDelay(-1);
        return;
      }

      if (MenuInput.Right)
      {
        AdjustDelay(1);
        return;
      }

      // Pas a 5 s pour traverser la plage rapidement.
      if (MenuInput.Down)
      {
        AdjustDelay(-5);
        return;
      }

      if (MenuInput.Up)
      {
        AdjustDelay(5);
        return;
      }

      if (MenuInput.Confirm || MenuInput.Back)
        RemoveSelf();
    }

    public override void Render()
    {
      Draw.Rect(0, 0, 320, 240, Color.Black * 0.7f);

      Draw.OutlineTextCentered(TFGame.Font, "PLAY TAG",
          Position + new Vector2(0f, -60f), Color.White, 2f);

      Draw.TextCentered(TFGame.Font, "LEFT/RIGHT: +/- 1s   UP/DOWN: +/- 5s",
          Position + new Vector2(0f, -40f), Color.Gray);

      Draw.TextCentered(TFGame.Font, "EXPLOSION DELAY",
          Position + new Vector2(0f, -8f), Color.White);

      int delay = TFModFortRiseGameModePlaytagModule.Settings.playTagDelayModePlayTag;
      Draw.OutlineTextCentered(TFGame.Font, delay + "S",
          Position + new Vector2(0f, 16f), Calc.HexToColor("FFEC5E"), 2f);

      Draw.TextCentered(TFGame.Font, "A/B: CLOSE",
          Position + new Vector2(0f, 52f), Color.Gray);
    }
  }
}
