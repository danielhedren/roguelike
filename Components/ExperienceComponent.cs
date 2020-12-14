using System;
namespace roguelike.Components
{
    public class ExperienceComponent : Component
    {
        public int Experience { get; set; }
        public int ExperienceToNextLevel { get => (int) (100 * Math.Pow(Level, 2)) - Experience; }
        public int Level { get => (int) Math.Floor(Math.Sqrt(Experience/100)) + 1; }
        public ExperienceComponent()
        {

        }
    }
}
