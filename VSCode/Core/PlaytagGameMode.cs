using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class PlayTag : CustomGameMode
  {
    public override void StartGame(Session session)
    {
    }

    public override RoundLogic CreateRoundLogic(Session session)
    {
      return new PlaytagRoundLogic(session);
    }

    public override void Initialize()
    {
      Icon = TFGame.MenuAtlas["gameModes/warlord"];
      NameColor = Color.LightPink;
      CoinOffset = 12;
    }

    public override void InitializeSounds() { }
  }
}
