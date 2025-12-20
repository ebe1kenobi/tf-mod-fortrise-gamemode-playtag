using FortRise;

namespace TFModFortRiseGameModePlaytag
{
  public class Variants : IRegisterable
  {
    public static IVariantEntry PlayTag = null!;

    public static void Register(IModContent content, IModRegistry registry)
    {
      PlayTag = registry.Variants.RegisterVariant("PlayTag", new()
      {
        Title = "PlayTag",
        Flags = CustomVariantFlags.None,
        Icon = TextureRegistry.PlayTag
      });
    }
  }
}
