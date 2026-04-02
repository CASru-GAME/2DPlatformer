using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

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