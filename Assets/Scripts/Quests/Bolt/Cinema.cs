using System;
using Assets.Scripts.Modules.Quests.Triggers;
using Bolt;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Cinema")]
    public class Cinema :QuestState
    {
        protected override void Definition()
        {
            base.Definition();
            respones = new ControlOutput[] { ControlOutput("Finished") };
        }

        internal override Type GetPresenterType() => typeof(TriggerCinema);
    }
}
