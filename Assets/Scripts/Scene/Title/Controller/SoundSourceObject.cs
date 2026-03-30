using UnityEngine;

namespace Scene.Controller
{
    public class SoundSourceObject : MonoBehaviour
    {
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource seSource;
        private static SoundSourceObject instance;
        public static SoundSourceObject Instance => instance;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }
    }
}
