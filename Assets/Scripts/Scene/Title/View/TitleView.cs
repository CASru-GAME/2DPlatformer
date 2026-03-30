using UnityEngine;

namespace Scene.View
{
    public class TitleView : MonoBehaviour
    {
        [SerializeField] private Canvas defaultCanvas;
        [SerializeField] private Canvas settingCanvas;
        
        public void OpenDefault()
        {
            defaultCanvas.enabled = true;
            settingCanvas.enabled = false;
        }

        public void OpenSetting()
        {
            settingCanvas.enabled = true;
        }
    }
}