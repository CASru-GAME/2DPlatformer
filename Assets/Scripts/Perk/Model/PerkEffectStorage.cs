using System;
using System.Collections.Generic;
using UnityEngine;
using Perk.Data;

namespace Perk.Model
{
    public static class PerkEffectStorage
    {
        private static readonly List<(int id, PerkEffect perkEffect)> enabledPerkList = new();
        public static IReadOnlyList<(int id, PerkEffect perkEffect)> EnabledPerkList => enabledPerkList;
        private static readonly Dictionary<int, Func<PerkEffect>> perkDictionary = new();
        private static readonly List<(int id, PerkEffect perkEffect)> usePerkList = new();
        public static IReadOnlyList<(int id, PerkEffect perkEffect)> UsePerkList => usePerkList;
        private static readonly List<int> perkIDList = new();

        public static void RegisterPerk(int id, Func<PerkEffect> perkEffectFactory)
        {
            if (!perkDictionary.ContainsKey(id) && id > 0)
            {
                perkDictionary.Add(id, perkEffectFactory);
                perkIDList.Add(id);
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

        public static void AddAllPerk()
        {
            foreach (var perk in perkDictionary)
            {
                if(perk.Key == 0 || perk.Key == 22) continue;
                AddPerk(perk.Key);
            }
        }

        public static void AddUsePerk(int id)
        {
            for(int i = 0; i < usePerkList.Count; i++)
                if(usePerkList[i].id == id) return;
            
            usePerkList.Add((id, enabledPerkList.Find(perk => perk.id == id).perkEffect));
        }

        public static void RemovePerkAtRandom(int exceptionID)
        {
            if (enabledPerkList.Count <= 1)
            {
                Debug.LogWarning("No perks to remove.");
                return;
            }

            List<int> removableIDList = new();
            for(int i = 0; i < enabledPerkList.Count; i++)
                if(enabledPerkList[i].id != exceptionID)
                    removableIDList.Add(enabledPerkList[i].id);

            int randomIndex = UnityEngine.Random.Range(0, removableIDList.Count);
            int removedID = removableIDList[randomIndex];

            for(int i = 0; i < enabledPerkList.Count; i++)
                if(enabledPerkList[i].id == removedID)
                {
                    enabledPerkList[i].perkEffect.Remove();
                    if(enabledPerkList[i].perkEffect.Stack == 0)
                        enabledPerkList.RemoveAt(i);
                    return;
                }
        }

        public static int GetPerkIDAtRandom()
        {
            
            int randomIndex = UnityEngine.Random.Range(0, perkIDList.Count);
            return perkIDList[randomIndex];
        }

        public static void ResetAllPerks()
        {
            enabledPerkList.Clear();
            usePerkList.Clear();
        }
    }
}