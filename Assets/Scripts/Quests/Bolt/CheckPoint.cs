using System;
using Bolt;
using Ludiq;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Check Point")]
    public class CheckPoint : QuestState
    {
        protected override void Request()
        {
            UnityEngine.Debug.Log($"I saved state {Name} fore Quest {quest.name}");
        }

        protected override void Definition()
        {
            base.AsyncState = true;
            base.Definition();
        }

        internal override Type GetPresenterType() => null;
    }
}
