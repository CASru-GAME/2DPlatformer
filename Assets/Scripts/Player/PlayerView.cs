using UnityEngine;

namespace Player.View
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        public void PlayAnimation(string animationName)
        {
            animator.Play(animationName);
        }
    }
}