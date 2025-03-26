using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using UnityEngine;

namespace TORW.Options;

public class TORWOptions : AbstractOptionGroup
{
    public override string GroupName => "TOR-W Mod Options";
    public override Color GroupColor => new Color32(3, 252, 240, 255);

    [ModdedToggleOption($"This option is soon to come!\n<size=70%>(Dark Chat Menu)</size>")]
    public bool ToggleOpt { get; set; } = false;
}
