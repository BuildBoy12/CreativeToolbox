using System;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using UnityEngine;
namespace CreativeToolbox
{
    public class SCP207AbilityCounter : MonoBehaviour
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
                string MessageToReplace = RogerFKTokenReplace.ReplaceAfterToken(CreativeToolbox.ConfigRef.Config.DrinkingScp207Message, '%', new Tuple<string, object>[] { new Tuple<string, object>("counter", Counter) });
                Used207.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{MessageToReplace}", new HintParameter[]
                {
                    new StringHintParameter("")
                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            }

            if (Counter == CreativeToolbox.ConfigRef.Config.Scp207PryGateLimit)
            {
                string MessageToReplace = RogerFKTokenReplace.ReplaceAfterToken(CreativeToolbox.ConfigRef.Config.PryGatesWithScp207Message, '%', new Tuple<string, object>[] { new Tuple<string, object>("counter", Counter) });
                if (!CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(Hub))
                {
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Add(Hub);
                    Used207.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{MessageToReplace}", new HintParameter[]
                    {
                    new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                }
            }

            if (Counter >= CreativeToolbox.ConfigRef.Config.Scp207DrinkLimit)
            {
                string MessageToReplace = RogerFKTokenReplace.ReplaceAfterToken(CreativeToolbox.ConfigRef.Config.ExplodeAfterScp207Message, '%', new Tuple<string, object>[] { new Tuple<string, object>("counter", Counter) });
                if (CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(Hub))
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Remove(Hub);
                Hub.Health = 0;
                Hub.AdrenalineHealth = 0;
                Hub.Kill();
                CreativeToolboxEventHandler.SpawnGrenadeOnPlayer(Hub, false);
                Counter = 0;
                Used207.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{MessageToReplace}", new HintParameter[]
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
