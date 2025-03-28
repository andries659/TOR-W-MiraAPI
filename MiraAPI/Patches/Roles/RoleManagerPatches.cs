﻿using System.Linq;
using AmongUs.GameOptions;
using HarmonyLib;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using MiraAPI.Roles;

namespace MiraAPI.Patches.Roles;

[HarmonyPatch(typeof(RoleManager))]
public static class RoleManagerPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(RoleManager.SetRole))]
    public static void SetRolePatch(RoleManager __instance, PlayerControl targetPlayer, RoleTypes roleType)
    {
        var @event = new SetRoleEvent(targetPlayer, roleType);
        MiraEventManager.InvokeEvent(@event);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(RoleManager.SelectRoles))]
    public static void ModifierSelectionPatches(RoleManager __instance)
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            return;
        }

        ModifierManager.AssignModifiers(
            PlayerControl.AllPlayerControls.ToArray().Where(plr => !plr.Data.IsDead && !plr.Data.Disconnected)
                .ToList());
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(RoleManager.AssignRoleOnDeath))]
    public static bool AssignRoleOnDeath(RoleManager __instance, [HarmonyArgument(0)] PlayerControl plr)
    {
        if (!plr || !plr.Data.IsDead)
        {
            return false;
        }

        if (plr.Data.Role is not ICustomRole role)
        {
            return true;
        }

        if (role.Configuration.GhostRole is RoleTypes.CrewmateGhost or RoleTypes.ImpostorGhost)
        {
            return true;
        }

        plr.RpcSetRole(role.Configuration.GhostRole);
        return false;
    }
}
