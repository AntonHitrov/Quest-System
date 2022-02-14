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
    internal class QuestRuntime :MonoBehaviour
    {
        #region Property
        private static List<QuestRuntime> questSubjects = new List<QuestRuntime>();

        [ShowNativeProperty] private string CurrentState => statesInGraph.Where(x => x.activate).Select(x => x.Name).FirstOrDefault() ?? "Unloaded State";
        [ShowNativeProperty] private string GUID => gameObject.name;

        private IEnumerable<QuestState> statesInGraph => quest.GetStates;
        private Quest quest;
        private FlowMachine machine;
        #endregion

        private void Activate(Quest quest,string state)
        {
            this.quest = quest;
            if (hasQuestRuntime(GUID))
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                questSubjects.Add(this);
                gameObject.OnDestroyAsObservable().Subscribe(_=>questSubjects.Remove(this));

                machine = gameObject.AddComponent<FlowMachine>();
                machine.nest.SwitchToMacro(quest.macro);

                if (string.IsNullOrEmpty(state))
                {
                    quest.GetStart.Start();
                }
                else
                {
                    Activate(state);
                }
            }
        }

        private void Activate(string state) => QuestState.ActivateAll(state, GUID);

        internal void Respone(string Name, int id)
            => statesInGraph.Where(x => x.Name == Name)
                            .FirstOrDefault()?
                            .Respone(id);

        #region static
        internal static QuestRuntime Get(string GUID)
            => questSubjects.Where(x => x.GUID == GUID)
                            .First();

        internal static bool hasQuestRuntime(string GUID) 
            => questSubjects.Any(x => x.GUID == GUID);

        internal static bool Acectable(string GUID) => !hasQuestRuntime(GUID);

        internal static void Load(string GUID,string state = "")
            => new AssetReference(GUID)
                .LoadAssetAsync<Quest>()
                .Completed += handler => Load(handler.Result,state);

        internal static void Load(Quest quest,string state = "") 
            => new GameObject(quest.guid)
                .AddComponent<QuestRuntime>()
                .Activate(quest,string.IsNullOrEmpty(state) ? quest.CurrentState : state);
        #endregion
    }
}
