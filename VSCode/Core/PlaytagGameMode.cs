using FortRise;
using Microsoft.Xna.Framework;
using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  public class PlayTagGameMode : IVersusGameMode, IRegisterable
  {
    public string Name => "PlayTag";
    public Color NameColor => Color.LightPink;
    public ISubtextureEntry Icon => PlayTagIcon;
    public ISubtextureEntry PlayTagIcon => TextureRegistry.PlayTag; // todo add TFGame.MenuAtlas["gameModes/warlord"]
    public static IVersusGameModeEntry PlayTagMode { get; private set; } = null!;
    public bool IsTeamMode => false;

    public void OnStartGame(Session session)
    {
      //var playerCount = EightPlayerUtils.GetMenuPlayerCount();
      //totalLives = new int[playerCount];
      //var goal = session.MatchSettings.GoalScore;

      //for (int i = 0; i < playerCount; i++)
      //{
      //  if (TFGame.Players[i])
      //  {
      //    session.Scores[i] = goal;
      //    session.OldScores[i] = goal;
      //    totalLives[i] = goal;
      //  }
      //  else
      //  {
      //    totalLives[i] = -1;
      //  }
      //}
    }

    public static void Register(IModContent content, IModRegistry registry)
    {
      //BaronIcon = registry.Subtextures.RegisterTexture(
      //    content.Root.GetRelativePath("Content/gameModes/baron.png")  //todo
      //);
      PlayTagMode = registry.GameModes.RegisterVersusGameMode(new PlayTagGameMode());
    }

    public RoundLogic OnCreateRoundLogic(Session session)
    {
      return new PlaytagRoundLogic(session);
    }
  }

  //public class PlayTag : CustomGameMode
  //{
  //  public override void StartGame(Session session)
  //  {
  //  }

  //  public override RoundLogic CreateRoundLogic(Session session)
  //  {
  //    return new PlaytagRoundLogic(session);
  //  }

  //  public override void Initialize()
  //  {
  //    Icon = TFGame.MenuAtlas["gameModes/warlord"];
  //    NameColor = Color.LightPink;
  //    CoinOffset = 12;
  //  }

  //  public override void InitializeSounds() { }
  //}
}
