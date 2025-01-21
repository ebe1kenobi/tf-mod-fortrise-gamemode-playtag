using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    [SettingsName("Pickup Activated")]
    public bool playTagPickupActivated = true;
  }
}
