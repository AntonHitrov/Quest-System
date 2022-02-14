using Assets.Scripts.Modules.Quests.Core;
using Bolt;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Modules.Quests.Triggers
{
    [System.Serializable]
    public struct Reference { public string GUID, Name; }

    public class StateReference :MonoBehaviour
    {
        #region EDITOR
#if UNITY_EDITOR
        [HideIf(nameof(binded))]
        public Quest Quest = QuestState.@default;
        [ShowNativeProperty]
        public string QuestName => Quest != null ? Quest.name : "";

        private List<string> getStates => Quest != null && Quest.macro != null ? Quest.GetStatesName() : null;

        public DropdownList<Reference> getReferences
        {
            get
            {
                var dropList = new DropdownList<Reference>();
                if(Quest != null)
                foreach (var refer in getStates.Select(x => new Reference() { GUID = Quest.guid, Name = x }))
                    dropList.Add(refer.Name, refer);
                return dropList;
            }
        }
        [SerializeField]
        [Dropdown(nameof(getReferences))]
        [HideIf(nameof(binded))]
#endif
        #endregion

        #region Property
        public Reference Reference;
        [HideInInspector] public BoolReactiveProperty run = new BoolReactiveProperty(false);

        [ShowNativeProperty] private string Name => Reference.Name != null ? Reference.Name : "";
        [ShowNativeProperty] private string GUID => Reference.GUID != null ? Reference.GUID : "";
        private QuestState state;
        #endregion

        internal void Activate(QuestState state)
        {
            this.state = state;
            run.Value = true;
        }

        internal void Respone(int id) => state?.Respone(id);

        static internal StateReference Get(string guid, string name)
            => FindObjectsOfType<StateReference>().Where(x => x.GUID == guid && x.Name == name)
                                                  .FirstOrDefault();

        #region EDITOR
#if UNITY_EDITOR
        private bool haveRef => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(GUID) && GetComponents<Component>().Count() <= 2;
        private bool binded=> !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(GUID) && GetComponents<Component>().Count() > 2;
        [Button]
        [ShowIf(nameof(haveRef))]
        internal void Bind()
        {
            System.Type type = Quest.GetStates.Where(x => x.Name == Name && x.quest.guid == GUID).First().GetPresenterType();
            gameObject.AddComponent(type);
            foreach (Collider c in GetComponents<Collider>())
                c.isTrigger = true;
            GameObject parent = GameObject.Find($"{QuestName}");
            if (parent == null)
            {
                parent = new GameObject($"{QuestName}");
                parent.AddComponent<ReferenceQuest>().quest = Quest;
            }

            transform.parent = parent.transform;
            gameObject.name = $"{QuestName} : {Name}";
            var texture = UnityEditor.EditorGUIUtility.IconContent("sv_label_1").image as Texture2D;
            typeof(UnityEditor.EditorGUIUtility).InvokeMember("SetIconForObject", System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, null, new object[] { gameObject, texture });
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }

        [UnityEditor.MenuItem("GameObject / Quests / State Reference", false,10)]
        public static void CreateReference(UnityEditor.MenuCommand menu)
        {
            var referrence = new GameObject("State Reference");
            referrence.AddComponent<StateReference>();
            UnityEditor.GameObjectUtility.SetParentAndAlign(referrence,menu.context as GameObject);
            UnityEditor.Undo.RegisterCreatedObjectUndo(referrence, "Create " + referrence.name);
            UnityEditor.Selection.activeObject = referrence;

        }

        [ContextMenu("Select Quest")]
        public void SelectQuest() => UnityEditor.Selection.activeObject = Quest;

        [ContextMenu("Edit Graf")]
        public void EditGraf()
        {
            if (Application.isPlaying && QuestRuntime.hasQuestRuntime(GUID))
            {
                
                Ludiq.GraphWindow.OpenTab();
                UnityEditor.Selection.activeObject = QuestRuntime.Get(GUID).gameObject;
            }
            else
            {
                Quest.EditGraf();
            }
        }
#endif
        #endregion
    }

}