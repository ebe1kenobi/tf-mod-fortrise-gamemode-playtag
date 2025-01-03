using FortRise;
using Microsoft.Xna.Framework;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class PlaytagGameMode : CustomGameMode
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

    //public override Sprite<int> CoinSprite()
    //{
    //  var sprite = new Sprite<int>(TFGame.Atlas["BaronMode/pickups/gemCoin"], 12, 10);

    //  sprite.Add(0, 0.1f, new int[] { 0, 0, 0, 1, 2, 3, 4, 5, 6, 7 });
    //  sprite.Play(0);
    //  sprite.CenterOrigin();
    //  return sprite;
    //}
  }
}
