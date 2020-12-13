namespace roguelike.Events
{
    public class OnAttackRollFailedEvent : AttackEvent
    {
        public int Roll { get; set; }
        public int Required { get; set; }
    }
}