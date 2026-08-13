using FortRise;

namespace TFModFortRiseGameModePlaytag
{
  public class Variants : IRegisterable
  {
    public static IVariantEntry PlayTag = null!;

    /// <summary>
    /// Le libelle de la case, en un seul endroit. C'est le seul repere fiable pour
    /// reconnaitre NOTRE case dans l'ecran des variantes (voir MyVariantToggle).
    /// </summary>
    public const string TITLE = "PlayTag";

    public static void Register(IModContent content, IModRegistry registry)
    {
      PlayTag = registry.Variants.RegisterVariant(TITLE, new()
      {
        // Header commun a tous mes mods : sans lui FortRise retombe sur le nom du
        // mod et chacun cree sa propre colonne dans l'ecran des variantes.
        Header = "EBE1 MODS",
        Title = TITLE,
        Flags = CustomVariantFlags.None,
        Icon = TextureRegistry.PlayTagVariant
      });
    }
  }
}
