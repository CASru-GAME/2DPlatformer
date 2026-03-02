using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class PerkView : MonoBehaviour
    {
        [SerializeField] private Canvas initialCanvas;
        [SerializeField] private Canvas perkCanvas;
        [SerializeField] private Canvas idleCanvas;
        [SerializeField] private Text perkCountText;
        
        public void OpenInitial()
        {
            initialCanvas.enabled = true;
            perkCanvas.enabled = false;
            idleCanvas.enabled = false;
        }

        public void OpenPerk(int currentPerkCount)
        {
            initialCanvas.enabled = false;
            perkCanvas.enabled = true;
            idleCanvas.enabled = false;
            perkCountText.text = currentPerkCount.ToString();
        }

        public void OpenIdle()
        {
            initialCanvas.enabled = false;
            perkCanvas.enabled = false;
            idleCanvas.enabled = true;
        }
    }
}