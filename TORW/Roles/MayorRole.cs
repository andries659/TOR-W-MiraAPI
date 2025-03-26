using MiraAPI.Roles;
using UnityEngine;

namespace TORW.Roles;

public class MayorRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Mayor";
    public string RoleLongDescription => "You get extra votes!";
    public string RoleDescription => RoleLongDescription;
    public Color RoleColor => new Color32(221, 176, 152, 255);
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    
    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = TORWAssets.MayorIcon,
        DefaultChance = 10,
        DefaultRoleCount = 1,
        CanModifyChance = true,
    };
}
