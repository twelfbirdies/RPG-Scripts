using System;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass,Dictionary<Stat, float[]>> lookupTable = null;
        public float GetStat(Stat stat,CharacterClass characterClass, int level) 
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length < level) 
            {
                return 0;
            }
            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass) 
        {
            BuildLookup();

            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass,Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionClass in characterClasses) 
            {
                var statLookupTable = new Dictionary<Stat, float[]>();
                foreach (ProgressionStats progressionStats in progressionClass.stats) 
                {
                    statLookupTable[progressionStats.stat] = progressionStats.levels;
                }
                lookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass 
        {
            public CharacterClass characterClass;
            public ProgressionStats[] stats;
        }

        [System.Serializable]
        class ProgressionStats 
        {
            public Stat stat;
            public float[] levels; 
        }
    }
}

