﻿using UnityEngine;

namespace MiraAPI.Utilities.Assets;

public static class MiraAssets
{
    public static readonly LoadableResourceAsset Empty = new("MiraAPI.Resources.Empty.png");

    public static readonly LoadableResourceAsset NextButton = new("MiraAPI.Resources.NextButton.png");
    public static readonly LoadableResourceAsset NextButtonActive = new("MiraAPI.Resources.NextButtonActive.png");
    public static readonly LoadableResourceAsset Cog = new("MiraAPI.Resources.Cog.png");
    public static readonly LoadableResourceAsset Checkmark = new("MiraAPI.Resources.Checkmark.png");
    public static readonly LoadableResourceAsset CategoryHeader = new("MiraAPI.Resources.CategoryHeader.png");
    public static readonly LoadableResourceAsset CheckmarkBox = new("MiraAPI.Resources.CheckMarkBox.png");
    public static readonly Color32 AcceptedTeal = new(43, 233, 198, 255);
}
