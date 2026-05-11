using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.Menus
{
    public class OverlayMenu : Menu
    {
        // color is in 0.0 - 1.0 format, thus color32 is used.
        [SerializeField] private Color32 mistake = new(231, 130, 132, 100);
        [SerializeField] private Color32 complete = new(166, 209, 137, 100);
        [SerializeField] private Image overlay;
        [SerializeField] private Animation overlayAnimation;

        private float animationTime = -1.0f;

        protected override void Start()
        {
            base.Start();
            Close();
            animationTime = overlayAnimation.clip.length + 0.01f;
        }

        [Button] public void PlayCompleteAnimation() => PlayAnimation(complete);

        [Button] public void PlayMistakeAnimation() => PlayAnimation(mistake);

        private void PlayAnimation(Color color)
        {
            Open();
            overlay.color = color;
            overlayAnimation.Play();
            StopAllCoroutines(); 
            StartCoroutine(CloseOnClipFinish());
        }

        private IEnumerator CloseOnClipFinish()
        {
            yield return new WaitForSecondsRealtime(animationTime);
            Close();
        }
    }
}