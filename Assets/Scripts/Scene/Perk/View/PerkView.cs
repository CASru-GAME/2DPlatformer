using System.Collections.Generic;
using Scene.Data;
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
        [SerializeField] private PerkSelectViewDataTable perkSelectViewDataTable;
        [SerializeField] private Image startImage;
        private Vector2 lastStageImagePosition;
        [SerializeField] private Image stageImagePrefab;
        private Vector2 startPosition;
        private Vector2 goalPosition;
        private readonly List<Image> selectedStageImageList = new();
        [SerializeField] private List<Image> stageImageList = new();
        [SerializeField] private List<Image> perkImageList = new();
        private int lastDirectionID = 1;

        private void Start()
        {
            startPosition = startImage.rectTransform.anchoredPosition;
            lastStageImagePosition = startPosition;
            selectedStageImageList.Add(startImage);
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

        public void SetPerkSelectView(int boxNumber, int perkID, int stageID, int gimmickID, int directionID)
        {
            perkTextList[boxNumber].text = perkSelectViewDataTable.GetDescription(perkID, gimmickID, directionID);
            stageImageList[boxNumber].sprite = perkSelectViewDataTable.GetStageSprite(stageID);
            perkImageList[boxNumber].sprite = perkSelectViewDataTable.GetPerkSprite(perkID);
        }

        public void OpenIdle()
        {
            SetGoalImage();
            initialCanvas.enabled = false;
            perkCanvas.enabled = false;
            idleCanvas.enabled = true;
        }

        public void SetStageImage(int newDirectionID, int stageID)
        {
            if (lastDirectionID == 3 && newDirectionID == 1)
                SetStageImageWithSprite(lastDirectionID, perkSelectViewDataTable.GetConnectStageSprite(1));
            else if (lastDirectionID == 1 && newDirectionID == 3)
                SetStageImageWithSprite(lastDirectionID, perkSelectViewDataTable.GetConnectStageSprite(2));
            else if (lastDirectionID == 1 && newDirectionID == 2)
                SetStageImageWithSprite(lastDirectionID, perkSelectViewDataTable.GetConnectStageSprite(3));
            else if (lastDirectionID == 2 && newDirectionID == 1)
                SetStageImageWithSprite(lastDirectionID, perkSelectViewDataTable.GetConnectStageSprite(4));

            SetStageImageWithSprite(newDirectionID, perkSelectViewDataTable.GetStageSprite(stageID));
        }

        private void SetStageImageWithSprite(int newDirectionID, Sprite stageSprite)
        {
            Vector2 direction = new();
            switch (newDirectionID)
            {
                case 1:
                    direction = new Vector2(1, 0);
                    break;
                case 2:
                    direction = new Vector2(0, 1);
                    break;
                case 3:
                    direction = new Vector2(0, -1);
                    break;
            }
            Vector2 newStageImagePosition = lastStageImagePosition + direction * startImage.rectTransform.sizeDelta;
            Image newStageImage = Instantiate(stageImagePrefab);
            newStageImage.sprite = stageSprite;
            newStageImage.transform.SetParent(startImage.transform.parent, false);
            newStageImage.rectTransform.anchoredPosition = newStageImagePosition;
            lastStageImagePosition = newStageImagePosition;
            lastDirectionID = newDirectionID;
            selectedStageImageList.Add(newStageImage);
        }

        private void SetGoalImage()
        {
            SetStageImage(1, 20);

            Vector2 diff = -0.5f * (lastStageImagePosition + startPosition);
            for (int i = 0; i < selectedStageImageList.Count; i++)
            {
                Image stageImage = selectedStageImageList[i];
                Vector2 targetPosition = stageImage.rectTransform.anchoredPosition + diff;
                stageImage.rectTransform.anchoredPosition = targetPosition;
            }
        }
    }
}