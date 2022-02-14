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
        private ControlOutput[] outputsDialog;

        [DoNotSerialize]
        private List<IDisposable> disposables = new List<IDisposable>();

        protected override void Definition()
        {
            outputsDialog = Enumerable.Range(0, dialogs.Length)
                                      .Select(i => ControlOutput($"Variant {i}"))
                                      .ToArray();
            ControlInput("Begin",
                flow => 
                {
                    reference = flow.stack.ToReference();
                    var factory = UnityEngine.Object
                                             .FindObjectOfType<SceneContext>()
                                             .Container
                                             .ResolveId<IFactory<Sprite, Action, string, IDisposable>>(nameof(UI.Canvas.UI.Cinema));
                    disposables = Enumerable.Range(0, dialogs.Length)
                                            .Select( i => factory.Create(null, () => Select(i, reference), dialogs[i]))
                                            .ToList();
                    return null;
                });
        }

        private void Select(int i, GraphReference reference)
        {
            disposables.ForEach(element => element.Dispose());
            Flow.New(reference).StartCoroutine(outputsDialog[i]);
        }
    }
}
