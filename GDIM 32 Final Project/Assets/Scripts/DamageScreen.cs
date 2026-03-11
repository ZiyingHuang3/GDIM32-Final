using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageScreen : MonoBehaviour
{
    [SerializeField] private Image overlay;
    [SerializeField] private float flashAlpha = 0.5f;
    [SerializeField] private float fadeSpeed = 4f;

    private void Awake()
    {
        overlay.enabled = false;
    }
    private void OnEnable()
    {
        GameEvents.OnHealthChanged += OnHealthChanged;
       // GameEvents.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        GameEvents.OnHealthChanged -= OnHealthChanged;
    }

    private void Start()
    {
        overlay.enabled = true;
    }
    private void OnGameStarted()
    {
        overlay.enabled = true;
    }
    private void OnHealthChanged(int health)
    {
        Flash();
        Debug.Log("Damage Flash Triggered");
    }
    private void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Color color = overlay.color;
        color.a = flashAlpha;
        overlay.color = color;

        while (overlay.color.a > 0)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            overlay.color = color;
            yield return null;
        }
    }
}
