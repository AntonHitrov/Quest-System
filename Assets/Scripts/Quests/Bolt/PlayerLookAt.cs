using Assets.Scripts.Modules.Units;
using Bolt;
using Ludiq;
using System;
using UniRx;
using UnityEngine;
using Unit = Bolt.Unit;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Player look at..")]
    [UnitCategory("Atropos/Quests")]
    public class PlayerLookAt : Unit
    {
        [DoNotSerialize]
        private IDisposable disposable;
        protected override void Definition()
        {
            var lookat = ValueInput<Transform>("Look At");
            var v_input = ValueInput<Action>("Undo ");
            var v_output = ValueOutput<Action>("Dispose", flow =>  disposable.Dispose);
            var c_output = ControlOutput("");
            ControlInput("Look", flow => 
            {
                if (v_input.hasAnyConnection)
                {
                    Action value= flow.GetValue<Action>(v_input);
                    value?.Invoke();
                }
                disposable = GameObject.FindObjectOfType<MainPlayer>().LookAt(flow.GetValue<Transform>(lookat));
                return c_output;
            });
        }
        
    }
}
