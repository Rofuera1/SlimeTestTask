using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Player
{
    public static class Stats
    {
        public enum Types
        {
            AttackValue,
            AttackSpeed,
            HealthValue,
            HealthRegenerationSpeed,
            CriticalDamageChance
        }

        private static Dictionary<Types, Stat> statsDict;

        public static void Init(StartStats[] StartStats)
        {
            bool didLoad = false;

            if (!didLoad)
                loadBasicStats(StartStats);
            else
                loadStatsFromFile(null);
        }

        private static void loadBasicStats(StartStats[] StartStats)
        {
            statsDict = new Dictionary<Types, Stat>();
            foreach (StartStats stat in StartStats)
                if (!statsDict.ContainsKey(stat.Type))
                    statsDict.Add(stat.Type, new Stat(stat.StartValue, 1, stat.StartCost, stat.UpgradeCost, stat.StartValue));
        }

        private static void loadStatsFromFile(StatSave save)
        {
            statsDict = new Dictionary<Types, Stat>();
            foreach (Stat stat in save.StatsArray)
                if (!statsDict.ContainsKey(stat.Type))
                    statsDict.Add(stat.Type, stat);
        }

        public static Stat GetStat(Types type)
        {
            if (statsDict.ContainsKey(type))
                return statsDict[type];
            return null;
        }
    }

    public class Stat
    {
        public Stats.Types Type;
        public int Value => _value;
        public int Level => _level;
        public int UpgradeCost => getUpgradeCost();
        public int StartValue => _startValue;

        [SerializeField]
        private int _value;
        [SerializeField]
        private int _startValue;
        [SerializeField]
        private int _level;
        [SerializeField]
        private int _basicCost;
        [SerializeField]
        private int _upgradeCost;

        public UnityEvent OnLevelChange = new UnityEvent();

        public void UpgradeLevel(int byHowMuch)
        {
            _level++;
            _value = Mathf.CeilToInt(Mathf.Pow(_level, 1.5f));

            OnLevelChange?.Invoke();
        }

        private int getUpgradeCost()
        {
            return _basicCost + Mathf.CeilToInt(_upgradeCost * Mathf.Pow(_level, 1.2f));
        }

        public Stat(int val, int level, int startCost, int upgradeCost, int startValue)
        {
            _value = val;
            _level = level;
            _basicCost = startCost;
            _upgradeCost = upgradeCost;
            _startValue = startValue;
        }
    }

    [System.Serializable]
    public class StatSave
    {
        [SerializeField]
        public Stat[] StatsArray;

        public StatSave(Stat[] save)
        {
            StatsArray = save;
        }
    }
}