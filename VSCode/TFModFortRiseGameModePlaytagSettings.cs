using System.Configuration;
using FortRise;

namespace TFModFortRiseGameModePlaytag
{
  public class TFModFortRiseGameModePlaytagSettings: ModuleSettings
  {


    // FortRise n'ecrit les reglages qu'en SORTANT du menu Options
    // (MainMenu.DestroyOptions) : quitter le jeu depuis ce menu perdait la
    // modification. Chaque changement declenche donc une sauvegarde immediate.
    public override void Create(ISettingsCreate settings)
    {
      settings.CreateOnOff("Pickup activated even \n\nwhen variant is not selected", playTagPickupActivated, (x) => playTagPickupActivated = x);
      // Meme echelle que les quatre mods a pickup : des crans nommes d'apres les
      // objets du jeu plutot qu'un "1 chance sur N" maison, qui ignorait les
      // variantes et le jeu d'objets de la tour.
      settings.CreateOptions("Treasure rate", Rarity.LabelOf(treasureRarity), Rarity.Labels,
          (x) => { treasureRarity = x.Item2; TFModFortRiseGameModePlaytagModule.SaveSettingsNow(); });
      settings.CreateNumber("Delay Pickup", playTagDelayPickup, (x) => { playTagDelayPickup = x; TFModFortRiseGameModePlaytagModule.SaveSettingsNow(); }, 1, 60);
      settings.CreateNumber("Delay Game Mode", playTagDelayModePlayTag, (x) => { playTagDelayModePlayTag = x; TFModFortRiseGameModePlaytagModule.SaveSettingsNow(); }, 0, 60);
      settings.CreateOptions("Periodicity", periodicity, ["Normal", "Test"], (x) => periodicity = x.Item1);

    }

    //[SettingsName("Pickup activated even \n\nwhen variant is not selected")]
    public bool playTagPickupActivated { get; set; } = false;

    /// <summary>
    /// Ancien taux "1 chance sur N". Conserve pour que les fichiers de sauvegarde
    /// deja ecrits restent lisibles - System.Text.Json ne sait pas ignorer une
    /// propriete disparue, il leve.
    /// </summary>
    public int treasureRate { get; set; } = 100;

    /// <summary>
    /// Le cran d'apparition, index dans Rarity.Steps. Le defaut est celui de la
    /// bombe, l'objet rare du jeu.
    /// </summary>
    public int treasureRarity { get; set; } = Rarity.Default;

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
