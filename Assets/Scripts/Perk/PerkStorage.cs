using System;
using System.Collections.Generic;
using UnityEngine;

public static class PerkStorage
{
    private static readonly List<Perk> enabledPerkList = new();
    private static readonly Dictionary<int, Func<Perk>> perkDictionary = new();

    public static void RegisterPerk(int id, Func<Perk> perkFactory)
    {
        if (!perkDictionary.ContainsKey(id))
        {
            perkDictionary.Add(id, perkFactory);
        }
        else
        {
            Debug.LogWarning($"Perk with ID {id} is already registered.");
        }
    }

    public static void EnablePerk(int id)
    {
        if (perkDictionary.TryGetValue(id, out Func<Perk> perkFactory))
        {
            Perk perk = perkFactory();
            enabledPerkList.Add(perk);
            perk.OnEnable();
        }
        else
        {
            Debug.LogWarning($"Perk with ID {id} not found.");
        }
    }
}
