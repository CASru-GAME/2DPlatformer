using System;
using System.Collections.Generic;
using UnityEngine;
using Perk.Data;

namespace Perk.Model
{
    public static class PerkEffectStorage
    {
        private static readonly List<(int id, PerkEffect perkEffect)> enabledPerkList = new();
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
                for(int i = 0; i < enabledPerkList.Count; i++)
                    if(enabledPerkList[i].id == id)
                    {
                        enabledPerkList[i].perkEffect.Add();
                        return;
                    }
                PerkEffect perkEffect = perkEffectFactory();
                enabledPerkList.Add((id, perkEffect));
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
            for(int i = 0; i < enabledPerkList.Count; i++)
                if(enabledPerkList[i].id == enabledPerkList[randomIndex].id)
                {
                    enabledPerkList[i].perkEffect.Remove();
                    if(enabledPerkList[i].perkEffect.Stack == 0)
                        enabledPerkList.RemoveAt(i);
                    return;
                }
        }

        public static int GetPerkIDAtRandom()
        {
            List<int> keys = new(perkDictionary.Keys);
            Debug.Log(keys.Count);
            int randomIndex = UnityEngine.Random.Range(1, keys.Count);
            return keys[randomIndex];
        }
    }
}