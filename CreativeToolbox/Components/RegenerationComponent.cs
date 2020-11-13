namespace CreativeToolbox
{
    using Exiled.API.Features;
    using MEC;
    using System.Collections.Generic;
    using UnityEngine;
    using static CreativeToolbox;

    public class RegenerationComponent : MonoBehaviour
    {
        private Player _ply;
        private CoroutineHandle _handle;

        public void Awake()
        {
            _ply = Player.Get(gameObject);
            _handle = Timing.RunCoroutine(HealHealth(_ply));
        }

        public void OnDestroy()
        {
            _ply = null;
            Timing.KillCoroutines(_handle);
        }

        private IEnumerator<float> HealHealth(Player ply)
        {
            while (true)
            {
                if (ply.Health < ply.MaxHealth)
                    ply.Health += Instance.Config.RegenerationValue;
                else
                    ply.Health = ply.MaxHealth;

                yield return Timing.WaitForSeconds(Instance.Config.RegenerationTime);
            }
        }
    }
}