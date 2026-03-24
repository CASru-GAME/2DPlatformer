using System.Collections.Generic;
using UnityEngine;

namespace Scene.Data
{
    [CreateAssetMenu(fileName = "PerkSelectViewDataTable", menuName = "ScriptableObjects/PerkSelectViewDataTable", order = 1)]
    public class PerkSelectViewDataTable : ScriptableObject
    {
        [SerializeField] private List<PerkViewData> perkViewDataList;
        [SerializeField] private List<Sprite> stageSpriteList;
        [SerializeField] private List<Sprite> connectStageSpriteList;
        [SerializeField] private List<GimmickViewData> gimmickViewDataList;
        private readonly Dictionary<int, string> directionDictionary = new()
        {
            { 1, "右" },
            { 2, "上" },
            { 3, "下" }
        };
        [SerializeField] private Color perkEffectColor;
        [SerializeField] private Color gimmickColor;
        [SerializeField] private Color directionColor;

        public string GetDescription(int perkID, int gimmickID, int directionID)
        {
            string perkDescription = GetPerkDescription(perkID);
            string gimmickDescription = GetGimmickDescription(gimmickID);
            string directionDescription = directionDictionary.GetValueOrDefault(directionID, "エラー");
            return $"<color=#{ColorUtility.ToHtmlStringRGB(perkEffectColor)}>{perkDescription}</color>\n<color=#{ColorUtility.ToHtmlStringRGB(gimmickColor)}>{gimmickDescription}</color>\n<color=#{ColorUtility.ToHtmlStringRGB(directionColor)}>設置方向ー{directionDescription}</color>";
        }

        public Sprite GetStageSprite(int stageID)
        {
            return stageSpriteList[stageID];
        }

        public Sprite GetConnectStageSprite(int stageID)
        {
            return connectStageSpriteList[stageID];
        }

        public Sprite GetPerkSprite(int perkID)
        {
            foreach (PerkViewData data in perkViewDataList)
                if (data.ID == perkID)
                    return data.Sprite;
            return GetPerkSprite(0);
        }

        private string GetPerkDescription(int perkID)
        {
            foreach (PerkViewData data in perkViewDataList)
                if (data.ID == perkID)
                    return data.Description;
            return GetPerkDescription(0);
        }

        private string GetGimmickDescription(int gimmickID)
        {
            foreach (GimmickViewData data in gimmickViewDataList)
                if (data.ID == gimmickID)
                    return data.Description;
            return GetGimmickDescription(0);
        }
    }
}
