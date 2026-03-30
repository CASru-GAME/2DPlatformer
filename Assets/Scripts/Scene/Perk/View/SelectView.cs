using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    [RequireComponent(typeof(Button))]
    public class SelectView : MonoBehaviour
    {
        private static WaitForSeconds _waitForSeconds0_1 = new WaitForSeconds(0.1f);
        [SerializeField] private float rerollTime = 0.5f;
        private Button button;
        [SerializeField] private float diffY = 20f;
        [SerializeField] private List<Image> imageList = new();
        [SerializeField] private Text text;
        private bool isSet = false;
        private string newDescription;
        private Vector2 defaultPosition;
        [SerializeField] private float noiseInterval = 2f;
        private Coroutine noiseCoroutine;

        private void Start()
        {
            button = GetComponent<Button>();
            defaultPosition = transform.localPosition;
        }

        public void Reroll(float delay)
        {
            StartCoroutine(RerollCoroutine(delay));
        }

        private IEnumerator RerollCoroutine(float delay)
        {
            StopCoroutine(noiseCoroutine);
            isSet = false;
            button.interactable = false;
            foreach (Image image in imageList)
                image.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            StartCoroutine(RandomTextCoroutine());
            yield return new WaitForSeconds(delay);
            button.transform.localPosition += new Vector3(0, -diffY * 0.9f, 0);
            yield return new WaitForSeconds(rerollTime * 0.1f);
            button.transform.localPosition += new Vector3(0, -diffY * 0.1f, 0);
        }

        public void StopReroll(string description)
        {
            StartCoroutine(StopRerollCoroutine(description));
        }

        private IEnumerator StopRerollCoroutine(string description)
        {
            isSet = true;
            newDescription = description;
            foreach (Image image in imageList)
                image.color = new Color(1f, 1f, 1f, 1f);
            button.transform.localPosition += new Vector3(0, diffY * 1.5f, 0);
            yield return new WaitForSeconds(rerollTime * 0.1f);
            button.transform.localPosition += new Vector3(0, diffY * 0.1f, 0);
            yield return new WaitForSeconds(rerollTime * 0.2f);
            button.transform.localPosition += new Vector3(0, -diffY * 0.6f, 0);
            button.interactable = true;
            noiseCoroutine = StartCoroutine(RandomNoiseCoroutine());
        }

        private IEnumerator RandomTextCoroutine()
        {
            while (!isSet)
            {
                string newText = "";
                for (int i = 0; i < 40; i++)
                {
                    int r = UnityEngine.Random.Range(0, 0xFFFF);
                    newText += ((char)r).ToString();
                }
                text.text = newText;
                text.color = new Color(1f, 1f, 1f, 0.25f);
                yield return _waitForSeconds0_1;
            }                    
            text.text = newDescription;
            text.color = new Color(1f, 1f, 1f, 1f);
        }

        private IEnumerator RandomNoiseCoroutine()
        {
            yield return new WaitForSeconds(noiseInterval);
            while (isSet)
            {
                if(transform.localPosition.x > defaultPosition.x)
                    transform.localPosition = (Vector2)transform.localPosition + new Vector2(UnityEngine.Random.Range(-3, 0), 0);
                else
                    transform.localPosition = (Vector2)transform.localPosition + new Vector2(UnityEngine.Random.Range(0, 3), 0);
                yield return new WaitForSeconds(0.2f);
                if(transform.localPosition.y > defaultPosition.y)
                    transform.localPosition = (Vector2)transform.localPosition + new Vector2(0, UnityEngine.Random.Range(-3, 0));
                else
                    transform.localPosition = (Vector2)transform.localPosition + new Vector2(0, UnityEngine.Random.Range(0, 3));
                yield return new WaitForSeconds(noiseInterval);
            }           
        }
    }
}