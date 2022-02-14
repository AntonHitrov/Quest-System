using Bolt;
using Ludiq;

namespace Assets.Scripts.Modules.Quests.Bolt
{

    [UnitTitle("Start Quest")]
    [UnitCategory("Atropos/Quests")]
    class StartQuest : Unit , IGraphEventListener
    {
        [DoNotSerialize] private ControlOutput output;
        [DoNotSerialize] private GraphReference reference;

        protected override void Definition() => output = ControlOutput("Start");
        internal void Start() => Flow.New(reference).StartCoroutine(output);

        bool IGraphEventListener.IsListening(GraphPointer pointer) => reference != null;
        void IGraphEventListener.StartListening(GraphStack stack) => reference = stack.AsReference();
        void IGraphEventListener.StopListening(GraphStack stack) => reference = null;
    }
}
