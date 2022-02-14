using Bolt;
using Ludiq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Modules.Quests.Bolt
{

    [UnitTitle("Composite action")]
    [UnitCategory("Atropos/Quests")]
    public class ComposeAction : Unit
    {
        [Inspectable]
        [UnitHeaderInspectable("Count")]
        public int Count = 0;

        [DoNotSerialize]
        private List<ValueInput> inputValues = new List<ValueInput>();


        protected override void Definition()
        {
            foreach (var i in Enumerable.Range(0, Count))
            {
                inputValues.Add(ValueInput<Action>($"Inpput {i}"));
            }
            var output = ControlOutput("Activated");
            ValueOutput<Action>("Combine", flow => ()=> Invoke(flow));
            ControlInput("Activate   ", 
                flow =>
                {
                    Invoke(flow);
                    return output;
                });
        }

        private void Invoke(Flow flow)
        {
            foreach (var i in inputValues)
            {
                flow.GetValue<Action>(i).Invoke();
            }
        }
    }
}
