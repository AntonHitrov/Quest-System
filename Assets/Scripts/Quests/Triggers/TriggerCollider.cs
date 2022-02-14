using Assets.Scripts.Modules.Units;
using Assets.Scripts.Modules.Units.Controlers;
using NaughtyAttributes;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.Quests
{
    
    [RequireComponent(typeof(SphereCollider))]
    public class TriggerCollider : Trigger
    {
        #region Property
        public EventTypeCollider EventType;
        public TriggerTarget Target;
        [ShowIf(nameof(IsAnyTarget))] public string[] indificators;

        internal bool IsAnyTarget => Target == TriggerTarget.AnyHasIndificator;
        #endregion

        private void Awake()
        {
            gameObject.layer = Physics.IgnoreRaycastLayer;
            GetComponent<SphereCollider>().isTrigger = true;
        }

        private void Invoke(string name, Collider collider)
        {
            if (run && EventType.ToString() == name)
            {
                if (Target == TriggerTarget.MainPlayer && collider.GetComponent<MainPlayer>() != null)
                {
                    Respone((int)EventType);
                }
                else
                {
                    CastTarget target = collider.GetComponent<CastTarget>();
                    if (Target == TriggerTarget.AnyHasIndificator && target != null && indificators.Any(x => target.indificator == x))
                        Respone((int)EventType);
                }
            }
        }

        private void OnTriggerEnter(Collider other) => Invoke(nameof(OnTriggerEnter), other);
        private void OnTriggerExit(Collider other) => Invoke(nameof(OnTriggerExit), other);
    }

    public enum TriggerTarget
    {
        MainPlayer,
        AnyHasIndificator
    }

    public enum EventTypeCollider :int
    {
        OnTriggerEnter,
        OnTriggerExit
    }
}
