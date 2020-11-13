using Exiled.API.Extensions;

namespace CreativeToolbox
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using MEC;
    using System.Collections.Generic;
    using UnityEngine;
    using static CreativeToolbox;

    public class KeepAhpShield : MonoBehaviour
    {
        private Player _ply;
        private float _currentAhp;
        private CoroutineHandle _handle;

        public void Awake()
        {
            _ply = Player.Get(gameObject);
            _handle = Timing.RunCoroutine(RetainAhp());
            Exiled.Events.Handlers.Player.Hurting += RunWhenPlayerIsHurt;
            Exiled.Events.Handlers.Player.ChangingRole += RunWhenPlayerChangesClass;
        }

        public void OnDestroy()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= RunWhenPlayerChangesClass;
            Exiled.Events.Handlers.Player.Hurting -= RunWhenPlayerIsHurt;
            _ply = null;
            Timing.KillCoroutines(_handle);
        }

        public void RunWhenPlayerIsHurt(HurtingEventArgs ev)
        {
            if (ev.Target != _ply)
                return;

            if (_currentAhp > 0)
                _currentAhp -= ev.Amount;
            else
                _currentAhp = 0;
        }

        public void RunWhenPlayerChangesClass(ChangingRoleEventArgs ev)
        {
            if (ev.Player != _ply)
                return;

            if (ev.NewRole.GetTeam() == Team.SCP || ev.NewRole == RoleType.Spectator)
            {
                Timing.KillCoroutines(_handle);
                _ply.AdrenalineHealth = ev.NewRole == RoleType.Scp096 ? 500f : 0f;
            }
            else
                Timing.RunCoroutine(RetainAhp());
        }

        private IEnumerator<float> RetainAhp()
        {
            while (true)
            {
                if (_ply.AdrenalineHealth <= _currentAhp)
                    _ply.AdrenalineHealth = _currentAhp;
                else
                {
                    if (_ply.AdrenalineHealth >= Instance.Config.AhpValueLimit)
                    {
                        _ply.AdrenalineHealth = Instance.Config.AhpValueLimit;
                    }

                    _currentAhp = _ply.AdrenalineHealth;
                }

                yield return Timing.WaitForSeconds(0.05f);
            }
        }
    }
}