namespace CreativeToolbox.Commands.CustomNade
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;
    using static CreativeToolbox;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CustomNade : ParentCommand
    {
        public CustomNade() => LoadGeneratedCommands();

        public override string Command => "customnade";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description => "Sets the time frag and flash grenades get set off";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new Flash());
            RegisterCommand(new Frag());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.customnade"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.customnade\"";
                return false;
            }

            if (!Instance.Config.EnableCustomGrenadeTime)
            {
                response = "You cannot modify grenade time as the setting is disabled";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: frag, flash";
            return false;
        }
    }
}