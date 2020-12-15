namespace roguelike.Events
{
    public abstract class Event
    {
        private static int _currentId { get; set; } = 0;
        public int Id { get; } = _currentId++;
        public bool Handled { get; set; } = false;
        public bool StopPropagation { get; set; } = false;
        public bool Interrupt { get; set; } = false;
        public bool InterruptOnCancel { get; set; } = false;
        public double ActivateIn { get; set; } = 0;
    }
}