using System;
using System.IO;
using System.Reflection;
using System.Xml;
using FortRise;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod.ModInterop;
using MonoMod.Utils;
using TowerFall;

namespace TFModFortRiseGameModePlaytag
{
  [Fort("com.ebe1.kenobi.tfmodfortrisegamemodeplaytag", "TFModFortRiseGameModePlaytag")]
  public class TFModFortRiseGameModePlaytagModule : FortModule
  {
    public static TFModFortRiseGameModePlaytagModule Instance;

    public override Type SettingsType => typeof(TFModFortRiseGameModePlaytagSettings);
    public TFModFortRiseGameModePlaytagSettings Settings => (TFModFortRiseGameModePlaytagSettings)Instance.InternalSettings;

    public TFModFortRiseGameModePlaytagModule() 
    {
        Instance = this;
    }

    public override void LoadContent()
    {
      
    }

    public override void Load()
    {
      MyPlayer.Load();
      PlaytagRoundLogic.Load();
    }

    public override void Unload()
    {
      MyPlayer.Unload();
      PlaytagRoundLogic.Unload();
    }
  }
}
