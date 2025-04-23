using FortRise;

namespace TFModFortRiseGameModePlaytag
{
  public class TFModFortRiseGameModePlaytagSettings: ModuleSettings
  {
    [SettingsName("Delay Pickup")]
    [SettingsNumber(1, 60)]
    public int playTagDelayPickup = 15;

    [SettingsName("Delay Game Mode")]
    [SettingsNumber(1, 60)]
    public int playTagDelayModePlayTag = 20;

    [SettingsName("Pickup Always Activated")]
    public bool playTagPickupActivated = false;

    public const int OncePerMatch = 0;
    public const int OncePerRound = 1;
    public const int Test = 2;
    [SettingsOptions("OncePerMatch", "OncePerRound", "Test")]
    public int periodicity = 0;
  }
}
