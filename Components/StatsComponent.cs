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
        public int StrengthModifier { get => (Strength - 10) / 2; }
        public int Dexterity { get; set; }
        public int DexterityModifier { get => (Dexterity - 10) / 2; }
        public int Constitution { get; set; }
        public int ConstitutionModifier { get => (Constitution - 10) / 2; }
        public int Intelligence { get; set; }
        public int IntelligenceModifier { get => (Intelligence - 10) / 2; }
        public int Wisdom { get; set; }
        public int WisdomModifier { get => (Wisdom - 10) / 2; }
        public int Charisma { get; set; }
        public int CharismaModifier { get => (Charisma - 10) / 2; }
        public int HitDice { get; set; }
        public int ArmorClass { get; set; }
        public int ExperienceGained { get; set; }
    }
}