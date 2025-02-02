namespace TFModFortRiseGameModePlaytag
{
  public class MySession
  {
    public static int NbPlayTagPickupActivated { get; set; }

    internal static void Load()
    {
      On.TowerFall.Session.StartGame += StartGame_patch;
    }

    internal static void Unload()
    {
      On.TowerFall.Session.StartGame -= StartGame_patch;
    }
    public MySession()
    {
    }

    public static void StartGame_patch(On.TowerFall.Session.orig_StartGame orig, global::TowerFall.Session self)
    {
      NbPlayTagPickupActivated = 0;
      orig(self);
    }
  }
}
