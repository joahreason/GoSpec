using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Utils;

namespace GoSpec;
public class GoSpec : BasePlugin
{
    public override string ModuleName => "GoSpec";
    public override string ModuleAuthor => "cosmic sans";
    public override string ModuleVersion => "1.1";

    public override void Load(bool hotReload)
    {
        Console.WriteLine($"{ModuleName} version {ModuleVersion} by {ModuleAuthor} is active.");
    }

    [ConsoleCommand("css_spec", "Moves you to spec.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnSpecCommand(CCSPlayerController? caller, CommandInfo command)
    {
        ChangeTeam(caller, CsTeam.Spectator);
    }

    #region Alternate Spec Commands
    [ConsoleCommand("s", "Moves you to spec.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnSCommand(CCSPlayerController? caller, CommandInfo command) => OnSpecCommand(caller, command);

    [ConsoleCommand("afk", "Moves you to spec.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnAFKCommand(CCSPlayerController? caller, CommandInfo command) => OnSpecCommand(caller, command);

    [ConsoleCommand("brb", "Moves you to spec.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnBRBCommand(CCSPlayerController? caller, CommandInfo command) => OnSpecCommand(caller, command);
    #endregion

    [ConsoleCommand("css_ct", "Joins the counterterrorist team.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnJoinCTCommand(CCSPlayerController? caller, CommandInfo command)
    {
        ChangeTeam(caller, CsTeam.CounterTerrorist);
    }

    [ConsoleCommand("css_t", "Joins the terrorist team.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnJoinTCommand(CCSPlayerController? caller, CommandInfo command)
    {
        ChangeTeam(caller, CsTeam.Terrorist);
    }

    private bool ChangeTeam(CCSPlayerController? caller, CsTeam moveTo)
    {
        if (caller == null) return false;
        
        if(IsInSpec(caller) && moveTo == CsTeam.Spectator)
        {
            caller.PrintToChat($"{ChatColors.Red}You already are in spec.");
            return false;
        }
        
        if (!IsUnassigned(caller) && moveTo != CsTeam.Spectator)
        {
            caller.PrintToChat($"{ChatColors.Red}You must be in spec to use this command.");
            return false;
        }

        NotifyTeamChange(caller, moveTo);
        caller.ChangeTeam(moveTo);

        return true;
    }

    private void NotifyTeamChange(CCSPlayerController caller, CsTeam team)
    {
        string name = caller.PlayerName ?? "Unknown";
        char teamColor = ChatColors.White;
        if (team != CsTeam.Spectator) teamColor = team == CsTeam.Terrorist ? ChatColors.Red : ChatColors.Blue;
        string teamMsg = $"{ChatColors.Default}Moving {ChatColors.Green}{name} {ChatColors.Default}to {teamColor}{team}.";

        Server.PrintToChatAll(teamMsg);
    }

    private int GetTeamCount(CsTeam team)
    {
        int count = 0;
        foreach (CCSPlayerController user in Utilities.GetPlayers())
        {
            if (user.PlayerPawn.Value.TeamNum == (int)team) count++;
        }

        return count;
    }

    private bool IsUnassigned(CCSPlayerController? caller)
    {
        if (caller == null) return false;
        return caller.TeamNum != (int)CsTeam.Terrorist && caller.TeamNum != (int)CsTeam.CounterTerrorist;
    }

    private bool IsInSpec(CCSPlayerController? caller)
    {
        if (caller == null) return false;
        return caller.TeamNum == (int)CsTeam.Spectator;
    }
}