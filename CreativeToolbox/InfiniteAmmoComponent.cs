using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using UnityEngine;

namespace CreativeToolbox
{
    public class InfiniteAmmoComponent : MonoBehaviour
    {
        private Player Hub;
        CoroutineHandle Handle;
        public void Awake()
        {
            Hub = Player.Get(gameObject);
            Exiled.Events.Handlers.Player.Shooting += RunWhenPlayerShoots;
            Exiled.Events.Handlers.Player.ItemDropped += RunWhenPlayerDropsItem;
            Handle = Timing.RunCoroutine(ReplenishAmmo());
        }

        public void OnDestroy()
        {
            ModifyAmmo(Hub.ReferenceHub, 0);
            Hub = null;
            Exiled.Events.Handlers.Player.ItemDropped -= RunWhenPlayerDropsItem;
            Exiled.Events.Handlers.Player.Shooting -= RunWhenPlayerShoots;
            Timing.KillCoroutines(Handle);
        }

        public void RunWhenPlayerShoots(ShootingEventArgs s)
        {
            if (s.Shooter.ReferenceHub != Hub.ReferenceHub)
                return;

            ModifyAmmo(s.Shooter.ReferenceHub, 999);
        }

        public void RunWhenPlayerDropsItem(ItemDroppedEventArgs d)
        {
            if (d.Player != Hub)
                return;

            if (d.Pickup.ItemId.IsGun())
                UnityEngine.Object.Destroy(d.Pickup.gameObject);
        }

        public void ModifyAmmo(ReferenceHub hub, int value)
        {
            hub.SetWeaponAmmo(value);
        }

        public IEnumerator<float> ReplenishAmmo()
        {
            while (true)
            {
                ModifyAmmo(Hub.ReferenceHub, 999);
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}
