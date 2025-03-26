using MiraAPI.Utilities.Assets;

namespace TORW;

public static class TORWAssets
{
    public static LoadableResourceAsset ExampleButton { get; } = new("TORW.Resources.ExampleButton.png");
    public static LoadableResourceAsset ModLogo { get; } = new("TORW.Resources.Logo.png");
    public static LoadableResourceAsset TeleportButton { get; } = new("TORW.Resources.TeleportButton.png");
    public static LoadableResourceAsset TeleportIcon { get; } = new("TORW.Resources.TeleportIcon.png");
    public static LoadableResourceAsset MayorIcon { get; } = new("TORW.Resources.MayorIcon.png");
    public static LoadableResourceAsset TeleporterBanner { get; } = new("TORW.Resources.Banner1.png");
    public static LoadableResourceAsset JesterBanner { get; } = new("TORW.Resources.Banner2.png");
    public static LoadableResourceAsset SherrifBanner { get; } = new("TORW.Resources.Banner3.png");
    public static LoadableResourceAsset SwooperBanner { get; } = new("TORW.Resources.Banner4.png");
}
