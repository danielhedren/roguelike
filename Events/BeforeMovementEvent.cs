namespace roguelike.Events
{
    public class BeforeMovementEvent : MovementEvent
    {
        public bool CancelMovement { get; set; } = false;
    }
}