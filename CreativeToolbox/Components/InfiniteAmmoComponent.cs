namespace CreativeToolbox
{
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using System.Collections.Generic;
    using UnityEngine;

    public class InfiniteAmmoComponent : MonoBehaviour
    {
        private Player _ply;
        private CoroutineHandle _handle;

        public void Awake()
        {
            _ply = Player.Get(gameObject);
            Exiled.Events.Handlers.Player.Shooting += RunWhenPlayerShoots;
            Exiled.Events.Handlers.Player.ItemDropped += RunWhenPlayerDropsItem;
            _handle = Timing.RunCoroutine(ReplenishAmmo());
        }

        public void OnDestroy()
        {
            ModifyAmmo(_ply, 0);
            _ply = null;
            Exiled.Events.Handlers.Player.ItemDropped -= RunWhenPlayerDropsItem;
            Exiled.Events.Handlers.Player.Shooting -= RunWhenPlayerShoots;
            Timing.KillCoroutines(_handle);
        }

        public void RunWhenPlayerShoots(ShootingEventArgs ev)
        {
            if (ev.Shooter != _ply)
                return;

            ModifyAmmo(ev.Shooter, 999);
        }

        public void RunWhenPlayerDropsItem(ItemDroppedEventArgs ev)
        {
            if (ev.Player != _ply)
                return;

            if (ev.Pickup.ItemId.IsWeapon())
                Destroy(ev.Pickup.gameObject);
        }

        public void ModifyAmmo(Player ply, int value)
        {
            ply.SetWeaponAmmo(value);
        }

        private IEnumerator<float> ReplenishAmmo()
        {
            while (true)
            {
                ModifyAmmo(_ply, 999);
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}