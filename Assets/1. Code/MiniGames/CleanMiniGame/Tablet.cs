using UnityEngine;

namespace CleanRoom.MiniGames.CleanMiniGame
{
    public class Tablet : MonoBehaviour
    {
        [SerializeField] private RectTransform dirtPrefab;
        
        public void SpawnDirt()
        {
            int dirtCount = Random.Range(3, 6);

            for (int i = 0; i < dirtCount; ++i)
            {
                RectTransform dirt = Instantiate(dirtPrefab, transform);
                float scale = Random.Range(50, 111) * 0.01f;
                dirt.sizeDelta *= scale;

                Vector2 position = Random.insideUnitCircle * 401;
                dirt.anchoredPosition = position;
            }
        }
    }
}