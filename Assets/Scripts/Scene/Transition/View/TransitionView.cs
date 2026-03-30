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
            animator.Play(animName);
        }
    }
}