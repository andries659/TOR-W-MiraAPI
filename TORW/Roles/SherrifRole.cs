using MiraAPI.Roles;
using UnityEngine;

namespace TORW.Roles;

[RegisterCustomRole]
public class SherrifRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Sherrif";
    public string RoleLongDescription => "As the Sherrif, your job is to find the player that you think is the Impostor\nIf yu shoot a crewmate, you and the crewmate die!";
    public string RoleDescription => "Shoot the player that you think is the Impostor!";
    public Color RoleColor => new Color32(239, 245, 66, 255);
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanModifyChance = true,
        DefaultChance = 10,
        DefaultRoleCount = 1,
        UseVanillaKillButton = true,
    };
}
