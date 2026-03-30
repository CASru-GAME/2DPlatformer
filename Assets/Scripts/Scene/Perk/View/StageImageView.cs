using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class StageImageView : MonoBehaviour
    {
        [SerializeField] Image image;
        private bool isSetting = false;

        public void Set()
        {
            StartCoroutine(SetCoroutine());
        }

        private IEnumerator SetCoroutine()
        {
            isSetting = true;
            yield return null;
            Vector2 initialPosition = image.rectTransform.anchoredPosition;
            Vector2 r = Random.insideUnitCircle * 10f;
            image.rectTransform.anchoredPosition = initialPosition + r;
            yield return new WaitForSeconds(0.1f);
            image.rectTransform.anchoredPosition = initialPosition;
            isSetting = false;
        }

        public void Shake()
        {
            if (isSetting) return;
            StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            Vector2 initialPosition = image.rectTransform.anchoredPosition;
            image.rectTransform.anchoredPosition = initialPosition + new Vector2(0f, -4f);
            yield return new WaitForSeconds(0.1f);
            image.rectTransform.anchoredPosition = initialPosition;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
