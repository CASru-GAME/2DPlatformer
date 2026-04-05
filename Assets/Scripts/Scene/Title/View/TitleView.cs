using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField] private Canvas defaultCanvas;
        [SerializeField] private Canvas settingCanvas;
        [SerializeField] private Canvas initialCanvas;
        [SerializeField] private Image titleImage;
        [SerializeField] private SpriteRenderer titleDummySpriteRenderer;

        public void CloseInitial()
        {
            initialCanvas.enabled = false;
        }
        
        public void OpenDefault()
        {
            defaultCanvas.enabled = true;
            settingCanvas.enabled = false;
        }

        public void OpenSetting()
        {
            settingCanvas.enabled = true;
        }

        private void Update()
        {
            titleImage.sprite = titleDummySpriteRenderer.sprite;
        }
    }
}