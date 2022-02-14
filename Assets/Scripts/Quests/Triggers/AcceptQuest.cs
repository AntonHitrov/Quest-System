using Assets.Scripts.Modules.Quests.Core;
using UniRx;

namespace Assets.Scripts.Modules.Quests.Triggers
{
    public class AcceptQuest : Atropos.GUI.Actions.Action
    {
        public Quest Quest;
        public bool DestroyAfterEvent = true;

        public void Start()
        {
            if (QuestRuntime.Acectable(Quest.guid))
                this.OnCliced.AsObservable().Take(1).Subscribe(_ => { QuestRuntime.Load(Quest); Destroy(gameObject); }).AddTo(this);
            else
                Destroy(gameObject);
        }
    }
}
