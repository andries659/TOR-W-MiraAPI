using MiraAPI.Roles;
using UnityEngine;

namespace TORW.Roles;

[RegisterCustomRole]
public class TeleporterRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Teleporter";
    public string RoleLongDescription => "Zoom out and teleport across the map!";
    public string RoleDescription => RoleLongDescription;
    public Color RoleColor => new Color32(221, 176, 152, 255);
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        OptionsScreenshot = TORWAssets.TeleporterBanner,
        CanModifyChance = true,
        DefaultChance = 10,
        DefaultRoleCount = 0,
    };
}
