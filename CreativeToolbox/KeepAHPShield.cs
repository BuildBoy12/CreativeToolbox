using System;
using UnityEngine;
using MEC;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;

namespace CreativeToolbox
{
    public class KeepAHPShield : MonoBehaviour
    {
        private Player Hub;
        private float CurrentAHP = 0;
        CoroutineHandle Handle;
        public void Awake()
        {
            Hub = Player.Get(gameObject);
            Handle = Timing.RunCoroutine(RetainAHP());
            Exiled.Events.Handlers.Player.Hurting += RunWhenPlayerIsHurt;
            Exiled.Events.Handlers.Player.ChangingRole += RunWhenPlayerChangesClass;
        }

        public void OnDestroy()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= RunWhenPlayerChangesClass;
            Exiled.Events.Handlers.Player.Hurting -= RunWhenPlayerIsHurt;
            Hub = null;
            Timing.KillCoroutines(Handle);
        }

        public void RunWhenPlayerIsHurt(HurtingEventArgs PlayerHurt)
        {
            if (PlayerHurt.Target != Hub)
                return;

            if (CurrentAHP > 0)
                CurrentAHP -= PlayerHurt.Amount;
            else
                CurrentAHP = 0;
        }

        public void RunWhenPlayerChangesClass(ChangingRoleEventArgs ChangedRole)
        {
            if (ChangedRole.Player != Hub)
                return;
    
            if (ChangedRole.NewRole.IsNotHuman())
            {
                Timing.KillCoroutines(Handle);
                if (ChangedRole.NewRole == RoleType.Scp096)
                    Hub.AdrenalineHealth = 500f;
                else
                    Hub.AdrenalineHealth = 0f;
            }
            else
                Timing.RunCoroutine(RetainAHP());
        }

        public IEnumerator<float> RetainAHP()
        {
            while (true)
            {
                if (Hub.AdrenalineHealth <= CurrentAHP)
                    Hub.AdrenalineHealth = CurrentAHP;
                else
                {
                    if (Hub.AdrenalineHealth >= CreativeToolbox.ConfigRef.Config.AhpValueLimit)
                    {
                        Hub.AdrenalineHealth = CreativeToolbox.ConfigRef.Config.AhpValueLimit;
                    }
                    CurrentAHP = Hub.AdrenalineHealth;
                }

                yield return Timing.WaitForSeconds(0.05f);
            }
        }
    }
}
