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

            if (animationDurationInSeconds <= 0)
            {
                LoadBaseValues();
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

        private void LoadBaseValues()
        {
            animationDurationInSeconds = 0.5f;
            goodColor = new Color32(166, 218, 149, 100);
            badColor = new Color32(237, 135, 150, 100);
        }

        public void PlayAnimation(bool isGood)
        {
            Color targetColor = isGood ? goodColor : badColor;
            float partDuration = animationDurationInSeconds / 2.0f;
            
            overlay.gameObject.SetActive(true);
            
            Sequence.Create(cycles: 1)
                .Chain(Tween.Color(overlay, targetColor, partDuration))
                .Chain(Tween.Color(overlay, Color.clear, partDuration))
                .OnComplete(() => overlay.gameObject.SetActive(false));
        }
    }
}