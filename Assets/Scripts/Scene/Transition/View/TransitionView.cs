using UnityEngine;
using UnityEngine.UI;

namespace Scene.View
{
    public class TransitionView : MonoBehaviour
    {
        private static TransitionView instance;
        public static TransitionView Instance => instance;
        [SerializeField] private SpriteRenderer dummySprite;
        [SerializeField] private Image image;
        [SerializeField] private Animator animator;
        [SerializeField] private float transitionDuration = 0.6f;
        public float TransitionHalfDuration => transitionDuration * 0.5f;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            PlayAnim("Open_1");
        }

        private void Update()
        {
            image.sprite = dummySprite.sprite;
        }

        public void PlayAnim(string animName)
        {
            animator.speed = 1f / transitionDuration;
            animator.Play(animName);
        }
    }
}