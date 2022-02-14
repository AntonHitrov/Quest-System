using Assets.Scripts.Modules.Quests.Triggers;
using Bolt;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    public class Spawn : QuestState
    {
        internal override Type GetPresenterType() => typeof(TriggerSpawn);



        protected override void Definition()
        {
            AsyncState = true;
            base.Definition();
            respones = new ControlOutput[]
            {
                ControlOutput("Un Spawned")
            };
            ValueOutput<Transform>("Unit",flow=> currentReference.GetComponent<TriggerSpawn>().proto_Transform);
            ControlInput("Un Spawn",flow=> { Respone(0); return null; } );
        }
    }
}
