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
        // Header commun a tous mes mods : sans lui FortRise retombe sur le nom du
        // mod et chacun cree sa propre colonne dans l'ecran des variantes.
        Header = "EBE1 MODS",
        Title = "PlayTag",
        Flags = CustomVariantFlags.None,
        Icon = TextureRegistry.PlayTagVariant
      });
    }
  }
}
