using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.UserInterface
{
    public class SoundSettings : MonoBehaviour
    {
        [SerializeField] private Slider main;
        [SerializeField] private Slider music;
        [SerializeField] private Slider sfx;

        private SoundManager soundManager;

        private void OnEnable()
        {
            soundManager = SoundManager.Instance;

            main.value = soundManager.GetVolume(SoundManager.MAIN);
            music.value = soundManager.GetVolume(SoundManager.MUSIC);
            sfx.value = soundManager.GetVolume(SoundManager.SFX);
            
            main.onValueChanged.AddListener(value => soundManager.SetVolume(SoundManager.MAIN, value));
            music.onValueChanged.AddListener(value => soundManager.SetVolume(SoundManager.MUSIC, value));
            sfx.onValueChanged.AddListener(value => soundManager.SetVolume(SoundManager.SFX, value));
        }

        private void OnDisable()
        {
            main.onValueChanged.RemoveAllListeners();
            music.onValueChanged.RemoveAllListeners();
            sfx.onValueChanged.RemoveAllListeners();
        }
    }
}