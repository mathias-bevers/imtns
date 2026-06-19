using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.UserInterface
{
    public class SoundSettings : MonoBehaviour
    {
        private const string SFX = "SFXVolume";
        private const string MAIN = "MainVolume";

        [SerializeField] private Slider main;
        [SerializeField] private Slider music;
        [SerializeField] private Slider sfx;

        private SoundManager soundManager;

        private void OnEnable()
        {
            soundManager = SoundManager.Instance;

            sfx.value = soundManager.GetVolume(SFX);
            main.value = soundManager.GetVolume(MAIN);

            sfx.onValueChanged.AddListener(soundManager.SetVolumeSFX);
            main.onValueChanged.AddListener(soundManager.SetVolumeMain);
        }

        private void OnDisable()
        {
            sfx.onValueChanged.RemoveListener(soundManager.SetVolumeSFX);
            main.onValueChanged.RemoveListener(soundManager.SetVolumeMain);
        }
    }
}