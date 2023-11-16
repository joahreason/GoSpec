using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Utils;

namespace GoSpec;

public class GoSpec : BasePlugin
{
    public override string ModuleName => "GoSpec";
    public override string ModuleAuthor => "cosmic sans";
    public override string ModuleVersion => "1.0";

    public override void Load(bool hotReload)
    {
        Console.WriteLine($"{ChatColors.Blue}{ModuleName} v{ModuleVersion} by {ModuleAuthor} is active. Type !spec to switch to spec.");
    }

    [ConsoleCommand("spec", "Moves you to spec.")]
    [CommandHelper(whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnSpecCommand(CCSPlayerController? caller, CommandInfo command)
    {
        if (caller == null) return;
        if (caller.PlayerPawn.Value.TeamNum == (int)CsTeam.Spectator) return;

        caller.ChangeTeam(CsTeam.Spectator);

        if (caller.PlayerPawn.Value.TeamNum != (int)CsTeam.Spectator)
        {
            caller.PrintToChat($"{ChatColors.Red}Failed to move you to spec.");
            return;
        }

        Server.PrintToChatAll($"{ChatColors.Default}Moving {ChatColors.Green}{caller.PlayerName} {ChatColors.Default}to spec.");
    }
}