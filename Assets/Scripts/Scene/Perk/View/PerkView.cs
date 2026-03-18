using System.Collections.Generic;
using Perk.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class PerkView : MonoBehaviour
    {
        [SerializeField] private Canvas initialCanvas;
        [SerializeField] private Canvas perkCanvas;
        [SerializeField] private Canvas idleCanvas;
        [SerializeField] private Canvas stageCanvas;
        [SerializeField] private Text perkCountText;
        [SerializeField] private List<Text> perkTextList;
        [SerializeField] private PerkViewDataTable perkViewDataTable;
        [SerializeField] private Image startImage;
        private Vector2 lastStageImagePosition;
        [SerializeField] private Image stageImagePrefab;
        private Vector2 startPosition;
        private Vector2 goalPosition;
        private readonly List<Image> stageImageList = new();

        private void Start()
        {
            startPosition = startImage.rectTransform.anchoredPosition;
            lastStageImagePosition = startPosition;
            stageImageList.Add(startImage);
        }
        
        public void OpenInitial()
        {
            initialCanvas.enabled = true;
            perkCanvas.enabled = false;
            idleCanvas.enabled = false;
            stageCanvas.enabled = false;
        }

        public void OpenPerk(int currentPerkCount)
        {
            initialCanvas.enabled = false;
            perkCanvas.enabled = true;
            idleCanvas.enabled = false;
            perkCountText.text = "ステージを選択してください(" + currentPerkCount.ToString() + " / 8)";
            stageCanvas.enabled = true;
        }

        public void SetPerkText(int boxNumber, int perkID, int stageID, int gimmickID, int directionID)
        {
            perkTextList[boxNumber].text = perkViewDataTable.GetPerkDescription(perkID) + "\n//以下デバッグ用//\nS:" + stageID 
                + " G:" + gimmickID + " D:" + directionID;
        }

        public void OpenIdle()
        {
            SetGoalImage();
            initialCanvas.enabled = false;
            perkCanvas.enabled = false;
            idleCanvas.enabled = true;
        }

        public void SetStageImage()
        {
            Vector2 direction = new(1, 0); //テスト用
            Vector2 newStageImagePosition = lastStageImagePosition + direction * startImage.rectTransform.sizeDelta * 1.1f;
            Image newStageImage = Instantiate(stageImagePrefab);
            newStageImage.transform.SetParent(startImage.transform.parent, false);
            newStageImage.rectTransform.anchoredPosition = newStageImagePosition;
            lastStageImagePosition = newStageImagePosition;
            stageImageList.Add(newStageImage);
        }

        private void SetGoalImage()
        {
            Vector2 direction = new(1, 0); //テスト用
            Vector2 newStageImagePosition = lastStageImagePosition + direction * startImage.rectTransform.sizeDelta * 1.1f;
            Image newStageImage = Instantiate(stageImagePrefab);
            newStageImage.transform.SetParent(startImage.transform.parent, false);
            newStageImage.rectTransform.anchoredPosition = newStageImagePosition;
            lastStageImagePosition = newStageImagePosition;
            goalPosition = newStageImagePosition;
            stageImageList.Add(newStageImage);

            Vector2 diff = -0.5f * (goalPosition + startPosition);

            for (int i = 0; i < stageImageList.Count; i++)
            {
                Image stageImage = stageImageList[i];
                Vector2 targetPosition = stageImage.rectTransform.anchoredPosition + diff;
                stageImage.rectTransform.anchoredPosition = targetPosition;
            }
        }
    }
}