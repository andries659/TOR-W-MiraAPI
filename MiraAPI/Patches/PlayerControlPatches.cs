using System.Linq;
using HarmonyLib;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Player;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using MiraAPI.Voting;
using Reactor.Utilities.Extensions;

namespace MiraAPI.Patches;

/// <summary>
/// General patches for the PlayerControl class.
/// </summary>
[HarmonyPatch(typeof(PlayerControl))]
public static class PlayerControlPatches
{
    /// <summary>
    /// Adds the modifier component to the player on start.
    /// </summary>
    /// <param name="__instance">PlayerControl instance.</param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.Start))]
    public static void PlayerControlStartPostfix(PlayerControl __instance)
    {
        if (__instance.gameObject.TryGetComponent<ModifierComponent>(out var modifierComp))
        {
            modifierComp.DestroyImmediate();
        }

        if (__instance.gameObject.TryGetComponent<PlayerVoteData>(out var voteComp))
        {
            voteComp.DestroyImmediate();
        }

        __instance.gameObject.AddComponent<ModifierComponent>();
        __instance.gameObject.AddComponent<PlayerVoteData>();
    }

    /// <summary>
    /// Calls the OnDeath method for all active modifiers.
    /// </summary>
    /// <param name="__instance">PlayerControl instance.</param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.Die))]
    public static void PlayerControlDiePostfix(PlayerControl __instance, DeathReason reason)
    {
        var deathEvent = new PlayerDeathEvent(__instance, reason);
        MiraEventManager.InvokeEvent(deathEvent);

        var modifiersComponent = __instance.GetComponent<ModifierComponent>();

        if (modifiersComponent)
        {
            modifiersComponent.ActiveModifiers.ForEach(x => x.OnDeath(reason));
        }
    }

    /// <summary>
    /// Used to trigger the <see cref="CompleteTaskEvent"/>.
    /// </summary>
    /// <param name="__instance">PlayerControl instance.</param>
    /// <param name="idx">The task id.</param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.CompleteTask))]
    public static void PlayerCompleteTaskPostfix(PlayerControl __instance, uint idx)
    {
        var playerTask = __instance.myTasks.ToArray().First(playerTask => playerTask.Id == idx);
        if (playerTask != null)
        {
            var completeTaskEvent = new CompleteTaskEvent(__instance, playerTask);
            MiraEventManager.InvokeEvent(completeTaskEvent);
        }
    }

    /// <summary>
    /// Used to trigger the <see cref="BeforeMurderEvent"/>.
    /// </summary>
    /// <param name="__instance">The source player.</param>
    /// <param name="target">The target.</param>
    /// <param name="didSucceed">Whether the kill succeeded.</param>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerControl.RpcMurderPlayer))]
    public static void PlayerControlMurderPrefix(PlayerControl __instance, PlayerControl target, ref bool didSucceed)
    {
        var beforeMurderEvent = new BeforeMurderEvent(__instance, target);
        MiraEventManager.InvokeEvent(beforeMurderEvent);

        didSucceed = beforeMurderEvent.IsCancelled;
    }

    /// <summary>
    /// FixedUpdate handler for custom roles and custom buttons.
    /// </summary>
    /// <param name="__instance">PlayerControl instance.</param>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PlayerControl.FixedUpdate))]
    public static void PlayerControlFixedUpdatePostfix(PlayerControl __instance)
    {
        if (__instance.Data?.Role is ICustomRole customRole)
        {
            customRole.PlayerControlFixedUpdate(__instance);
        }

        if (!__instance.AmOwner)
        {
            return;
        }

        foreach (var button in CustomButtonManager.CustomButtons)
        {
            if (__instance.Data?.Role == null)
            {
                continue;
            }

            if (!button.Enabled(__instance.Data?.Role))
            {
                continue;
            }

            button.FixedUpdateHandler(__instance);
        }
    }

    /// <summary>
    /// Clear from modifier component cache.
    /// </summary>
    /// <param name="__instance">PlayerControl instance.</param>
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerControl.OnDestroy))]
    public static void PlayerControlOnDestroyPrefix(PlayerControl __instance)
    {
        ModifierExtensions.ModifierComponents.Remove(__instance);
    }
}
