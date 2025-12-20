using FortRise;
using Monocle;
using TowerFall;

namespace TFModFortRiseGameModePlaytag;

// Optional way to use textures
public class TextureRegistry : IRegisterable
{
    // Variants
    public static ISubtextureEntry PlayTag { get; private set; } = null!;

    public static void Register(IModContent content, IModRegistry registry)
    {
    PlayTag = registry.Subtextures.RegisterTexture(
                content.Root.GetRelativePath("Content/Atlas/playtag.png")
            );
    }
}