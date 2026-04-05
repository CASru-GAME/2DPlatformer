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
        [SerializeField] private Image initialImage;

        private void Awake()
        {
            instance = this;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        private void Update()
        {
            image.sprite = dummySprite.sprite;
        }

        public void PlayAnim(string animName)
        {
            if(initialImage.enabled)
                initialImage.enabled = false;
            animator.speed = 1f / transitionDuration;
            animator.Play(animName);
        }
    }
}