using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MiraAPI.Modifiers.Types;
using MiraAPI.PluginLoading;
using MiraAPI.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using Random = System.Random;

namespace MiraAPI.Modifiers;

/// <summary>
/// The manager for handling modifiers.
/// </summary>
public static class ModifierManager
{
    private static readonly Dictionary<uint, Type> IdToTypeModifierMap = [];
    private static readonly Dictionary<Type, uint> TypeToIdModifierMap = [];
    private static readonly Dictionary<int, List<uint>> PrioritiesToIdsMap = [];

    private static uint _nextTypeId;

    private static uint GetNextTypeId()
    {
        _nextTypeId++;
        return _nextTypeId;
    }

    /// <summary>
    /// Gets the modifier type from the type id.
    /// </summary>
    /// <param name="id">The ID.</param>
    /// <returns>The Type of the modifier.</returns>
    public static Type? GetModifierType(uint id)
    {
        return IdToTypeModifierMap.GetValueOrDefault(id);
    }

    /// <summary>
    /// Gets the modifier id from the type.
    /// </summary>
    /// <param name="type">The Type.</param>
    /// <returns>The ID of the modifier.</returns>
    public static uint? GetModifierTypeId(Type type)
    {
        if (!TypeToIdModifierMap.TryGetValue(type, out var id))
        {
            return null;
        }

        return id;
    }

    internal static bool RegisterModifier(Type modifierType, MiraPluginInfo info)
    {
        if (!typeof(BaseModifier).IsAssignableFrom(modifierType))
        {
            return false;
        }

        IdToTypeModifierMap.Add(GetNextTypeId(), modifierType);
        TypeToIdModifierMap.Add(modifierType, _nextTypeId);

        BaseModifier modifier;
        if (modifierType.GetConstructor(Type.EmptyTypes) != null)
        {
            modifier = (BaseModifier)Activator.CreateInstance(modifierType)!;
        }
        else
        {
            // this is probably terrible but its good enough for now.
            modifier = (BaseModifier)FormatterServices.GetUninitializedObject(modifierType);
        }

        info.Modifiers.Add(modifier);

        if (modifier is not GameModifier gameModifier)
        {
            return true;
        }

        if (modifierType.GetConstructor(Type.EmptyTypes) == null)
        {
            Logger<MiraApiPlugin>.Error($"Game Modifier {modifierType.FullName} does not have a parameterless constructor!");
            return false;
        }

        var priority = gameModifier.Priority();

        if (!PrioritiesToIdsMap.TryGetValue(priority, out var list))
        {
            PrioritiesToIdsMap[priority] = list = [];
        }

        list.Add(_nextTypeId);
        return true;
    }

    internal static void AssignModifiers(List<PlayerControl> plrs)
    {
        var rand = new Random();

        foreach (var priority in PrioritiesToIdsMap.Keys.OrderByDescending(x => x))
        {
            var filteredModifiers = new List<uint>();

            foreach (var id in PrioritiesToIdsMap[priority])
            {
                var mod = Activator.CreateInstance(IdToTypeModifierMap[id]) as GameModifier;
                var chance = Math.Clamp(mod!.GetAssignmentChance(), 0, 100);
                var count = mod.GetAmountPerGame();

                if (chance == 0 || count == 0)
                {
                    continue;
                }

                var maxCount = plrs.Count(x => IsGameModifierValid(x, mod, id));

                if (maxCount == 0)
                {
                    Logger<MiraApiPlugin>.Warning($"No players are valid for {mod.ModifierName}");
                    continue;
                }

                var num = Math.Clamp(count, 0, maxCount);

                for (var i = 0; i < num; i++)
                {
                    var randomNum = rand.Next(100);

                    if (randomNum < chance)
                    {
                        filteredModifiers.Add(id);
                    }
                }
            }

            if (filteredModifiers.Count == 0)
            {
                Logger<MiraApiPlugin>.Warning($"No filtered modifiers for priority {priority}");
                continue;
            }

            var shuffledList = filteredModifiers.Randomize();

            if (shuffledList.Count > plrs.Count)
            {
                shuffledList = shuffledList.GetRange(0, plrs.Count);
            }

            foreach (var id in shuffledList)
            {
                var mod = Activator.CreateInstance(IdToTypeModifierMap[id]) as GameModifier;
                var plr = plrs.Where(x => IsGameModifierValid(x, mod!, id)).Random();

                if (!plr || plr == null)
                {
                    Logger<MiraApiPlugin>.Warning($"Somehow valid players for modifier {mod!.ModifierName} disappeared");
                }
                else
                {
                    plr.RpcAddModifier(id);
                }
            }
        }
    }

    private static bool IsGameModifierValid(PlayerControl player, GameModifier modifier, uint modifierId)
    {
        return (player.Data.Role is not ICustomRole role || role.IsModifierApplicable(modifier)) &&
               modifier.IsModifierValidOn(player.Data.Role) && !player.HasModifier(modifierId);
    }
}
