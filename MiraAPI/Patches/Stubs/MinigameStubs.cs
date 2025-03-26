using System.Runtime.CompilerServices;
using HarmonyLib;

namespace MiraAPI.Patches.Stubs;

/// <summary>
/// Reverse patches for Minigames.
/// </summary>
[HarmonyPatch]
public static class MinigameStubs
{
    /// <summary>
    /// Reverse patch for Minigame.Begin.
    /// </summary>
    /// <param name="instance">The Minigame instance.</param>
    /// <param name="task">Associated PlayerTask.</param>
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Begin))]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Begin(Minigame instance, PlayerTask? task)
    {
        // nothing needed
    }

    /// <summary>
    /// Reverse patch for Minigame.Close.
    /// </summary>
    /// <param name="instance">The Minigame instance.</param>
    [HarmonyReversePatch]
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Close), [])]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Close(Minigame instance)
    {
        // nothing needed
    }

    /// <summary>
    /// Fixes the error when creating custom minigames. Credit to Submerged for discovering this fix.
    /// </summary>
    /// <param name="__instance">The Minigame instance.</param>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Minigame), nameof(Minigame.Begin))]
    public static void MinigameErrorFixPatch(Minigame __instance)
    {
        __instance.logger ??= new Logger("Minigame", Logger.Level.Info, Logger.Category.Gameplay);
    }
}
