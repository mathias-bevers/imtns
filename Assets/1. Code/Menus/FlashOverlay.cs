using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.Menus
{
    public class FlashOverlay : Singleton<FlashOverlay>
    {
        private static bool _isInitialized;

        [SerializeField] private float animationDurationInSeconds;
        [SerializeField] private Color goodColor;
        [SerializeField] private Color badColor;

        private Image overlay;

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1;

            overlay = new GameObject("Overlay").AddComponent<Image>();
            overlay.sprite = Resources.Load<Sprite>("white1x1");
            overlay.color = Color.clear;

            RectTransform overlayRectTransform = (RectTransform)overlay.transform;
            overlayRectTransform.SetParent(canvas.transform);
            overlayRectTransform.anchorMin = Vector2.zero;
            overlayRectTransform.anchorMax = Vector2.one;
            overlayRectTransform.offsetMin = overlayRectTransform.offsetMax = new Vector2(0, 0);

            overlay.gameObject.SetActive(false);
            _isInitialized = true;
        }

        public void Animate(bool isGood)
        {
            Color targetColor = isGood ? goodColor : badColor;
            
            overlay.gameObject.SetActive(true);
            
            Sequence.Create(cycles: 1)
                .Chain(Tween.Color(overlay, targetColor, animationDurationInSeconds))
                .Chain(Tween.Color(overlay, Color.clear, animationDurationInSeconds))
                .OnComplete(() => overlay.gameObject.SetActive(false));
        }
    }
}