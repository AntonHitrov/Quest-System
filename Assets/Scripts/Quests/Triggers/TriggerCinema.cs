using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Modules.Quests.Triggers
{

    [RequireComponent(typeof(GUI.Kinematics.Cinema))]
    public class TriggerCinema :Trigger
    {
        protected override void BeginListened()
        {
            cinema = GetComponent<GUI.Kinematics.Cinema>();
            cinema.AutoEnd = true;
            cinema.OnComplited.AsObservable().Take(1).Subscribe(_=>Respone(0)).AddTo(this);
            cinema.Show();
        }

        private GUI.Kinematics.Cinema cinema;
    }
}
