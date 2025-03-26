using MiraAPI.Roles;
using UnityEngine;

namespace TORW.Roles;

public class TeleporterRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Teleporter";
    public string RoleLongDescription => "Zoom out and teleport across the map!";
    public string RoleDescription => RoleLongDescription;
    public Color RoleColor => new Color32(66, 135, 245, 255);
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        OptionsScreenshot = TORWAssets.TeleporterBanner,
        Icon = TORWAssets.TeleportIcon,
        DefaultChance = 10,
        DefaultRoleCount = 1,
        CanModifyChance = true,
    };
}
