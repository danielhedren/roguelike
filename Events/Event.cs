namespace roguelike.Events
{
    public abstract class Event
    {
        public bool Handled { get; set; } = false;
        public bool StopPropagation { get; set; } = false;
        public bool Interrupt { get; set; } = false;
        public double ActivateIn { get; set; } = 0;
    }
}