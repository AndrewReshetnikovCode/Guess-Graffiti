using UnityEngine;
using System.Collections;

public class UIShakeEffect : MonoBehaviour
{
    [SerializeField] private float defaultShakeDuration = 0.5f;
    [SerializeField] private float shakeIntensityMultiplier = 5f;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private bool isShaking = false;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("UIShakeEffect requires a RectTransform component!");
            return;
        }

        originalPosition = rectTransform.anchoredPosition;
    }

    /// <summary>
    /// Shake the UI element with default intensity
    /// </summary>
    public void Shake()
    {
        Shake(shakeIntensityMultiplier);
    }

    /// <summary>
    /// Shake the UI element with specified intensity
    /// </summary>
    /// <param name="intensity">Shake intensity (recommended: 1-20)</param>
    public void Shake(float intensity)
    {
        if (rectTransform == null) return;

        // If already shaking, stop the current shake
        if (isShaking && shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            rectTransform.anchoredPosition = originalPosition;
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine(intensity * shakeIntensityMultiplier, defaultShakeDuration));
    }

    /// <summary>
    /// Shake the UI element with specified intensity and duration
    /// </summary>
    /// <param name="intensity">Shake intensity</param>
    /// <param name="duration">Shake duration in seconds</param>
    public void Shake(float intensity, float duration)
    {
        if (rectTransform == null) return;

        // If already shaking, stop the current shake
        if (isShaking && shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            rectTransform.anchoredPosition = originalPosition;
        }

        shakeCoroutine = StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        isShaking = true;
        originalPosition = rectTransform.anchoredPosition;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // Calculate shake amount based on time
            float currentIntensity = intensity * (1f - (elapsedTime / duration));

            // Generate random offset
            float offsetX = Random.Range(-currentIntensity, currentIntensity);
            float offsetY = Random.Range(-currentIntensity, currentIntensity);

            // Apply the shake offset
            rectTransform.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            yield return null;
        }

        // Return to original position
        rectTransform.anchoredPosition = originalPosition;
        isShaking = false;
    }

    /// <summary>
    /// Stop the shake animation immediately
    /// </summary>
    public void StopShake()
    {
        if (isShaking && shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            rectTransform.anchoredPosition = originalPosition;
            isShaking = false;
        }
    }

    void OnDisable()
    {
        // Reset position when component is disabled
        if (rectTransform != null && isShaking)
        {
            rectTransform.anchoredPosition = originalPosition;
            isShaking = false;
        }
    }

    void OnDestroy()
    {
        // Clean up
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
    }
}