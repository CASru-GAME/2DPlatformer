using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Scene.Controller
{
    public class VolumeSetting : MonoBehaviour
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider seVolumeSlider;
        [SerializeField] private AudioMixer audioMixer;

        public void Awake()
        {
            audioMixer.GetFloat("MASTER", out float masterVolume);
            audioMixer.GetFloat("BGM", out float bgmVolume);
            audioMixer.GetFloat("SE", out float seVolume);

            masterVolumeSlider.value = masterVolume;
            bgmVolumeSlider.value = bgmVolume;
            seVolumeSlider.value = seVolume;
        }

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("MASTER", volume);
        }

        public void SetBGMVolume(float volume)
        {
            audioMixer.SetFloat("BGM", volume);
        }

        public void SetSEVolume(float volume)
        {
            audioMixer.SetFloat("SE", volume);
        }
    }
}
