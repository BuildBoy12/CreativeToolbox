using System;
using EXILED.Extensions;
using UnityEngine;
using MEC;
using System.Collections.Generic;

namespace CreativeToolbox
{
    public class RegenerationComponent : MonoBehaviour
    {
        private ReferenceHub Hub;
        CoroutineHandle Handle;
        public void Awake()
        {
            Hub = gameObject.GetPlayer();
            Handle = Timing.RunCoroutine(HealHealth(Hub));
        }

        public void OnDestroy()
        {
            Hub = null;
            Timing.KillCoroutines(Handle);
        }

        public IEnumerator<float> HealHealth(ReferenceHub player)
        {
            while (true)
            {
                if (player.GetHealth() < player.playerStats.maxHP)
                    player.AddHealth(CreativeToolbox.HPRegenerationValue);
                else
                    player.SetHealth(player.playerStats.maxHP);

                yield return Timing.WaitForSeconds(CreativeToolbox.HPRegenerationTimer);
            }
        }
    }
}
