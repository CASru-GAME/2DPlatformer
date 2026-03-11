using UnityEngine;

namespace Perk.Data
{
    [CreateAssetMenu(fileName = "PerkTextCommonData", menuName = "ScriptableObjects/PerkTextCommonData", order = 1)]
    public class PerkTextCommonData : ScriptableObject
    {
        [SerializeField] private Color perkEffectColor;
        [SerializeField] private Color stageColor;
        
        public string GetPerkDescription(string effectDescription, string stageDescription)
        {
            return $"{GetEffectDescription(effectDescription)}\n{GetStageDescription(stageDescription)}";
        }

        private string GetEffectDescription(string description)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(perkEffectColor)}>{description}</color>";
        }

        private string GetStageDescription(string description)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(stageColor)}>{description}</color>";
        }

    }
}
