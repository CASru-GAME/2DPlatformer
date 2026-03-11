using UnityEngine;

namespace Perk.Data
{
    [CreateAssetMenu(fileName = "PerkViewData", menuName = "ScriptableObjects/PerkViewData", order = 1)]
    public class PerkViewData : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private Sprite sprite;
        [SerializeField] private string effectDescription;
        [SerializeField] private string stageDescription;
        [SerializeField] private PerkTextCommonData perkTextCommonData;

        public string Description => perkTextCommonData.GetPerkDescription(effectDescription, stageDescription);
    }
}
