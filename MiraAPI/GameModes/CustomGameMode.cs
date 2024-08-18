﻿using System.Collections.Generic;

namespace MiraAPI.GameModes;

/// <summary>
/// Base class for custom gamemodes
/// </summary>
public abstract class CustomGameMode
{
    /// <summary>
    /// Game mode name
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Game mode description
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Game mode ID
    /// </summary>
    public abstract int Id { get; }

    /// <summary>
    /// Called when Intro Cutscene is destroyed
    /// </summary>
    public virtual void Initialize() { }

    /// <summary>
    /// Called on HudManager.Start()
    /// </summary>
    /// <param name="instance">Instance of HudManager</param>
    public virtual void HudStart(HudManager instance) { }

    /// <summary>
    /// Called every frame on HudManager.Update()
    /// </summary>
    /// <param name="instance">Instance of HudManager</param>
    public virtual void HudUpdate(HudManager instance) { }

    /// <summary>
    /// Called when a player is killed
    /// </summary>
    /// <param name="player">PlayerControl that was killed</param>
    public virtual void OnDeath(PlayerControl player) { }

    /// <summary>
    /// Calculate Game End logic
    /// </summary>
    /// <param name="runOriginal">Should original game end logic be used</param>
    /// <param name="instance">Instance of LogicGameFlowNormal</param>
    public virtual void CheckGameEnd(out bool runOriginal, LogicGameFlowNormal instance)
    {
        runOriginal = true;
    }

    /// <summary>
    /// Custom role assignment
    /// </summary>
    /// <param name="runOriginal">Should original role assignment be used</param>
    /// <param name="instance">Instance of LogicRoleSelection</param>
    public virtual void AssignRoles(out bool runOriginal, LogicRoleSelectionNormal instance)
    {
        runOriginal = true;
    }

    /// <summary>
    /// Custom murder logic
    /// </summary>
    /// <param name="runOriginal">Should original murder logic be used</param>
    /// <param name="result">Result of custom murder logic</param>
    /// <param name="target">Target player for murder</param>
    public virtual void CanKill(out bool runOriginal, out bool result, PlayerControl target)
    {
        result = false;
        runOriginal = true;
    }

    /// <summary>
    /// Should Roles Settings be available when this gamemode is selected?
    /// </summary>
    public virtual bool CanAccessRolesTab() => true;

    /// <summary>
    /// Should Game Settings be available when this gamemode is selected?
    /// </summary>
    public virtual bool CanAccessSettingsTab() => true;

    /// <summary>
    /// Custom winner selection
    /// </summary>
    /// <returns>List of winners</returns>
    public virtual List<NetworkedPlayerInfo> CalculateWinners() => null;

    /// <summary>
    /// Show gamemode in Intro Cutscene
    /// </summary>
    public virtual bool ShowCustomRoleScreen() => false;

    /// <summary>
    /// Can Admin be used in this gamemode
    /// </summary>
    /// <param name="console">Admin Console</param>
    public virtual bool CanUseMapConsole(MapConsole console) => true;

    /// <summary>
    /// Can a body be reported in this gamemode
    /// </summary>
    /// <param name="body">Target body for reporting</param>
    public virtual bool CanReport(DeadBody body) => true;

    /// <summary>
    /// Can system consoles be used in this gamemode
    /// </summary>
    /// <param name="console">System Console</param>
    public virtual bool CanUseSystemConsole(SystemConsole console) => true;

    /// <summary>
    /// Can tasks be interacted with in this gamemode
    /// </summary>
    /// <param name="console">Task console</param>
    public virtual bool CanUseConsole(Console console) => true;

    /// <summary>
    /// Should the sabotage map be used when attempting to open sabotage overlay
    /// </summary>
    /// <param name="map">MapBehaviour object</param>
    public virtual bool ShouldShowSabotageMap(MapBehaviour map) => true;

    /// <summary>
    /// Can a player vent in this gamemode
    /// </summary>
    /// <param name="vent">Target vent</param>
    /// <param name="playerInfo">Player attempting to vent</param>
    public virtual bool CanVent(Vent vent, NetworkedPlayerInfo playerInfo) => true;

}