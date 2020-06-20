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
        private bool IsNotAlive = false;
        CoroutineHandle Handle;
        public void Awake()
        {
            Hub = Player.Get(gameObject);
            Handle = Timing.RunCoroutine(RetainAHP());
            Exiled.Events.Handlers.Player.Hurting += RunWhenPlayerIsHurt;
        }

        public void OnDestroy()
        {
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
            if (ChangedRole.Player != Hub || (ChangedRole.NewRole == RoleType.Spectator || ChangedRole.NewRole == RoleType.None))
                IsNotAlive = true;
            else
                IsNotAlive = false;
        }

        public IEnumerator<float> RetainAHP()
        {
            while (true)
            {
                if (!IsNotAlive)
                    yield return Timing.WaitForSeconds(0.05f);

                if (Hub.AdrenalineHealth <= CurrentAHP)
                    Hub.AdrenalineHealth = CurrentAHP;
                else
                {
                    if (Hub.AdrenalineHealth >= CreativeConfig.AHPValueLimit)
                    {
                        Hub.AdrenalineHealth = CreativeConfig.AHPValueLimit;
                    }
                    CurrentAHP = Hub.AdrenalineHealth;
                }

                yield return Timing.WaitForSeconds(0.05f);
            }
        }
    }
}
