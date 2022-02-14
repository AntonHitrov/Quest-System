using NaughtyAttributes;
using UnityEngine;

namespace Assets.Scripts.Modules.Quests.Triggers
{
    internal class ReferenceQuest:MonoBehaviour
    {
        [HideInInspector]
        public Quest quest;

#if UNITY_EDITOR
        [Button]
        public void EditGraph()
        {
            quest.EditGraf();
        }
#endif
    }
}