using Assets.Scripts.Modules.Units;
using Bolt;
using UnityEngine;

namespace Assets.Scripts.Modules.Quests.Bolt
{
    [UnitTitle("Transform")]
    [UnitCategory("Atropos/Quests/Player")]
    public class MainPlayerTransform : Unit
    {
        protected override void Definition() 
            => ValueOutput<Transform>("Transform", flow => GameObject.FindObjectOfType<MainPlayer>().transform);
    }
}
