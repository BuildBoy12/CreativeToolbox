using System;
using UnityEngine;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CreativeToolbox
{
    public class SCP106AdvancedGodComponent : MonoBehaviour
    {
        private Player Hub;
        public void Awake()
        {
            Hub = Player.Get(gameObject);
            Hub.IsGodModeEnabled = true;
            Exiled.Events.Handlers.Player.Hurting += RunWhenPlayerGetsHurt;
            Exiled.Events.Handlers.Player.ChangingRole += RunWhenPlayerChangesClass;
        }

        public void OnDestroy()
        {
            Hub = null;
            Hub.IsGodModeEnabled = false;
            Exiled.Events.Handlers.Player.ChangingRole -= RunWhenPlayerChangesClass;
            Exiled.Events.Handlers.Player.Hurting -= RunWhenPlayerGetsHurt;
        }

        public void RunWhenPlayerGetsHurt(HurtingEventArgs PlayerHurt)
        {
            if (PlayerHurt.Target == Hub && (!Hub.IsGodModeEnabled || Hub.Health < Hub.MaxHealth))
            {
                Hub.IsGodModeEnabled = true;
                Hub.Health = Hub.MaxHealth;
            }
        }

        public void RunWhenPlayerChangesClass(ChangingRoleEventArgs ChangedRole)
        {
            if (ChangedRole.Player.Role != RoleType.Scp106)
            {
                CreativeToolboxEventHandler.PlayersWithAdvancedGodmode.Remove(ChangedRole.Player.ReferenceHub);
            }
            else
            {
                CreativeToolboxEventHandler.PlayersWithAdvancedGodmode.Add(ChangedRole.Player.ReferenceHub);
            }
        }
    }
}
