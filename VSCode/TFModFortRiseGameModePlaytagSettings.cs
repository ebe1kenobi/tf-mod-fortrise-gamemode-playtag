using System.Configuration;
using FortRise;

namespace TFModFortRiseGameModePlaytag
{
  public class TFModFortRiseGameModePlaytagSettings: ModuleSettings
  {

    public override void Create(ISettingsCreate settings)
    {
      settings.CreateOnOff("Pickup activated even \n\nwhen variant is not selected", playTagPickupActivated, (x) => playTagPickupActivated = x);
      settings.CreateNumber("Treasure Rate 1 chance on N, choose N", treasureRate, (x) => treasureRate = x, 0, 100);
      settings.CreateNumber("Delay Pickup", playTagDelayPickup, (x) => playTagDelayPickup = x, 1, 60);
      settings.CreateNumber("Delay Game Mode", playTagDelayModePlayTag, (x) => playTagDelayModePlayTag = x, 0, 60);
      settings.CreateOptions("Periodicity", periodicity, ["Normal", "Test"], (x) => periodicity = x.Item1);

    }

    //[SettingsName("Pickup activated even \n\nwhen variant is not selected")]
    public bool playTagPickupActivated { get; set; } = false;

    //[SettingsName("Treasure Rate 1 chance on N, choose N")]
    //[SettingsNumber(10, 100)]
    public int treasureRate { get; set; } = 100;

    //[SettingsName("Delay Pickup")]
    //[SettingsNumber(1, 60)]
    public int playTagDelayPickup { get; set; } = 15;

    //[SettingsName("Delay Game Mode")]
    //[SettingsNumber(1, 60)]
    public int playTagDelayModePlayTag { get; set; } = 20;

    //public const int OncePerMatch = 0;
    //public const int OncePerRound = 1;
    //public const int Test = 2;
    //[SettingsOptions("OncePerMatch", "OncePerRound", "Test")]
    public string periodicity { get; set; } = "Normal";
  }
}
