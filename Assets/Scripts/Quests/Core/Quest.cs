using Bolt;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UniRx;
using Assets.Scripts.Modules.Units;
using Ludiq;
using Assets.Scripts.Modules.Quests.Triggers;

namespace Assets.Scripts.Modules.Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Atropos/new Quest", order = 1)]
    public class Quest :Asset 
    {
        #region Info
       // [HideInInspector]public AssetReference Reference;
        [ShowNativeProperty]internal string guid => string.IsNullOrEmpty(Reference.AssetGUID) ? "Exception: GUID don't set" : Reference.AssetGUID;
        #endregion

        #region Settings
        public bool Hiden = true;
        [HideIf(nameof(Hiden))] public string Title;
        [HideIf(nameof(Hiden))] public string DescriptionShort;
        [HideIf(nameof(Hiden)),TextArea] public string Description;
        [HideInInspector] public FlowMacro macro;
        [HideInInspector] public string State = "";
        public string CurrentState
        {
            get
            {
                if (!values.ContainsKey("STATE"))
                    return State;
                else
                    return values["STATE"];
            }
            set
            {
                if (!values.ContainsKey("STATE"))
                    values.Add("STATE", value);
                else
                    values["STATE"] = value;
                (values as Networking.Realisation.Item).Save();
            }
        }
        #endregion

        private IDictionary<string, string> values = new Dictionary<string, string>();
        public override IDictionary<string, string>  Values
        {
            get
            {
                if (!values.ContainsKey("GUID"))
                    values.Add("GUID", Reference.AssetGUID);
                return values;
            }
            set => values = value;
        }

        #region ListState
        public List<string> GetStatesName() => GetAll<QuestState>(macro.graph).Select(x => x.Name).ToList();
        public List<QuestState> GetStates => GetAll<QuestState>(macro.graph).ToList();
        internal Bolt.StartQuest GetStart => GetAll<Bolt.StartQuest>(macro.graph).First();

        private IEnumerable<Element> GetAll<Element>(IGraph graph)
            => GetStateInGraph<Element>(graph).Concat(GetSubGraphs(graph)
                                              .SelectMany(x => GetAll<Element>(x)));

        private static IEnumerable<Element> GetStateInGraph<Element>(IGraph graph) 
            => graph.elements.Where(x => x is Element)
                             .Cast <Element> ();
        private static IEnumerable<IGraph> GetSubGraphs(IGraph graph)
            => graph.elements.Where(x => x is IGraphParent)
                             .Cast<IGraphParent>()
                             .Select(x=>x.childGraph);
        #endregion

        #region EDITOR
#if UNITY_EDITOR

        #region AssetRef
        private bool hasAssetReference => !string.IsNullOrEmpty(Reference.AssetGUID);
        #endregion

        #region Macro Inspector
        private bool hasMacro => macro != null;


        [Button]
        [HideIf(nameof(hasMacro))]
        public void CreateFlowMacro()
        {
            macro = ScriptableObject.CreateInstance<FlowMacro>();
            macro.name = $"Graph {this.name}";
            UnityEditor.AssetDatabase.AddObjectToAsset(macro, this);
            UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(macro));
        }

        [Button]
        [ShowIf(nameof(hasMacro))]
        public void EditGraf()
        {
            if (macro == null) return;
            QuestState.@default = this;
            Ludiq.GraphWindow.OpenTab();
            UnityEditor.Selection.activeObject = macro;
        }
        #endregion

        #region Saver
        [Button]
        public void Save()
        {
            if(macro != null)
                UnityEditor.EditorUtility.SetDirty(macro);
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }


        #endregion
        [Button]
        void CreateAllReference() 
              => GetStatesName().Distinct()
                                .Where(x => StateReference.Get(guid, x) == null)
                                .ToList()
                                .ForEach(
                                name =>
                                {
                                    var reference = new GameObject().AddComponent<StateReference>();
                                    reference.Reference = new Reference() { Name = name, GUID = guid };
                                    reference.Bind();
                                });
#endif
        #endregion
    }

}
