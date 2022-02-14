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
        #region Property
        [Inspectable, UnitHeaderInspectable("Count")]
        public int Count = 0;

        [DoNotSerialize]
        private List<ValueInput> inputValues = new List<ValueInput>();
        #endregion

        protected override void Definition()
        {
            inputValues = Enumerable.Range(0, Count)
                                    .Select(i => ValueInput<Action>($"Inpput {i}"))
                                    .ToList();
            ValueOutput<Action>("Combine", flow => ()=> Invoke(flow));

            var output = ControlOutput("Activated");
            ControlInput("Activate", 
                flow =>
                {
                    Invoke(flow);
                    return output;
                });
        }

        private void Invoke(Flow flow) 
            => inputValues.ForEach(value => flow.GetValue<Action>(value).Invoke());
    }
}
