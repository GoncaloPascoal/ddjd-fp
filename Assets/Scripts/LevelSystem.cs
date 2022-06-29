using System;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    public static LevelSystem Instance;

    public int Experience { get; private set; } = 0;
    public int Level { get; private set; } = 1;
    private Dictionary<StatName, int> _statPoints;

    public int StatPointsRemaining => Level - _statPoints.Values.Sum() - 1;

    [SerializeField] public StatsDictionary boostPerLevel;

    private void Awake()
    {
        Instance = this;
        
        _statPoints = new Dictionary<StatName, int>();
        foreach (StatName stat in Enum.GetValues(typeof(StatName)))
        {
            _statPoints.Add(stat, 0);
        }
    }

    public void AddExperience(int experience)
    {
        Experience += experience;
        while (CanLevelUp()) LevelUp();
    }

    private void LevelUp()
    {
        Experience -= ExperienceToLevelUp();
        Level++;
    }

    private bool CanLevelUp()
    {
        return Experience >= ExperienceToLevelUp();
    }

    public int ExperienceToLevelUp()
    {
        return ExperienceToLevelUp(Level);
    }

    private static int ExperienceToLevelUp(int level)
    {
        return (int) (level * Mathf.Sqrt(level)) + 2 * level;
    }

    public bool HasStatPoints()
    {
        return StatPointsRemaining > 0;
    }

    public void AddStatPoint(StatName stat)
    {
        if (StatPointsRemaining > 0) _statPoints[stat] += 1;
    }

    public int GetStatPoints(StatName stat)
    {
        return _statPoints[stat];
    }

    public float GetLevelBonus(StatName stat)
    {
        return boostPerLevel[stat] * _statPoints[stat];
    }
}
