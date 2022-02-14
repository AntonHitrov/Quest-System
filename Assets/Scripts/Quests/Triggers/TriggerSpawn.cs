using Assets.Scripts.Modules.Units.Spawners;
using UnityEngine;
using Unit = Atropos.Core.Unit;

namespace Assets.Scripts.Modules.Quests.Triggers
{
    public class TriggerSpawn :Trigger
    {
        public Prototype prototype;
        public bool HasBehavior = false;

        public Transform proto_Transform => ProtoObject != null ? (ProtoObject is Unit ? (ProtoObject as Unit).transform : (ProtoObject as GameObject).transform) : null;
        private System.Object ProtoObject;
        protected override void BeginListened()
        {
            base.BeginListened();
            if(HasBehavior)
                ProtoObject = prototype.Spawn(transform.position, transform.rotation);
            else
            {
                Debug.Log($"{transform.position}");
                Debug.Log($"{transform.rotation}");
               
                ProtoObject = prototype.Spawn_nobehavior(transform.position,transform.rotation);

                Debug.Log($"{(ProtoObject as GameObject).transform.position}");
                Debug.Log($"{(ProtoObject as GameObject).transform.rotation}");
            }
        }

        protected override void EndListened()
        {
            base.EndListened();
            if(ProtoObject != null)
            Destroy(HasBehavior ? (ProtoObject as Unit).transform.gameObject : ProtoObject as GameObject);
        }
    }
    [System.Serializable]
    public class SpawnPoint
    {
        public Prototype prototypes;
        public Transform SpawnPlases;

        public GameObject Spawn_nobehavior() => prototypes.Spawn_nobehavior(SpawnPlases.position,SpawnPlases.rotation);
        public Unit Spawn() => prototypes.Spawn(SpawnPlases.position, SpawnPlases.rotation);

    }
}
