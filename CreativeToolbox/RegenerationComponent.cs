using System;
using UnityEngine;
using MEC;
using System.Collections.Generic;
using Exiled.API.Features;

namespace CreativeToolbox
{
    public class RegenerationComponent : MonoBehaviour
    {
        private Player Hub;
        CoroutineHandle Handle;
        public void Awake()
        {
            Hub = Player.Get(gameObject);
            Handle = Timing.RunCoroutine(HealHealth(Hub));
        }

        public void OnDestroy()
        {
            Hub = null;
            Timing.KillCoroutines(Handle);
        }

        public IEnumerator<float> HealHealth(Player ply)
        {
            while (true)
            {
                if (ply.Health < ply.MaxHealth)
                    ply.Health += CreativeConfig.HPRegenerationValue;
                else
                    ply.Health = ply.MaxHealth;

                yield return Timing.WaitForSeconds(CreativeConfig.HPRegenerationTimer);
            }
        }
    }
}
