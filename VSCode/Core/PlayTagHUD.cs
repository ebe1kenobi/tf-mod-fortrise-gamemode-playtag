using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  /// <summary>
  /// Le decompte au-dessus du joueur marque. C'est l'indicateur du mode : il dit a la
  /// fois QUI a le tag et combien de temps il lui reste.
  ///
  /// Le composant est VISIBLE, donc dessine par Monocle avec l'archer auquel il est
  /// attache. Il ne l'etait pas : il fallait que quelqu'un appelle son Render a la
  /// main, et c'etait un postfix sur <c>Player.HUDRender</c> - une methode de trois
  /// lignes, donc de celles que le JIT recopie dans son appelant. Le jour ou il
  /// choisit de le faire, le patch ne part plus, le decompte disparait, et - c'est le
  /// pire - la manche ne se termine jamais.
  ///
  /// Car la LOGIQUE vivait ici aussi : c'est le rendu qui declenchait l'explosion et
  /// remettait les drapeaux a zero. Un rendu qui ne part pas emportait donc la regle
  /// du jeu avec lui. Elle est maintenant dans <see cref="MyPlayer.Update"/>, ou elle
  /// aurait toujours du etre : ici on ne fait plus que dessiner.
  /// </summary>
  public class PlayTagHUD : Component
  {
    public static readonly Color TriggerColorA = Calc.HexToColor("FF2E16");

    private Player player;
    private readonly Color triggerColor;

    public PlayTagHUD()
      : base(true, true)
    {
      this.triggerColor = ArrowHUD.TriggerColorA;
    }

    public override void Added()
    {
      base.Added();
      this.player = this.Entity as Player;
    }

    public override void Removed()
    {
      base.Removed();
      this.player = null;
    }

    public override void Update()
    {
    }

    public override void Render()
    {
      // Visible pour tout le monde, donc c'est ICI qu'on decide de ne rien dessiner :
      // seul le joueur marque porte le decompte, et seulement tant qu'il tourne.
      if (!MyPlayer.ShowsCountdown(player))
      {
        return;
      }

      Draw.OutlineTextCentered(TFGame.Font,
          MyPlayer.playTagCountDown[player.PlayerIndex].ToString(),
          Calc.Floor(player.Position + new Vector2(0f, -15f)),
          triggerColor, new Vector2(1.8f, 1.8f));
    }
  }
}
