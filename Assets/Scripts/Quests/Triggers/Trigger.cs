using Assets.Scripts.Modules.Quests.Triggers;
using NaughtyAttributes;
using UnityEngine;
using UniRx;
using UnityEngine.Events;

namespace Assets.Scripts.Modules.Quests
{
    [RequireComponent(typeof(StateReference))]
    public class Trigger : MonoBehaviour
    {
        #region Property

        private StateReference reference;
        [ShowNativeProperty] protected bool run => reference != null ? reference.run.Value : false;

        #region public
        public UnityEvent Begin;
        public UnityEvent End;
        #endregion
        #endregion

        protected virtual void Start()
        {
            reference = GetComponent<StateReference>();
            reference.run.Skip(1).Subscribe(Handler).AddTo(gameObject);
        }

        public void Respone(int id) => reference.Respone(id);

        protected virtual void BeginListened() { }
        protected virtual void EndListened() { }

        private void Handler(bool runing)
        {
            if (runing)
            {
                BeginListened();
                Begin.Invoke();
            }
            else
            {
                EndListened();
                End.Invoke();
            }
        }

        #region Editor
#if UNITY_EDITOR
        [ContextMenu("Select Quest")]
        public void SelectQuest() => GetComponent<StateReference>().SelectQuest();

        [ContextMenu("Edit Graf")]
        public void EditGraf() => GetComponent<StateReference>().EditGraf();
#endif
        #endregion
    }



}
