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
        private readonly List<PerkIconView> currentPerkIconViewList = new();
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
            lastIconPosition = new Vector2(canvas.GetComponent<RectTransform>().rect.width / 2 - perkIconPrefab.rectTransform.sizeDelta.x / 2 - 10, canvas.GetComponent<RectTransform>().rect.height / 2 + perkIconPrefab.rectTransform.sizeDelta.y / 2);
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
                if (i < currentPerkIconViewList.Count)
                {
                    int id = PerkEffectStorage.EnabledPerkList[i].id;
                    currentPerkIconViewList[i].UpdateIcon(id, PerkEffectStorage.EnabledPerkList[i].perkEffect.Stack);
                }
                else
                {
                    Image newIcon = Instantiate(perkIconPrefab);
                    newIcon.rectTransform.SetParent(canvas.transform, false);
                    newIcon.rectTransform.anchoredPosition = lastIconPosition + new Vector2(0, -newIcon.rectTransform.sizeDelta.y - 5);
                    if(newIcon.rectTransform.anchoredPosition.y < -canvas.GetComponent<RectTransform>().rect.height / 2)
                    {
                        newIcon.rectTransform.anchoredPosition = new Vector2(lastIconPosition.x - newIcon.rectTransform.sizeDelta.x - 5, canvas.GetComponent<RectTransform>().rect.height / 2 - newIcon.rectTransform.sizeDelta.y / 2 - 5);
                    }
                    
                    var newIconView = newIcon.GetComponent<PerkIconView>();
                    currentPerkIconViewList.Add(newIconView);
                    lastIconPosition = newIcon.rectTransform.anchoredPosition;
                }
            }

            for(int i = PerkEffectStorage.EnabledPerkList.Count; i < currentPerkIconViewList.Count; i++)
            {
                Destroy(currentPerkIconViewList[i].gameObject);
            }

            if(currentPerkIconViewList.Count > 0)
                lastIconPosition = currentPerkIconViewList[^1].RectTransform.anchoredPosition;
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