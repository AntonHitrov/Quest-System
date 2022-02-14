
using Assets.Scripts.Modules.Units;
using Bolt;
using Ludiq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules.Quests.Bolt
{

    [UnitTitle("Set camera to diolog")]
    [UnitCategory("Atropos/Quests")]
    public class CameraToDiolog : Unit
    {
        protected override void Definition()
        {
            var transform = ValueInput<Transform>("Look At");
            var dispose = ValueOutput<Action>("Dispose",flow=> unlock);
            var output = ControlOutput("Next");
            ControlInput("Set Camera",
                flow=> 
                {
                    var at = flow.GetValue<Transform>(transform);
                    var player = GameObject.FindObjectOfType<MainPlayer>();
                    var cam = new GameObject().AddComponent<Camera>();

                    disposables.Add(player.LookAt(at));
                    var rotation = at.rotation;
                    at.LookAt(player.transform);
                    cam.transform.position = player.positionToCamera;
                    cam.transform.LookAt(at);
                    Camera.SetupCurrent(cam);
                    disposables.Add(UniRx.Disposable.Create(() => { GameObject.Destroy(cam.gameObject); at.rotation = rotation; }));
                    return output;
                });
        }

        [DoNotSerialize]
        private List<IDisposable> disposables = new List<IDisposable>();

        private void unlock()
        {
            foreach (var d in disposables)
            {
                d.Dispose();
            }
            disposables = new List<IDisposable>();
        }
    }
}
