using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Modules.Quests.Triggers
{
    [RequireComponent(typeof(SphereCollider))]
    class TriggerOnClick : Trigger
    {
        public UnityEvent OnDown;

        private void Awake()
        {
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<SphereCollider>().isTrigger = true;
        }

        public void OnMouseDown()
        {
            if(run)
            {
                Respone(0);
                OnDown.Invoke();
            }
        }

        protected override void BeginListened() => GetComponent<SphereCollider>().enabled = true;

        protected override void EndListened() => GetComponent<SphereCollider>().enabled = false;
    }
}
