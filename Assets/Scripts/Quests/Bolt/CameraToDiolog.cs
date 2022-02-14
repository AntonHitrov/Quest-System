
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
        [DoNotSerialize] private List<IDisposable> disposables = new List<IDisposable>();

        protected override void Definition()
        {
            var transform = ValueInput<Transform>("Look At");
            var dispose = ValueOutput<Action>("Dispose",flow=> Unlock);
            var output = ControlOutput("Next");
            ControlInput("Set Camera",
                flow=> 
                {
                    var target = flow.GetValue<Transform>(transform);
                    var player = GameObject.FindObjectOfType<MainPlayer>();
                    var camera = new GameObject().AddComponent<Camera>();

                    disposables.Add(player.LookAt(target));
                    var rotation = target.rotation;
                    target.LookAt(player.transform);
                    camera.transform.position = player.positionToCamera;
                    camera.transform.LookAt(target);
                    Camera.SetupCurrent(camera);

                    disposables.Add(UniRx.Disposable.Create(
                        () => 
                        {
                            GameObject.Destroy(camera.gameObject);
                            target.rotation = rotation;
                        }));
                    return output;
                });
        }


        private void Unlock()
        {
            disposables.ForEach(action => action.Dispose());
            disposables = new List<IDisposable>();
        }
    }
}
