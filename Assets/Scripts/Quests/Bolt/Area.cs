using System;
using Bolt;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    public class Area :QuestState
    {
        protected override void Definition()
        {
            base.Definition();
            respones = new ControlOutput[] 
            {
                ControlOutput("Entered"),
                ControlOutput("Leave")
            };
        }

        internal override Type GetPresenterType() => typeof(TriggerCollider);
    }
}
