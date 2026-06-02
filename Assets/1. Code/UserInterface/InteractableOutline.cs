using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOutline : MonoBehaviour
{
    private Material material;

    [Header("Outline Settings")]
    [SerializeField] private float maxThickness = 5f;
    [SerializeField] private float minThickness = 0f;
    [SerializeField] private float blinkSpeed = 4f;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        float sinValue = Mathf.Sin(Time.time * blinkSpeed);

        float normalizedValue = (sinValue + 1f) / 2f;

        float currentThickness = Mathf.Lerp(minThickness, maxThickness, normalizedValue);

        material.SetFloat("_OutlineThickness", currentThickness);
    }
}