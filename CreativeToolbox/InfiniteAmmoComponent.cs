using System;
using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using MEC;
using UnityEngine;

namespace CreativeToolbox
{
    public class InfiniteAmmoComponent : MonoBehaviour
    {
        public ReferenceHub Hub;
        CoroutineHandle Handle;
        public void Awake()
        {
            Hub = gameObject.GetPlayer();
            Events.ShootEvent += RunWhenPlayerShoots;
            Events.ItemDroppedEvent += RunWhenPlayerDropsItem;
            Handle = Timing.RunCoroutine(ReplenishAmmo());
        }

        public void OnDestroy()
        {
            ModifyAmmo(Hub, 0);
            Hub = null;
            Events.ItemDroppedEvent -= RunWhenPlayerDropsItem;
            Events.ShootEvent -= RunWhenPlayerShoots;
            Timing.KillCoroutines(Handle);
        }

        public void RunWhenPlayerShoots(ref ShootEvent s)
        {
            if (s.Shooter != Hub)
                return;

            ModifyAmmo(s.Shooter, 999);
        }

        public void RunWhenPlayerDropsItem(ItemDroppedEvent d)
        {
            if (d.Player != Hub)
                return;

            if (d.Item.IsGun())
                UnityEngine.Object.Destroy(d.Item.gameObject);
        }

        public void ModifyAmmo(ReferenceHub hub, int value)
        {
            hub.SetWeaponAmmo(value);
        }

        public IEnumerator<float> ReplenishAmmo()
        {
            while (true)
            {
                ModifyAmmo(Hub, 999);
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}
