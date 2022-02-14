using System.Collections.Generic;
using Bolt;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UniRx.Triggers;
using UniRx;
using NaughtyAttributes;
using System.Linq;

namespace Assets.Scripts.Modules.Quests.Core
{
    public class QuestRuntime :MonoBehaviour
    {
        private static List<QuestRuntime> questSubjects = new List<QuestRuntime>();

        [ShowNativeProperty]
        string CurrentState => statesInGraph.Where(x => x.activate).Select(x => x.Name).FirstOrDefault() ?? "Unloaded State";
        IEnumerable<QuestState> statesInGraph => quest.GetStates;
        [ShowNativeProperty]
        string GUID =>gameObject.name;
        Quest quest;
        FlowMachine machine;


        internal static void Load(string GUID,string state = "")
            => new AssetReference(GUID)
                .LoadAssetAsync<Quest>()
                .Completed += handler => Load(handler.Result,state);
        internal static void Load(Quest quest,string state = "") 
            => new GameObject(quest.guid)
                .AddComponent<QuestRuntime>()
                .Activate(quest,string.IsNullOrEmpty(state) ? quest.CurrentState : state);
        
        private void Activate(Quest quest,string state)
        {
            this.quest = quest;
            if (hasQuestRuntime(GUID))
                Destroy(gameObject);
            questSubjects.Add(this);
            DontDestroyOnLoad(this.gameObject);
            gameObject.OnDestroyAsObservable().Subscribe(_=>questSubjects.Remove(this));
            machine = gameObject.AddComponent<FlowMachine>();
            machine.nest.SwitchToMacro(quest.macro);
            if (!string.IsNullOrEmpty(state))
            {
                Debug.Log("State " + state);
                Activate(state);
            }
            else
            {
                Debug.Log("Started");
                quest.GetStart.Start();
            }
        }
        
        private void Activate(string state) => QuestState.ActivateAll(state, GUID);

        public void Respone(string Name, int id) => statesInGraph.Where(x => x.Name == Name).FirstOrDefault()?.Respone(id);

        public static QuestRuntime Get(string GUID) => questSubjects.Where(x => x.GUID == GUID).First();
        public static bool hasQuestRuntime(string GUID) => questSubjects.Any(x => x.GUID == GUID);

        public static bool Acectable(string GUID) => !hasQuestRuntime(GUID);
    }
}
