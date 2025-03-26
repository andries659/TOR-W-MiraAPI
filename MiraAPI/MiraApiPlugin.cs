using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime;
using MiraAPI.PluginLoading;
using MiraAPI.Roles;
using MonoMod.Utils;
using Reactor;
using Reactor.Networking;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using Delegate = Il2CppSystem.Delegate;

namespace MiraAPI;

/// <summary>
/// The main plugin class for Mira API.
/// </summary>
[BepInAutoPlugin("mira.api", "MiraAPI")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(ModFlags.RequireOnAllClients)]
public partial class MiraApiPlugin : BasePlugin
{
    public static Color MiraColor { get; } = new Color32(238, 154, 112, 255);
    public static Color DefaultHeaderColor = new Color32(77, 77, 77, 255);
    private static MiraPluginManager? PluginManager { get; set; }
    internal Harmony Harmony { get; } = new(Id);

    /// <inheritdoc />
    public override void Load()
    {
        Harmony.PatchAll();

        ReactorCredits.Register("Mira API", Version, true, ReactorCredits.AlwaysShow);

        PluginManager = new MiraPluginManager();
        PluginManager.Initialize();
    }
}
