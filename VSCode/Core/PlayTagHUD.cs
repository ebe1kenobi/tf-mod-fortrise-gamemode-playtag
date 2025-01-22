
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class PlayTagHUD : Component
  {
    public static readonly Color TriggerColorA = Calc.HexToColor("FF2E16");
    private Player player;
    private Color triggerColor;

    public PlayTagHUD()
      : base(true, false)
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
      this.player = (Player) null;
    }

    public void ShowAtStart()
    {
        return;
    }

    public override void Update()
    {
    }

    public override void Render() 
    {
      if (MyPlayer.playTagCountDown[player.PlayerIndex] <= 0)
      { // Yes I know, it's so bad to put that here ...
        foreach (Player p in player.Level.Session.CurrentLevel[GameTags.Player])
        {
          MyPlayer.playTagCountDownOn[p.PlayerIndex] = false;
        }
        Player.ShootLock = false;
        Explosion.SpawnSuper(player.Level, player.Position, player.PlayerIndex, true);
      }

      Draw.OutlineTextCentered(TFGame.Font, MyPlayer.playTagCountDown[player.PlayerIndex].ToString(), Calc.Floor(player.Position + new Vector2(0f, -15f)), triggerColor, new Vector2(1.8f, 1.8f));
    }
  }
}
