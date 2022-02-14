using Bolt;
using Zenject;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Player")]
    [UnitCategory("Atropos/Quests/Player")]
    public class Player : Unit
    {
        protected override void Definition()
        {
            ValueOutput("",flow => ProjectContext.Instance.Container.Resolve<Networking.Realisation.CurrentPlayer>());
        }
    }
}
