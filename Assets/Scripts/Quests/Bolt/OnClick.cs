using Bolt;
using System;

namespace Assets.Scripts.Modules.Quests.Bolt
{

    [UnitTitle("On Click")]
    class OnClick : QuestState
    {
        protected override void Definition()
        {
            base.Definition();
            respones = new ControlOutput[]
            {
                ControlOutput("OnClick"),
            };
        }

        internal override Type GetPresenterType() => typeof(Triggers.TriggerOnClick);

    }
}
