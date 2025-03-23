using MiraAPI.Colors;
using UnityEngine;

namespace TORW
{
[RegisterCustomColors]
public static class TORWColors
{
    public static CustomColor SkyBlue { get; } = new CustomColor("Sky Blue", new Color32(135, 206, 235, 255), new Color32(70, 130, 180, 255));
    public static CustomColor Salmon { get; } = new CustomColor("Salmon", new Color32(250, 128, 114, 255), new Color32(233, 150, 122, 255));
    public static CustomColor Teal { get; } = new CustomColor("Teal", new Color32(0, 128, 128, 255), new Color32(0, 100, 100, 255));
    public static CustomColor Amber { get; } = new CustomColor("Amber", new Color32(255, 191, 0, 255), new Color32(255, 165, 0, 255));
    public static CustomColor Turquoise { get; } = new CustomColor("Turquoise", new Color32(64, 224, 208, 255), new Color32(72, 209, 204, 255));
    public static CustomColor SlateGray { get; } = new CustomColor("Slate Gray", new Color32(112, 128, 144, 255), new Color32(47, 79, 79, 255));
    public static CustomColor Periwinkle { get; } = new CustomColor("Periwinkle", new Color32(204, 204, 255, 255), new Color32(196, 196, 255, 255));
    public static CustomColor LimeGreen { get; } = new CustomColor("Lime Green", new Color32(50, 205, 50, 255), new Color32(34, 139, 34, 255));
    public static CustomColor Indigo { get; } = new CustomColor("Indigo", new Color32(75, 0, 130, 255), new Color32(54, 0, 102, 255));
    public static CustomColor Apricot { get; } = new CustomColor("Apricot", new Color32(251, 206, 177, 255), new Color32(255, 160, 122, 255));
    public static CustomColor Charcoal { get; } = new CustomColor("Charcoal", new Color32(54, 69, 79, 255), new Color32(70, 70, 70, 255));
    public static CustomColor Burgundy { get; } = new CustomColor("Burgundy", new Color32(128, 0, 32, 255), new Color32(100, 0, 20, 255));
    public static CustomColor Mustard { get; } = new CustomColor("Mustard", new Color32(255, 219, 88, 255), new Color32(255, 215, 0, 255));
    public static CustomColor Emerald { get; } = new CustomColor("Emerald", new Color32(80, 200, 120, 255), new Color32(0, 201, 87, 255));
    public static CustomColor Fuchsia { get; } = new CustomColor("Fuchsia", new Color32(255, 119, 255, 255), new Color32(255, 0, 255, 255));
    public static CustomColor NavyBlue { get; } = new CustomColor("Navy Blue", new Color32(0, 0, 128, 255), new Color32(0, 0, 102, 255));
}
}