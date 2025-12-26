using FortRise;
using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag;

// Optional way to use textures
public class TextureRegistry : IRegisterable
{
    // Variants
    public static ISubtextureEntry PlayTagGameMode { get; private set; } = null!;
    public static ISubtextureEntry PlayTagVariant { get; private set; } = null!;

    public static void Register(IModContent content, IModRegistry registry)
    {
    PlayTagVariant = registry.Subtextures.RegisterTexture(
                content.Root.GetRelativePath("Content/Atlas/variant.png")
            );
    PlayTagGameMode = registry.Subtextures.RegisterTexture(
                content.Root.GetRelativePath("Content/Atlas/gamemode.png")
            );
  }
}