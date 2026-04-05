using System;
using System.Collections;
using System.Collections.Generic;
using Scene.Controller;
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
        [SerializeField] private Image lifeImagePrefab;
        [SerializeField] private Canvas lifeCanvas;
        private readonly List<Image> lifeImageList = new();
        private Vector2 initialLifeImagePosition;
        [SerializeField] private Image initialLifeImage;
        public static Action<int, int> Update;
        
        private void Awake()
        {
            clearBackCanvas.enabled = false;
            clearCanvas.enabled = false;
            overBackCanvas.enabled = false;
            overCanvas.enabled = false;
            initialLifeImagePosition = initialLifeImage.rectTransform.anchoredPosition;
            Destroy(initialLifeImage.gameObject);
            Update += SetLifeImage;
        }

        public void OpenClear()
        {
            SoundSourceObject.Instance.PlayClearSE();
            Update -= SetLifeImage;
            StartCoroutine(OpenCanvasCoroutine(clearBackCanvas, clearCanvas));
        }

        public void OpenOver()
        {
            Update -= SetLifeImage;
            StartCoroutine(OpenCanvasCoroutine(overBackCanvas, overCanvas));
        }

        private IEnumerator OpenCanvasCoroutine(Canvas back, Canvas front)
        {
            SoundSourceObject.Instance.ActivateLowPassFilter(true);
            back.enabled = true;
            CanvasGroup group = back.GetComponent<CanvasGroup>();
            Time.timeScale = 0.2f;
            for (float i = 0; i <= 1; i += Time.unscaledDeltaTime * 1f)
            {
                group.alpha = i;
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(0.3f);
            front.enabled = true;
        }

        private void SetLifeImage(int maxLife, int currentLife)    
        {
            if (lifeImageList.Count < maxLife)
            {
                for (int i = lifeImageList.Count; i < maxLife; i++)
                {
                    Image newImage = Instantiate(lifeImagePrefab);
                    newImage.transform.SetParent(lifeCanvas.transform, false);
                    newImage.rectTransform.anchoredPosition = initialLifeImagePosition + new Vector2(i * (newImage.rectTransform.rect.width + 5), 0);
                    lifeImageList.Add(newImage);
                }
            }
            else if (lifeImageList.Count > maxLife)
            {
                for (int i = lifeImageList.Count - 1; i >= maxLife; i--)
                {
                    Destroy(lifeImageList[i].gameObject);
                    lifeImageList.RemoveAt(i);
                }
            }

            for (int i = 0; i < lifeImageList.Count; i++)
            {
                if (i < currentLife)
                    lifeImageList[i].color = Color.white;
                else
                    lifeImageList[i].color = new Color(1, 1, 1, 0.3f);
            }
        }
    }
}