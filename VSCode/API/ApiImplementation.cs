using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag;

public class ApiImplementation : IPlayTagApi
{
  /// <summary>
  /// Vrai quand la partie EN COURS est une partie de chat.
  ///
  /// C'est la session du niveau qu'on interroge et non les reglages du menu : ces
  /// derniers decrivent ce qui sera lance la prochaine fois, pas ce qui se joue.
  /// L'appelant type - une IA, en plein match - a besoin de la seconde reponse.
  /// </summary>
  public bool IsPlayTagMatch()
  {
    if (PlayTagGameMode.PlayTagMode == null)
    {
      return false;
    }

    Session session = (Engine.Instance?.Scene as Level)?.Session;
    return session?.MatchSettings != null
        && session.MatchSettings.Mode == PlayTagGameMode.PlayTagMode.Modes;
  }

  public bool IsTagged(int playerIndex)
  {
    return MyPlayer.playTag.TryGetValue(playerIndex, out bool tagged) && tagged;
  }

  public int TaggedPlayer()
  {
    foreach (var entry in MyPlayer.playTag)
    {
      if (entry.Value)
      {
        return entry.Key;
      }
    }

    return -1;
  }
}
