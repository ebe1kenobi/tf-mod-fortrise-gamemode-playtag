namespace TFModFortRiseGameModePlaytag
{
  public class MySession
  {
    public static int NbPlayTagPickupActivated { get; set; }

    internal static void Load()
    {
      On.TowerFall.Session.StartGame += StartGame_patch;
      On.TowerFall.Session.GotoNextRound += GotoNextRound_patch;
      On.TowerFall.Session.ctor += ctor_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Session.StartGame -= StartGame_patch;
      On.TowerFall.Session.GotoNextRound -= GotoNextRound_patch;
      On.TowerFall.Session.ctor -= ctor_patch;
    }
    public MySession()
    {
    }

    public static void StartGame_patch(On.TowerFall.Session.orig_StartGame orig, global::TowerFall.Session self)
    {
      if (TFModFortRiseGameModePlaytagModule.Settings.periodicity == TFModFortRiseGameModePlaytagSettings.OncePerMatch)
      {
        NbPlayTagPickupActivated = 0;
      }
      orig(self);
    }

    public static void GotoNextRound_patch(On.TowerFall.Session.orig_GotoNextRound orig, global::TowerFall.Session self)
    {
      if (TFModFortRiseGameModePlaytagModule.Settings.periodicity == TFModFortRiseGameModePlaytagSettings.OncePerRound)
      {
        NbPlayTagPickupActivated = 0;
      }
      if (TFModFortRiseGameModePlaytagModule.Settings.periodicity == TFModFortRiseGameModePlaytagSettings.Test)
      {
        NbPlayTagPickupActivated = 0;
      }
      orig(self);
    }

    public static void ctor_patch(On.TowerFall.Session.orig_ctor orig, global::TowerFall.Session self, global::TowerFall.MatchSettings settings)
    {
      NbPlayTagPickupActivated = 0;
      orig(self, settings);
    }

  }
}
