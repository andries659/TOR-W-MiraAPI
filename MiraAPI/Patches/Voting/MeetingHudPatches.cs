using System.Linq;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Meeting.Voting;
using MiraAPI.Utilities;
using MiraAPI.Voting;

namespace MiraAPI.Patches.Voting;

[HarmonyPatch(typeof(MeetingHud))]
internal static class MeetingHudPatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(MeetingHud.Start))]
    public static void MeetingHudStartPatch(MeetingHud __instance)
    {
        foreach (var plr in PlayerControl.AllPlayerControls)
        {
            var voteData = plr.GetVoteData();
            voteData.Votes.Clear();
            voteData.SetRemainingVotes(1);

            if (plr.Data.IsDead || plr.Data.Disconnected)
            {
                voteData.SetRemainingVotes(0);
            }
        }
        var @event = new StartMeetingEvent(__instance);
        MiraEventManager.InvokeEvent(@event);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MeetingHud.Select))]
    public static bool SelectPatch(MeetingHud __instance, int suspectStateIdx)
    {
        var voteData = PlayerControl.LocalPlayer.GetVoteData();

        var hasVotes = voteData.VotesRemaining > 0;
        var hasVotedFor = voteData.VotedFor((byte)suspectStateIdx);

        var @event = new MeetingSelectEvent(voteData, suspectStateIdx, hasVotes && !hasVotedFor);
        MiraEventManager.InvokeEvent(@event);

        return @event.AllowSelect;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MeetingHud.HandleDisconnect), typeof(PlayerControl), typeof(DisconnectReasons))]
    public static bool HandleDisconnect(MeetingHud __instance, PlayerControl pc)
    {
        if (!AmongUsClient.Instance.AmHost || __instance.playerStates is null || !pc || !GameData.Instance)
        {
            return false;
        }

        var playerVoteArea = __instance.playerStates.First(pv => pv.TargetPlayerId == pc.PlayerId);
        playerVoteArea.AmDead = true;
        playerVoteArea.Overlay.gameObject.SetActive(true);

        foreach (var player in Helpers.GetAlivePlayers())
        {
            var pva = __instance.playerStates.First(pv => pv.TargetPlayerId == player.PlayerId);
            var voteData = player.GetVoteData();

            if (pva.AmDead || !voteData.VotedFor(pc.PlayerId))
            {
                continue;
            }

            voteData.Votes.RemoveAll(x=>x.Suspect==pc.PlayerId);
            voteData.VotesRemaining += 1;

            VotingUtils.RpcRemoveVote(PlayerControl.LocalPlayer, player.PlayerId, pc.PlayerId);
        }

        __instance.SetDirtyBit(1U);
        __instance.CheckForEndVoting();

        if (__instance.state == MeetingHud.VoteStates.Results)
        {
            __instance.SetupProceedButton();
        }

        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MeetingHud.CastVote))]
    public static bool CastVotePatch(MeetingHud __instance, byte srcPlayerId, byte suspectPlayerId)
    {
        var plr = GameData.Instance.GetPlayerById(srcPlayerId);

        if (plr == null)
        {
            return false;
        }

        var voteData = plr.Object.GetVoteData();
        if (voteData == null || voteData.VotesRemaining == 0 ||
            voteData.VotedFor(suspectPlayerId))
        {
            return false;
        }

        VotingUtils.HandleVote(voteData, suspectPlayerId);

        __instance.SetDirtyBit(1U);
        __instance.CheckForEndVoting();

        if (voteData.VotesRemaining != 0)
        {
            return false;
        }

        __instance.playerStates.First(x => x.TargetPlayerId == srcPlayerId).SetVote(suspectPlayerId);
        PlayerControl.LocalPlayer.RpcSendChatNote(srcPlayerId, ChatNoteTypes.DidVote);

        return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MeetingHud.CmdCastVote))]
    public static void CmdCastVotePatch(MeetingHud __instance, byte playerId, byte suspectIdx)
    {
        var plr = GameData.Instance.GetPlayerById(playerId);
        var voteData = plr.Object.GetVoteData();

        if (!AmongUsClient.Instance.AmHost)
        {
            if (voteData == null || voteData.VotesRemaining == 0 ||
                voteData.VotedFor(suspectIdx))
            {
                return;
            }

            VotingUtils.HandleVote(voteData, suspectIdx);

            if (voteData.VotesRemaining == 0)
            {
                __instance.playerStates.First(x => x.TargetPlayerId == playerId).SetVote(suspectIdx);
            }
        }

        if (PlayerControl.LocalPlayer.PlayerId != playerId)
        {
            return;
        }

        SoundManager.Instance.PlaySound(__instance.VoteLockinSound, false);

        foreach (var playerVoteArea in __instance.playerStates)
        {
            playerVoteArea.ClearButtons();
        }

        __instance.SkipVoteButton.ClearButtons();

        var localVoteData = PlayerControl.LocalPlayer.GetVoteData();
        if (localVoteData == null || localVoteData.VotesRemaining != 0) return;

        __instance.SkipVoteButton.voteComplete = true;
        __instance.SkipVoteButton.gameObject.SetActive(false);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MeetingHud.CheckForEndVoting))]
    public static bool EndCheck(MeetingHud __instance)
    {
        var shouldEnd = !Helpers.GetAlivePlayers().Exists(plr => plr.GetVoteData().VotesRemaining > 0);

        var checkEndEvent = new CheckForEndVotingEvent(shouldEnd);
        MiraEventManager.InvokeEvent(checkEndEvent);
        shouldEnd = checkEndEvent.ForceEndVoting || checkEndEvent is { IsCancelled: false, IsVotingComplete: true };

        if (!shouldEnd)
        {
            return false;
        }

        var votes = VotingUtils.CalculateVotes();
        var exiled = VotingUtils.GetExiled(votes, out var isTie);

        var @event = new ProcessVotesEvent(votes, exiled);
        MiraEventManager.InvokeEvent(@event);

        if (@event.VotesModified)
        {
            votes = @event.Votes;
            exiled = VotingUtils.GetExiled(votes, out isTie);
        }

        if (@event.ExiledPlayerModified)
        {
            exiled = @event.ExiledPlayer;
        }

        var voterStates = new Il2CppStructArray<MeetingHud.VoterState>([
            .. votes.Select(
            v=> new MeetingHud.VoterState
            {
                VoterId = v.Voter,
                VotedForId = v.Suspect,
            })
        ]);

        __instance.RpcVotingComplete(voterStates, exiled, isTie);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MeetingHud.PopulateResults))]
    public static bool PopulateResultsPatch(MeetingHud __instance, ref Il2CppStructArray<MeetingHud.VoterState> states)
    {
        var votes = states.Select(x=> new CustomVote(x.VoterId, x.VotedForId)).ToList();
        var @event = new PopulateResultsEvent(votes);
        MiraEventManager.InvokeEvent(@event);

        if (@event.IsCancelled)
        {
            return false;
        }

        VotingUtils.HandlePopulateResults(votes);
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MeetingHud.Confirm))]
    public static bool ConfirmPatch(MeetingHud __instance, [HarmonyArgument(0)] byte suspect)
    {
        __instance.CmdCastVote(PlayerControl.LocalPlayer.PlayerId, suspect);

        return false;
    }
}
