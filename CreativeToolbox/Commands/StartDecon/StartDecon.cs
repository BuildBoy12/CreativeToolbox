using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using LightContainmentZoneDecontamination;

namespace CreativeToolbox.Commands.StartDecon
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class StartDecon : ParentCommand
    {
        public StartDecon() => LoadGeneratedCommands();

        public override string Command { get; } = "startdecon";

        public override string[] Aliases { get; } = new string[] { };

        public override string Description { get; } = "Force starts Light Containment Zone decontamination";

        public override void LoadGeneratedCommands() { }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.startdecon"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.startdecon\"";
                return false;
            }

            if (arguments.Count != 0)
            {
                response = "Usage: startdecon";
                return false;
            }

            if (Map.IsLCZDecontaminated || CreativeToolboxEventHandler.WasDeconCommandRun)
                response = "Light Contaimnent Zone decontamination is already on";
            else
            {
                CreativeToolboxEventHandler.WasDeconCommandRun = true;
                DecontaminationController.Singleton._nextPhase = DecontaminationController.Singleton.DecontaminationPhases.Length - 1;
                DecontaminationController.Singleton._decontaminationBegun = true;
                Map.Broadcast(5, "Light Contaimnent Zone decontamination has been turned on!");
                response = "Light Contaimnent Zone decontamination has been turned on";
            }
            return true;
        }
    }
}
