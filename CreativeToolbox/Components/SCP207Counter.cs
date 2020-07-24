using System;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using UnityEngine;
namespace CreativeToolbox
{
    public class SCP207Counter : MonoBehaviour
    {
        private Player Hub;
        public int Counter = 0;
        public void Awake()
        {
            Hub = Player.Get(gameObject);
            Counter = 1;
            Hub.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nnumber of drinks consumed: {Counter}", new HintParameter[]
            {
                new StringHintParameter("")
            }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            Exiled.Events.Handlers.Player.ChangingRole += RunWhenPlayerSwitchesClasses;
            Exiled.Events.Handlers.Player.MedicalItemUsed += RunWhenplayerDrinksSCP207;
        }

        public void OnDestroy()
        {
            Hub = null;
            Counter = 0;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= RunWhenplayerDrinksSCP207;
            Exiled.Events.Handlers.Player.ChangingRole -= RunWhenPlayerSwitchesClasses;
        }

        public void RunWhenplayerDrinksSCP207(UsedMedicalItemEventArgs Used207)
        {
            if (Used207.Player != Hub || Used207.Item != ItemType.SCP207)
                return;

            if (Hub.IsGodModeEnabled)
            {
                Counter = 0;
                return;
            }
            
            if (Counter < CreativeToolbox.ConfigRef.Config.Scp207DrinkLimit)
            {
                Used207.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nnumber of drinks consumed: {Counter}", new HintParameter[]
                {
                    new StringHintParameter("")
                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            }

            if (Counter == CreativeToolbox.ConfigRef.Config.Scp207PryGateLimit)
            {
                if (!CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(Hub))
                {
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Add(Hub);
                    Used207.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nyou can now pry gates open", new HintParameter[]
                    {
                    new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                }
            }

            if (Counter >= CreativeToolbox.ConfigRef.Config.Scp207DrinkLimit)
            {
                if (CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(Hub))
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Remove(Hub);
                CreativeToolboxEventHandler.SpawnGrenadeOnPlayer(Hub, false);
                Counter = 0;
                Used207.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nyou drank too much and your body could not handle it", new HintParameter[]
                {
                    new StringHintParameter("")
                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            }
        }

        public void RunWhenPlayerSwitchesClasses(ChangingRoleEventArgs RoleChanged)
        {
            if (RoleChanged.Player != Hub)
                return;

            Counter = 0;
        }
    }
}
