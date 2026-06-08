using System.Collections.Generic;
using CleanRoom.Interactables;
using KattenKasteel.FSM;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    private List<InteractionZone> activeInteractables = new List<InteractionZone>();

    [SerializeField] private Material OutlineMaterial;
    [SerializeField] private Material DefaultMaterial;

    [Header("Outline Settings")]
    [SerializeField] private float maxThickness = 10f;
    [SerializeField] private float minThickness = 0.2f;
    [SerializeField] private float blinkSpeed = 8f;

    void Start()
    {
        InteractionZone[] foundInteractables = FindObjectsByType<InteractionZone>(FindObjectsSortMode.None);

        foreach (var interactable in foundInteractables)
        {
            if (!interactable.TryGetComponent(out Transitioner transitioner))
            {
                Debug.Log("Make sure there is a transitioner attached to the interactable");
                continue;
            }

            if (!transitioner.CanTransition())
            {
                continue;
            }
            
            activeInteractables.Add(interactable);

            if (interactable.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            {
                spriteRenderer.material = OutlineMaterial;
            }
        }
    }

    private void Update()
    {
        if (activeInteractables.Count == 0) return;

        float sinValue = Mathf.Sin(Time.time * blinkSpeed);
        float normalizedValue = (sinValue + 1f) / 2f;
        float currentThickness = Mathf.Lerp(minThickness, maxThickness, normalizedValue);

        OutlineMaterial.SetFloat("_OutlineThickness", currentThickness);
    }
}