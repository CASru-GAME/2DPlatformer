using System.Collections.Generic;
using UnityEngine;

namespace Perk.Data
{
    [CreateAssetMenu(fileName = "PerkViewDataTable", menuName = "ScriptableObjects/PerkViewDataTable", order = 1)]
    public class PerkViewDataTable : ScriptableObject
    {
        [SerializeField] private List<PerkViewData> perkViewDataList;
        public string GetPerkDescription(int perkID)
        {
            foreach (PerkViewData data in perkViewDataList)
                if (data.ID == perkID)
                    return data.Description;
            return GetPerkDescription(0);
        }
    }
}
