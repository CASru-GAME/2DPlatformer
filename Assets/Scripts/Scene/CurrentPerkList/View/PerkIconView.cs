using Scene.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class PerkIconView : MonoBehaviour
    {
        [SerializeField] private Text stackText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Image descriptionImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private PerkSelectViewDataTable perkSelectViewDataTable;
        public RectTransform RectTransform => iconImage.rectTransform;

        private void Awake()
        {
            descriptionImage.enabled = false;
            descriptionText.enabled = false;
        }

        public void UpdateIcon(int id, int stack)
        {
            descriptionText.text = perkSelectViewDataTable.GetPerkColoredDescription(id);
            iconImage.sprite = perkSelectViewDataTable.GetPerkSprite(id);
            stackText.text = "x" + stack.ToString();
        }

        public void ShowDescription()
        {
            descriptionImage.enabled = true;
            descriptionText.enabled = true;
            Debug.Log("111");
        }

        public void HideDescription()
        {
            descriptionImage.enabled = false;
            descriptionText.enabled = false;
        }
    }
}