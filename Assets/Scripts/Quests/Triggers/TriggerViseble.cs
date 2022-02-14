namespace Assets.Scripts.Modules.Quests
{
    public class TriggerViseble :Trigger
    {
        public EventTypeViseble EventType;
        
        private void Invoke(string name)
        {
            if (run && EventType.ToString() == name) Respone((int)EventType);
        }
        

        private void OnBecameInvisible() => Invoke(nameof(OnBecameInvisible));
        private void OnBecameVisible() => Invoke(nameof(OnBecameVisible));
    }

    public enum EventTypeViseble:int
    {
        OnBecameInvisible, OnBecameVisible
    }
}
