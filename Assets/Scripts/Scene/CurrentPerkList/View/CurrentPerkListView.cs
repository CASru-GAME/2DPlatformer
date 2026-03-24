using System.Collections.Generic;
using Perk.Model;
using Scene.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class CurrentPerkListView : MonoBehaviour
    {
        [SerializeField] private Image perkIconPrefab;
        private readonly List<Image> currentPerkIconList = new();
        private readonly List<Text> currentPerkTextList = new();
        [SerializeField] private PerkSelectViewDataTable perkSelectViewDataTable;
        [SerializeField] private Canvas canvas;
        private Vector2 lastIconPosition;
        [SerializeField] private Image usePerkImagePrefab;
        private readonly List<Image> currentUsePerkImageList = new();
        private readonly List<Image> currentUsePerkIconList = new();
        private readonly List<Text> currentUsePerkStackTextList = new();
        private readonly List<Text> currentUsePerkKeyTextList = new();
        private Vector2 lastUseImagePosition;
        
        private void Start()
        {
            lastIconPosition = new Vector2(-canvas.GetComponent<RectTransform>().rect.width / 2 - perkIconPrefab.rectTransform.sizeDelta.x / 2 + 10, canvas.GetComponent<RectTransform>().rect.height / 2 - perkIconPrefab.rectTransform.sizeDelta.y / 2 - 10);
            lastUseImagePosition = new Vector2(-canvas.GetComponent<RectTransform>().rect.width / 2 - usePerkImagePrefab.rectTransform.sizeDelta.x / 2 + 60, -canvas.GetComponent<RectTransform>().rect.height / 2 + usePerkImagePrefab.rectTransform.sizeDelta.y / 2 - 40);
        }

        private void Update()
        {
            UpdatePerkIcon();
            UpdateUsePerkIcon();
        }

        private void UpdatePerkIcon()
        {
            for(int i = 0; i < PerkEffectStorage.EnabledPerkList.Count; i++)
            {
                if (i < currentPerkIconList.Count)
                {
                    int id = PerkEffectStorage.EnabledPerkList[i].id;
                    currentPerkIconList[i].sprite = perkSelectViewDataTable.GetPerkSprite(id);
                    currentPerkTextList[i].text = "x" + PerkEffectStorage.EnabledPerkList[i].perkEffect.Stack.ToString();
                }
                else
                {
                    Image newIcon = Instantiate(perkIconPrefab);
                    newIcon.sprite = perkSelectViewDataTable.GetPerkSprite(PerkEffectStorage.EnabledPerkList[i].id);
                    newIcon.rectTransform.SetParent(canvas.transform, false);
                    newIcon.rectTransform.anchoredPosition = lastIconPosition + new Vector2(newIcon.rectTransform.sizeDelta.x + 5, 0);
                    if(newIcon.rectTransform.anchoredPosition.x > canvas.GetComponent<RectTransform>().rect.width / 2)
                    {
                        newIcon.rectTransform.anchoredPosition = new Vector2(-canvas.GetComponent<RectTransform>().rect.width / 2 + newIcon.rectTransform.sizeDelta.x / 2 + 15, lastIconPosition.y - newIcon.rectTransform.sizeDelta.y - 5);
                    }
                    
                    
                    currentPerkIconList.Add(newIcon);
                    currentPerkTextList.Add(newIcon.GetComponentInChildren<Text>());
                    lastIconPosition = newIcon.rectTransform.anchoredPosition;
                }
            }

            for(int i = PerkEffectStorage.EnabledPerkList.Count; i < currentPerkIconList.Count; i++)
            {
                Destroy(currentPerkIconList[i].gameObject);
            }

            if(currentPerkIconList.Count > 0)
                lastIconPosition = currentPerkIconList[^1].rectTransform.anchoredPosition;
        }

        private void UpdateUsePerkIcon()
        {
            for(int i = 0; i < PerkEffectStorage.UsePerkList.Count; i++)
            {
                if (i < currentUsePerkImageList.Count)
                {
                    int id = PerkEffectStorage.UsePerkList[i].id;
                    currentUsePerkStackTextList[i].text = "残り使用回数\n<size=150>" + PerkEffectStorage.UsePerkList[i].perkEffect.UseStack.ToString() + "</size>";
                    currentUsePerkKeyTextList[i].text = "使用キー\n<size=150>" + (i+1).ToString() + "</size>";
                }
                else
                {
                    Image newImage = Instantiate(usePerkImagePrefab);
                    Image iconImage = newImage.transform.GetChild(2).GetComponent<Image>();
                    iconImage.sprite = perkSelectViewDataTable.GetPerkSprite(PerkEffectStorage.UsePerkList[i].id);
                    newImage.rectTransform.SetParent(canvas.transform, false);
                    newImage.rectTransform.anchoredPosition = lastUseImagePosition + new Vector2(newImage.rectTransform.sizeDelta.x + 5, 0);                    
                    
                    currentUsePerkImageList.Add(newImage);
                    currentUsePerkIconList.Add(iconImage);
                    currentUsePerkStackTextList.Add(newImage.GetComponentInChildren<Text>());
                    currentUsePerkKeyTextList.Add(newImage.GetComponentsInChildren<Text>()[1]);
                    lastUseImagePosition = newImage.rectTransform.anchoredPosition;
                }
            }

            if(currentUsePerkImageList.Count > 0)
                lastUseImagePosition = currentUsePerkImageList[^1].rectTransform.anchoredPosition;
        }
    }
}