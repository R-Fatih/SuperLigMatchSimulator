namespace SuperLigMatchSimulator.Classes
{
    public class Standing
    {
        public string Team { get; set; }
        public string TeamLogo { get; set; }
        public int Played { get; set; }
        public int Won { get; set; }
        public int Drawn { get; set; }
        public int Lost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
        public TrendDirection TrendDirection { get; set; }
        public int ReductedPoints { get; set; }
    }
    public enum TrendDirection
    {
        Up,
        Down,
        Equal
    }
}
