using MiraAPI.Utilities.Assets;

namespace TORW;

public static class TORWAssets
{
    public static LoadableResourceAsset ExampleButton { get; } = new("TORW.Resources.ExampleButton.png");

    // Credit to EpicHorrors for the teleport button asset.
    public static LoadableResourceAsset TeleportButton { get; } = new("TORW.Resources.TeleportButton.png");
    public static LoadableResourceAsset TeleporterBanner { get; } = new("TORW.Resources.Banner1.png");
    public static LoadableResourceAsset FreezerBanner { get; } = new("TORW.Resources.Banner2.png");
}
