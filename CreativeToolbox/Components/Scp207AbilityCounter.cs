namespace CreativeToolbox
{
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;
    using Hints;
    using System;
    using UnityEngine;
    using static CreativeToolbox;

    public class Scp207AbilityCounter : MonoBehaviour
    {
        private Player _ply;
        public int counter;

        public void Awake()
        {
            _ply = Player.Get(gameObject);
            counter = 1;
            _ply.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\nnumber of drinks consumed: {counter}",
                new HintParameter[]
                {
                    new StringHintParameter("")
                }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            Exiled.Events.Handlers.Player.ChangingRole += RunWhenPlayerSwitchesClasses;
            Exiled.Events.Handlers.Player.MedicalItemUsed += RunWhenPlayerDrinksScp207;
        }

        public void OnDestroy()
        {
            _ply = null;
            counter = 0;
            Exiled.Events.Handlers.Player.MedicalItemUsed -= RunWhenPlayerDrinksScp207;
            Exiled.Events.Handlers.Player.ChangingRole -= RunWhenPlayerSwitchesClasses;
        }

        public void RunWhenPlayerDrinksScp207(UsedMedicalItemEventArgs ev)
        {
            if (ev.Player != _ply || ev.Item != ItemType.SCP207)
                return;

            if (_ply.IsGodModeEnabled)
            {
                counter = 0;
                return;
            }

            if (counter < Instance.Config.Scp207DrinkLimit)
            {
                string messageToReplace =
                    Instance.Config.DrinkingScp207Message.ReplaceAfterToken('%',
                        new[] {new Tuple<string, object>("counter", counter)});
                ev.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{messageToReplace}",
                    new HintParameter[]
                    {
                        new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            }

            if (counter == Instance.Config.Scp207PryGateLimit)
            {
                string messageToReplace =
                    Instance.Config.PryGatesWithScp207Message.ReplaceAfterToken('%',
                        new[] {new Tuple<string, object>("counter", counter)});
                if (!CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(_ply))
                {
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Add(_ply);
                    ev.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{messageToReplace}",
                        new HintParameter[]
                        {
                            new StringHintParameter("")
                        }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
                }
            }

            if (counter >= Instance.Config.Scp207DrinkLimit)
            {
                string messageToReplace =
                    Instance.Config.ExplodeAfterScp207Message.ReplaceAfterToken('%',
                        new[] {new Tuple<string, object>("counter", counter)});
                if (CreativeToolboxEventHandler.PlayersThatCanPryGates.Contains(_ply))
                    CreativeToolboxEventHandler.PlayersThatCanPryGates.Remove(_ply);
                _ply.Health = 0;
                _ply.AdrenalineHealth = 0;
                _ply.Kill();
                CreativeToolboxEventHandler.SpawnGrenadeOnPlayer(_ply, false);
                counter = 0;
                ev.Player.ReferenceHub.hints.Show(new TextHint($"\n\n\n\n\n\n\n\n\n{messageToReplace}",
                    new HintParameter[]
                    {
                        new StringHintParameter("")
                    }, HintEffectPresets.FadeInAndOut(0.25f, 1f, 0f)));
            }
        }

        public void RunWhenPlayerSwitchesClasses(ChangingRoleEventArgs ev)
        {
            if (ev.Player != _ply)
                return;

            counter = 0;
        }
    }
}