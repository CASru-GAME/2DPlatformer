using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Scene.Controller
{
    public class SoundSourceObject : MonoBehaviour
    {
        [SerializeField] private AudioSource bgmSourceA;
        [SerializeField] private AudioSource bgmSourceB;
        [SerializeField] private AudioSource seSource;
        private static SoundSourceObject instance;
        public static SoundSourceObject Instance => instance;
        [SerializeField] private AudioClip titleBGMA;
        [SerializeField] private AudioClip titleBGMB;
        [SerializeField] private AudioClip perkBGMA;
        [SerializeField] private AudioClip perkBGMB;
        [SerializeField] private AudioClip gameBGMA;
        [SerializeField] private AudioClip gameBGMB;
        [SerializeField] private AudioClip buttonSE;
        [SerializeField] private AudioClip damagedSE;
        [SerializeField] private AudioClip jumpSE;
        [SerializeField] private AudioClip landSE;
        [SerializeField] private AudioClip clearSE;
        [SerializeField] private AudioClip healSE;
        [SerializeField] private AudioClip shieldSE;
        [SerializeField] private AudioClip warpSE;
        [SerializeField] private AudioClip invincibleSE;
        [SerializeField] private AudioLowPassFilter lowPassFilter;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        private IEnumerator PlayBGMCoroutine()
        {
            double startTime = AudioSettings.dspTime;
            bgmSourceA.PlayScheduled(startTime);
            bgmSourceB.PlayScheduled(startTime + bgmSourceA.clip.length);

            double nextStartTime = startTime + bgmSourceA.clip.length;

            while(true)
            {
                while(bgmSourceA.isPlaying)
                    yield return null;
                bgmSourceA.clip = bgmSourceB.clip;
                nextStartTime += bgmSourceB.clip.length - 0.1f * Time.timeScale;
                bgmSourceA.PlayScheduled(nextStartTime);

                while(bgmSourceB.isPlaying)
                    yield return null;
                nextStartTime += bgmSourceA.clip.length - 0.1f * Time.timeScale;
                bgmSourceB.PlayScheduled(nextStartTime);
            }
        }

        public void PlayTitleBGM()
        {
            bgmSourceA.Stop();
            bgmSourceB.Stop();
            bgmSourceA.clip = titleBGMA;
            bgmSourceB.clip = titleBGMB;
            StartCoroutine(PlayBGMCoroutine());
        }

        public void PlayPerkBGM()
        {
            bgmSourceA.Stop();
            bgmSourceB.Stop();
            bgmSourceA.clip = perkBGMA;
            bgmSourceB.clip = perkBGMB;
            StartCoroutine(PlayBGMCoroutine());
        }

        public void PlayGameBGM()
        {
            bgmSourceA.Stop();
            bgmSourceB.Stop();
            bgmSourceA.clip = gameBGMA;
            bgmSourceB.clip = gameBGMB;
            StartCoroutine(PlayBGMCoroutine());
        }

        public void PlayButtonSE()
        {
            seSource.PlayOneShot(buttonSE);
        }

        public void PlayDamagedSE()
        {
            seSource.PlayOneShot(damagedSE);
        }

        public void PlayJumpSE()
        {
            seSource.PlayOneShot(jumpSE);
        }

        public void PlayLandSE()
        {
            seSource.PlayOneShot(landSE);
        }

        public void PlayClearSE()
        {
            bgmSourceA.Stop();
            bgmSourceB.Stop();
            seSource.PlayOneShot(clearSE);
        }

        public void PlayHealSE()
        {
            seSource.PlayOneShot(healSE);
        }

        public void PlayShieldSE()
        {
            seSource.PlayOneShot(shieldSE);
        }

        public void PlayWarpSE()
        {
            seSource.PlayOneShot(warpSE);
        }

        public void PlayInvincibleSE()
        {
            seSource.PlayOneShot(invincibleSE);
        }

        public void ActivateLowPassFilter(bool isActive)
        {
            lowPassFilter.enabled = isActive;
        }
    }
}
