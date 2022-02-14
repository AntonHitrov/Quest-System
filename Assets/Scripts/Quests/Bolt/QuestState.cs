using Assets.Scripts.Modules.Quests.Triggers;
using Bolt;
using Ludiq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using static UnityEngine.Debug;

namespace Assets.Scripts.Modules.Quests
{
    [UnitCategory("Atropos/Quests/States")]
    public abstract class QuestState : Unit, IGraphEventListener
    {

        #region EDITOR

#if UNITY_EDITOR
        internal static Quest @default;

        #region Scene
        [Inspectable]
        [UnitHeaderInspectable("Scene")]
        [DoNotSerialize]
        public UnityEditor.SceneAsset scene
        {
            get => asset != null ?
                asset.editorAsset as UnityEditor.SceneAsset :
                String.IsNullOrEmpty(SceneGUID) ?
                 CreateDefaultScene() :
                (asset = new AssetReference(SceneGUID)).editorAsset as UnityEditor.SceneAsset;
            set
            {
                asset = new AssetReference();
                asset.SetEditorAsset(value);
                SceneGUID = asset.AssetGUID;
                SceneName = value.name;
            }
        }
        [DoNotSerialize]
        public AssetReference asset;

        private UnityEditor.SceneAsset CreateDefaultScene()
        {
            SceneName = SceneManager.GetActiveScene().name;
            SceneGUID = UnityEditor.AssetDatabase.AssetPathToGUID(SceneManager.GetActiveScene().path);
            return (asset = new AssetReference(SceneGUID)).editorAsset as UnityEditor.SceneAsset;
        }
        #endregion
#endif
        #endregion

        #region Property
        public string SceneGUID, SceneName;
        [Inspectable,UnitHeaderInspectable("Quest")] public Quest quest = null;
        [Inspectable,UnitHeaderInspectable("State")]public string Name = "";
        [Inspectable,UnitHeaderInspectable("AsyncState")] public bool AsyncState = false;
        [Inspectable,UnitHeaderInspectable("Create Dispose")] public bool CreateDispose = false;
        [DoNotSerialize] private bool ValidScene => SceneName == SceneManager.GetActiveScene().name;
        [DoNotSerialize] protected StateReference currentReference => StateReference.Get(quest.guid, Name);
        [DoNotSerialize] protected ControlOutput[] respones;
        #endregion

        internal void Respone(int id)
        {
            Respone();
            Flow.New(reference).StartCoroutine(respones[id]);
        }

        internal void Respone()
        {
            StateReference.Get(quest.guid, Name).run.Value = false;
            activate = false;
        }

        protected virtual void Request()
        {
            Log($"Activate {Name} : {SceneName} : {quest.name} : {reference}");
            activate = true;
            if (!ValidScene) return;
            StateReference.Get(quest.guid, Name).Activate(this);
        }

        internal virtual Type GetPresenterType()=> null;



        #region IGraphEventListener
        [DoNotSerialize]
        private GraphReference reference;
        public void StartListening(GraphStack stack)
        {
            reference = stack.ToReference();
            questStates.Add(this);
            Log($"StartListening {Name} : {quest.guid}");
        }
        public void StopListening(GraphStack stack)
        {
            questStates.Remove(this);
            reference = null;
            Log($"StopListening {Name} : {quest.guid}");
        }
        public bool IsListening(GraphPointer pointer) => reference != null;
        #endregion

        #region Unit
        [DoNotSerialize] internal bool activate { get; private set; }
        [DoNotSerialize] private ControlOutput outputAsync;
        public bool coroutine => true;
        public override void BeforeAdd()
        {
            #region Editor
#if UNITY_EDITOR
            quest = quest == null ? @default : quest;
            if (graph != null && graph.elements != null && quest != null)
            {
                if (string.IsNullOrEmpty(Name))
                {
                    IEnumerable<QuestState> states = quest.GetStates; //graph.elements.Where(x => x is QuestState).Cast<QuestState>();

                    int count = 0;
                    while (states.Any(x => x.quest == quest && x.Name == $"State №{count}")) count++;
                    Name = $"State №{count}";
                }
            }
#endif
            #endregion
            base.BeforeAdd();

        }
        protected override void Definition()
        {
            if (CreateDispose)
            {
                ValueOutput<Action>("Dispose",flow=> Respone);
            }
            outputAsync = AsyncState ? ControlOutput("Async Next") : null;
            ControlInput("Activate                                ",
                    (flow) => { Request(); return outputAsync; });
        }
        #endregion

        #region Static
        private static List<QuestState> questStates = new List<QuestState>();
        private static IEnumerable<QuestState> states => questStates;


        private static IEnumerable<QuestState> GetQuestStates(string GUID) 
            => states.Where(x => x.quest.guid == GUID);
        private static IEnumerable<QuestState> GetQuestStates(string Name, string GUID)
            => GetQuestStates(GUID).Where(x => x.Name == Name);
        internal static void ActivateAll(string Name, string GUID)
        {
            foreach (var states in GetQuestStates(Name, GUID))
            {
                states.Request();
                if (states.AsyncState)
                {
                    Flow.New(states.reference).StartCoroutine(states.outputAsync);
                }
            }
        }
        #endregion
    }

}