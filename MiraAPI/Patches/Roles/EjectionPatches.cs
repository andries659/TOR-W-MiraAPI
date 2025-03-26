using HarmonyLib;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Roles;

namespace MiraAPI.Patches.Roles;

/// <summary>
/// Patches for custom ejection messages.
/// </summary>
[HarmonyPatch(typeof(ExileController))]
public static class EjectionPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ExileController.Begin))]
    public static void BeginPostfix(ExileController __instance)
    {
        var @event = new EjectionEvent(__instance);
        MiraEventManager.InvokeEvent(@event);

        if (!__instance.initData.networkedPlayer || !__instance.initData.networkedPlayer.Role ||
            __instance.initData.networkedPlayer.Role is not ICustomRole role)
        {
            return;
        }

        if (role.GetCustomEjectionMessage(__instance.initData.networkedPlayer) == null)
        {
            return;
        }

        __instance.completeString = role.GetCustomEjectionMessage(__instance.initData.networkedPlayer);
    }
}
