using Bolt;
using Ludiq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Modules.Quests.Bolt
{

    [UnitTitle("Question")]
    [UnitCategory("Atropos/Quests")]
    public class Question : Unit
    {
        [Inspectable]
        [UnitHeaderInspectable("Questions")]
        public string[] dialogs = new string[] { };
        
        [DoNotSerialize]
        GraphReference reference;
        [DoNotSerialize]
        private ControlOutput[] outputsDialog;

        [DoNotSerialize]
        private List<IDisposable> disposables = new List<IDisposable>();

        protected override void Definition()
        {
            outputsDialog = new ControlOutput[dialogs.Length];
            foreach (var i in Enumerable.Range(0,dialogs.Length))
            {
                outputsDialog[i] = ControlOutput($"Variant {i}");
            }
            ControlInput("Begin                                      ",
                (flow)=> 
                {
                    var factory = UnityEngine
                                    .Object
                                    .FindObjectOfType<SceneContext>()
                                    .Container
                                    .ResolveId<IFactory<Sprite, Action, string, IDisposable>>(nameof(UI.Canvas.UI.Cinema));
                    reference = flow.stack.ToReference();
                    
                    foreach (var i in Enumerable.Range(0, dialogs.Length))
                    {
                        disposables.Add(factory.Create(null,()=>Select(i),dialogs[i]));
                    }

                    return null;
                });
        }

        private void Select(int i)
        {
            foreach (var dis in disposables)
                dis.Dispose();
            Flow.New(reference).StartCoroutine(outputsDialog[i]);
        }
    }
}
