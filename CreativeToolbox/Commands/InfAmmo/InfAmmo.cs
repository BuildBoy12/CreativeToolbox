namespace CreativeToolbox.Commands.InfAmmo
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class InfAmmo : ParentCommand
    {
        public InfAmmo() => LoadGeneratedCommands();

        public override string Command => "infammo";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description =>
            "Gives infinite ammo to players, clears infinite ammo from players, and shows who has infinite ammo";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new All());
            RegisterCommand(new Clear());
            RegisterCommand(new Give());
            RegisterCommand(new List());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.infammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.infammo\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: all, clear, give, list";
            return false;
        }
    }
}