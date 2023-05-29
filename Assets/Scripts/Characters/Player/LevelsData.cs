using System.Collections.Generic;

public class LevelsData
{
    public List<LevelData> Levels { get; } = new List<LevelData>();

    public LevelsData(int maxLevel)
    {
        for (int i = 1; i <= maxLevel; i++)
        {
            int expTotal = (i == 1) ? 0 : Levels[i - 2].ExpTotal;
            Levels.Add(new LevelData(i, expTotal));
        }
    }
    public class LevelData
    {
        public int Level { get; }
        public int Exp { get; }
        public int ExpTotal { get; }

        public LevelData(int level, int expTotalLevelBefore)
        {
            Level = level;
            Exp = 25 * (level * level + 7 * level + 32);
            ExpTotal = Exp + expTotalLevelBefore;
        }
    }
}

