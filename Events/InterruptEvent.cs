namespace roguelike.Events
{
    public class InterruptEvent : Event
    {
        public InterruptEvent()
        {
            Interrupt = true;
        }
    }
}