using Assets.Scripts.Modules.Units.Spawners;
using UnityEngine;
using Unit = Atropos.Core.Unit;

namespace Assets.Scripts.Modules.Quests.Triggers
{
    public class TriggerSpawn :Trigger
    {
        #region Property
        public Prototype prototype;
        public bool HasBehavior = false;
        public Transform proto_Transform => ProtoObject == null ?
                                            null :
                                            ProtoObject is Unit ?
                                            ProtoAsUnit.transform :
                                            ProtoAsGameObject.transform;
        private System.Object ProtoObject;
        private Unit ProtoAsUnit => ProtoObject as Unit;
        private GameObject ProtoAsGameObject => ProtoObject as GameObject;
        #endregion


        protected override void BeginListened()
        {
            base.BeginListened();
            ProtoObject = HasBehavior ?
                          prototype.Spawn(transform.position, transform.rotation):
                          prototype.Spawn_nobehavior(transform.position,transform.rotation);
        }

        protected override void EndListened()
        {
            base.EndListened();
            if( ProtoObject != null )
            Destroy( proto_Transform.gameObject );
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
