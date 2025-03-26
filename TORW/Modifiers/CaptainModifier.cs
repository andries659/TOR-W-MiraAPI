using TORW.Options.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;

namespace TORW.Modifiers;

public class CaptainModifier : GameModifier
{
    public override string ModifierName => "Captain";

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<CaptainModifierSettings>.Instance.Chance;
    }

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<CaptainModifierSettings>.Instance.Amount;
    }
}
