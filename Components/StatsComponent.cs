namespace roguelike.Components
{
    public class StatsComponent : Component
    {
        public enum Stat
        {
            Strength,
            Dexterity,
            Constitution,
            Intelligence,
            Wisdom,
            Charisma
        }
        public int Strength { get; set; }
        public int StrengthModifier { get => Modifier(Strength); }
        public int Dexterity { get; set; }
        public int DexterityModifier { get => Modifier(Dexterity); }
        public int Constitution { get; set; }
        public int ConstitutionModifier { get => Modifier(Constitution); }
        public int Intelligence { get; set; }
        public int IntelligenceModifier { get => Modifier(Intelligence); }
        public int Wisdom { get; set; }
        public int WisdomModifier { get => Modifier(Wisdom); }
        public int Charisma { get; set; }
        public int CharismaModifier { get => Modifier(Charisma); }
        public int HitDice { get; set; }
        public int ArmorClass { get; set; }
        public int ExperienceGained { get; set; }

        private static int Modifier(int stat)
        {
            return (stat - 10) / 2;
        }
    }
}