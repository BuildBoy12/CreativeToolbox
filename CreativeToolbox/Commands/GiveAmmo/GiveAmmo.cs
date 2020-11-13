namespace CreativeToolbox.Commands.GiveAmmo
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class GiveAmmo : ParentCommand
    {
        public GiveAmmo() => LoadGeneratedCommands();

        public override string Command => "giveammo";

        public override string[] Aliases => Array.Empty<string>();

        public override string Description =>
            "Gives a specified user or users a specified ammount of a given ammo type";

        public sealed override void LoadGeneratedCommands()
        {
            RegisterCommand(new All());
            RegisterCommand(new To());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
            out string response)
        {
            if (!(sender as CommandSender).CheckPermission("ct.giveammo"))
            {
                response = "You do not have permission to run this command! Missing permission: \"ct.giveammo\"";
                return false;
            }

            response = "Please enter a valid subcommand! Available ones: all, to";
            return false;
        }
    }
}