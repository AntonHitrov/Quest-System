using Bolt;
using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Modules.Quests.Bolt
{

    [UnitTitle("Deactivate GUI")]
    [UnitCategory("Atropos/Quests")]
    public class OffGUI : Unit
    {
        protected override void Definition()
        {
            var output_c = ControlOutput("Next  ");
            var output_v = ValueOutput<Action>("Undo",flow=>Undo);
            ControlInput("Off GUI",
                flow =>
                {
                    SetActive(false);
                    return output_c;
                });
        }

        private static void SetActive(bool value) 
            => GameObject.FindObjectOfType<SceneContext>().Container.ResolveId<Canvas>(nameof(UI.Canvas.UI.SceneCanvas)).gameObject.SetActive(value);

        private void Undo()
            => SetActive(true);
    }
}
