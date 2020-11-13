namespace CreativeToolbox.Commands.FallDamage
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class FallDamage : ParentCommand
    {
        public FallDamage() => LoadGeneratedCommands();

        public override string Command => "falldamage";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description => "Turns on/off fall damage";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Off());
            RegisterCommand(new On());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.falldamage"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.falldamage\"";
                return false;
            }

            if (!Instance.Config.EnableFallDamagePrevention)
            {
                response = "You cannot modify fall damage as the setting is disabled";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: frag, flash";
            return false;
        }
    }
}