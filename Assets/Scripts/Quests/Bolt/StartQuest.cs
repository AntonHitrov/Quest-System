using Bolt;
using Ludiq;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Start Quest")]
    [UnitCategory("Atropos/Quests")]
    class StartQuest : Unit , IGraphEventListener
    {
        #region Property
        [DoNotSerialize] private ControlOutput output;
        [DoNotSerialize] private GraphReference reference;
        #endregion

        protected override void Definition() => output = ControlOutput("Start");

        internal void Start() => Flow.New(reference).StartCoroutine(output);

        #region IGraphEventListener
        bool IGraphEventListener.IsListening(GraphPointer pointer) => reference != null;
        void IGraphEventListener.StartListening(GraphStack stack) => reference = stack.AsReference();
        void IGraphEventListener.StopListening(GraphStack stack) => reference = null;
        #endregion
    }
}
