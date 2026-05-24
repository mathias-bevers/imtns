using CleanRoom.Utils;
using NaughtyAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.Menus
{
    public class OverlayMenu : Menu
    {
        [SerializeField] private SerializablePair<Color, Sprite> completePair;
        [SerializeField] private SerializablePair<Color, Sprite> mistakePair;

        [SerializeField] private Image overlay;
        [SerializeField] private Image icon;
        [SerializeField] private float animationTime = 1.0f;


        protected override void Start()
        {
            base.Start();
            Close();
        }

        [Button] public void PlayCompleteAnimation() => PlayAnimation(completePair);

        [Button] public void PlayMistakeAnimation() => PlayAnimation(mistakePair);

        private void PlayAnimation(SerializablePair<Color, Sprite> pair)
        {
            Open();
            overlay.color = Color.clear;
            icon.color = Color.clear;
            icon.sprite = pair.Second;


            Sequence.Create().Group(Tween.Color(overlay, pair.First, animationTime))
                .Group(Tween.Color(icon, Color.white, animationTime - 0.5f, startDelay: 0.5f))
                .OnComplete(Close);
        }
    }
}