namespace CreativeToolbox.Commands.StartDecon
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using LightContainmentZoneDecontamination;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class StartDecon : ICommand
    {
        public string Command => "startdecon";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Force starts Light Containment Zone decontamination";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
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

            if (Map.IsLCZDecontaminated || CreativeToolboxEventHandler.WasDecontaminationCommandRun)
                response = "Light Contaimnent Zone decontamination is already on";
            else
            {
                CreativeToolboxEventHandler.WasDecontaminationCommandRun = true;
                DecontaminationController.Singleton._nextPhase =
                    DecontaminationController.Singleton.DecontaminationPhases.Length - 1;
                DecontaminationController.Singleton._decontaminationBegun = true;
                Map.Broadcast(5, "Light Contaimnent Zone decontamination has been turned on!");
                response = "Light Contaimnent Zone decontamination has been turned on";
            }

            return true;
        }
    }
}