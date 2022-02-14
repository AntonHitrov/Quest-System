using Assets.Scripts.Modules.Quests.Core;
using Bolt;
using Ludiq;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Accept Quest")]
    [UnitCategory("Atropos/Quests")]
    public class AddQuest : Unit
    {
        [Inspectable]
        [UnitHeaderInspectable("Quest")]
        public Quest quest;

        protected override void Definition()
        {
#if UNITY_EDITOR
            quest = quest == null ? QuestState.@default : quest;
#endif
            ControlOutput ControleOut = ControlOutput("                       ");
            ControlInput("Accept", (flow)=> { QuestRuntime.Load(quest);  return ControleOut; });
        }
    }
}
