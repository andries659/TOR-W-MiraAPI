using TORW.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;

namespace TORW.Options.Modifiers;

public class CaptainModifierSettings : AbstractOptionGroup<CaptainModifier>
{
    public override string GroupName => "Captain Settings";

    [ModdedNumberOption("Amount", 0, 5)]
    public float Amount { get; set; } = 1;

    [ModdedNumberOption("Chance", 0, 100, 10)]
    public float Chance { get; set; } = 50;

    [ModdedNumberOption("Uses", 1, 5)]
    public float NumUses { get; set; } = 3;
}
