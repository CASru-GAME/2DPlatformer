using System;
using System.Collections.Generic;
using UnityEngine;
using Perk.Data;

namespace Perk.Model
{
    public static class PerkEffectStorage
    {
        private static readonly List<PerkEffect> enabledPerkList = new();
        private static readonly Dictionary<int, Func<PerkEffect>> perkDictionary = new();

        public static void RegisterPerk(int id, Func<PerkEffect> perkEffectFactory)
        {
            if (!perkDictionary.ContainsKey(id))
            {
                perkDictionary.Add(id, perkEffectFactory);
            }
            else
            {
                Debug.LogWarning($"Perk with ID {id} is already registered.");
            }
        }

        public static void AddPerk(int id)
        {
            if (perkDictionary.TryGetValue(id, out Func<PerkEffect> perkEffectFactory))
            {
                PerkEffect perkEffect = perkEffectFactory();
                enabledPerkList.Add(perkEffect);
                perkEffect.Add();
            }
            else
            {
                Debug.LogWarning($"Perk with ID {id} not found.");
            }
        }

        public static void RemovePerkAtRandom()
        {
            if (enabledPerkList.Count == 0)
            {
                Debug.LogWarning("No perks to remove.");
                return;
            }

            int randomIndex = UnityEngine.Random.Range(0, enabledPerkList.Count);
            enabledPerkList[randomIndex].Remove();
            enabledPerkList.RemoveAt(randomIndex);
        }
    }
}