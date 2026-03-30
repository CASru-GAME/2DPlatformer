using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Canvas clearBackCanvas;
        [SerializeField] private Canvas clearCanvas;
        [SerializeField] private Canvas overBackCanvas;
        [SerializeField] private Canvas overCanvas;
        
        private void Awake()
        {
            clearBackCanvas.enabled = false;
            clearCanvas.enabled = false;
            overBackCanvas.enabled = false;
            overCanvas.enabled = false;
        }

        public void OpenClear()
        {
            StartCoroutine(OpenCanvasCoroutine(clearBackCanvas, clearCanvas));
        }

        public void OpenOver()
        {
            StartCoroutine(OpenCanvasCoroutine(overBackCanvas, overCanvas));
        }

        private static IEnumerator OpenCanvasCoroutine(Canvas back, Canvas front)
        {
            back.enabled = true;
            CanvasGroup group = back.GetComponent<CanvasGroup>();
            for (float i = 0; i <= 1; i += Time.unscaledDeltaTime * 2)
            {
                Time.timeScale = 1 - i;
                group.alpha = i;
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(0.3f);
            front.enabled = true;
        }
    }
}