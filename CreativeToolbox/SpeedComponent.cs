using System;
using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using UnityEngine;
using CustomPlayerEffects;

namespace CreativeToolbox
{
    public class SpeedComponent : MonoBehaviour
    {
        public Scp207 Scp207;
        public ReferenceHub Hub;
        public void Awake()
        {
            Hub = gameObject.GetPlayer();
            Scp207 = Hub.effectsController.GetEffect<Scp207>("SCP-207");
            Scp207.ServerEnable();
            Events.PlayerHurtEvent += RunWhenPlayerIsHurt;
        }

        public void OnDestroy()
        {
            Scp207.ServerDisable();
            Scp207.Enabled = false;
            Scp207 = null;
            Events.PlayerHurtEvent -= RunWhenPlayerIsHurt;
        }

        public void RunWhenPlayerIsHurt(ref PlayerHurtEvent plyHurt)
        {
            if (plyHurt.Player == Hub && plyHurt.DamageType == DamageTypes.Scp207)
                plyHurt.Amount = 0;
        }
    }
}