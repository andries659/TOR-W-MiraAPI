using System.Linq;
using MiraAPI.Events;
using MiraAPI.Events.Mira;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Map;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Meeting.Voting;
using MiraAPI.Events.Vanilla.Player;
using TORW.Roles;
using MiraAPI.Utilities;
using Reactor.Utilities;

namespace TORW;

public static class TORWEventHandlers
{
    public static void Initialize()
    {
        // You can register event handlers with the MiraEventManager class.
        MiraEventManager.RegisterEventHandler<BeforeMurderEvent>(@event =>
        {
            Logger<TORW>.Info($"{@event.Source.Data.PlayerName} is about to kill {@event.Target.Data.PlayerName}");
        });

        MiraEventManager.RegisterEventHandler<AfterMurderEvent>(@event =>
        {
            Logger<TORW>.Info($"{@event.Source.Data.PlayerName} has killed {@event.Target.Data.PlayerName}");
        });

        MiraEventManager.RegisterEventHandler<CompleteTaskEvent>(@event =>
        {
            Logger<TORW>.Info($"{@event.Player.Data.PlayerName} completed {@event.Task.TaskType.ToString()}");
        });
    }

    // If you want to add extra votes to a player, do something like this.
    [RegisterEvent]
    public static void StartMeetingEvent(StartMeetingEvent _)
    {
        foreach (var plr in PlayerControl.AllPlayerControls.ToArray().Where(player => player.Data.Role is MayorRole))
        {
            plr.GetVoteData().IncreaseRemainingVotes(1);
        }
    }

    // Events can be registered using an attribute as well.
    [RegisterEvent]
    public static void UpdateSystemEventHandler(UpdateSystemEvent @event)
    {
        Logger<TORW>.Error(@event.SystemType.ToString());
    }
}
