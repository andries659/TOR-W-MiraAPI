using System.Linq;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using MiraAPI;
using MiraAPI.Networking;
using MiraAPI.GameOptions;
using MiraAPI.PluginLoading;
using MiraAPI.Utilities;
using Reactor;
using Reactor.Networking.Attributes;
using Reactor.Utilities;
using HarmonyLib;
using Sentry.Unity;

namespace TORW;

[BepInAutoPlugin("com.andries659.torw-miraapi", "Town Of Re-Worked", "1.0.0")]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
public partial class TORW : BasePlugin, IMiraPlugin
{
    public Harmony Harmony { get; } = new(Id);
    public string OptionsTitleText => "Town Of\nRe-Worked";
    public ConfigFile GetConfigFile() => Config;
    public override void Load()
    {
        Harmony.PatchAll();
        ReactorCredits.Register<TORW>(ReactorCredits.AlwaysShow);
    }
}
