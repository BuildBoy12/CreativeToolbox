using System;
using System.Collections.Generic;
using CustomPlayerEffects;
using EXILED;
using EXILED.Extensions;
using MEC;
using Mirror;
using UnityEngine;

namespace CreativeToolbox
{
    public class InvisibleComponent : MonoBehaviour
    {
        public ReferenceHub Hub;
        public Scp268 SCP268;
        public void Awake()
        {
            Hub = gameObject.GetPlayer();
            Hub.effectsController.NetworksyncEffects = "0100";
            SetPlayerScale(Hub.gameObject, 0);
        }

        public void OnDestroy()
        {
            SetPlayerScale(Hub.gameObject, 1);
            Hub.effectsController.NetworksyncEffects = "0000";
            Hub = null;
        }

        public void SetPlayerScale(GameObject target, float scale)
        {
            try
            {
                NetworkIdentity identity = target.GetComponent<NetworkIdentity>();
                target.transform.localScale = Vector3.one * scale;
                ObjectDestroyMessage destroyMessage = new ObjectDestroyMessage();
                destroyMessage.netId = identity.netId;


                foreach (GameObject player in PlayerManager.players)
                {
                    if (player == target)
                        continue;

                    NetworkConnection playerCon = player.GetComponent<NetworkIdentity>().connectionToClient;
                    playerCon.Send(destroyMessage, 0);
                    object[] parameters = new object[] { identity, playerCon };
                    typeof(NetworkServer).InvokeStaticMethod("SendSpawnMessage", parameters);
                }
            }
            catch (Exception e)
            {
                Log.Info($"Set Scale error: {e}");
            }
        }
    }
}
