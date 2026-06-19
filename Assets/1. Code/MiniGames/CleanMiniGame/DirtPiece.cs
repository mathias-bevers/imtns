using System;
using CleanRoom.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class DirtPiece : MonoBehaviour
    {
        [SerializeField] private float cleanDistanceBase;
        [SerializeField] private Sprite[] sprites;

        private Tablet tablet;
        private Bounds bounds;
        private Image image;
        private RectTransform cachedTransform;
        private Transform wipeTransform;

        private void Update()
        {
            if (tablet.Cleanliness != Tablet.CleanlinessLevel.Sprayed)
            {
                return;
            }

            CheckWipe();
        }

        private void OnDestroy()
        {
            destroyedEvent?.Invoke();
        }

        public event Action destroyedEvent;

        public void Initialize(float scale, Vector2 position)
        {
            cachedTransform = (RectTransform)transform;
            image = GetComponent<Image>();
            wipeTransform = CleanGame.Instance.Wipe.CachedTransform;
            tablet = CleanGame.Instance.Tablet;

            image.sprite = sprites.GetRandomElement();

            cachedTransform.sizeDelta *= scale;
            cachedTransform.anchoredPosition = position;

            Rect rect = cachedTransform.rect;
            bounds = new Bounds(cachedTransform.position, new Vector3(rect.width * 0.5f, rect.height * 0.5f, 1));
            bounds.size *= 0.75f;
        }

        private void CheckWipe()
        {
            if (!bounds.Contains(wipeTransform.position))
            {
                return;
            }

            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            if (bounds.size.magnitude < 0.01f)
            {
                return;
            }

            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}