using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuckDragSensor : MonoBehaviour
{
    [Header("Slider Reference")]
    [SerializeField] private Slider slider;

    [Header("Threshold Settings")]
    [SerializeField][Range(0f, 1f)] private float valueThreshold = 0.7f; // Threshold value (0 to 1)
    [SerializeField] private float speedThreshold = 2.0f; // Minimum speed (units per second)

    [Header("Callback")]
    [SerializeField] private UnityEvent<float> onFastRightDragPastThreshold;

    private float previousValue;
    private float previousTime;
    private bool thresholdCrossed = false; // To prevent multiple triggers
    private bool wasAboveThreshold = false;

    private void Start()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        previousValue = slider.value;
        previousTime = Time.time;
        wasAboveThreshold = slider.value >= valueThreshold;
    }

    private void Update()
    {
        float currentValue = slider.value;
        float currentTime = Time.time;

        // Calculate delta and speed
        float delta = currentValue - previousValue;
        float deltaTime = currentTime - previousTime;

        if (deltaTime > 0)
        {
            float speed = Mathf.Abs(delta) / deltaTime;
            bool isMovingRight = delta > 0;
            bool isAboveThreshold = currentValue >= valueThreshold;

            // Check if we're moving right fast enough AND crossing the threshold
            if (isMovingRight &&
                speed >= speedThreshold &&
                isAboveThreshold &&
                !wasAboveThreshold &&
                !thresholdCrossed)
            {
                // Trigger the callback
                onFastRightDragPastThreshold?.Invoke(speed);
                thresholdCrossed = true;
            }

            // Reset threshold crossing flag when we go below threshold
            if (!isAboveThreshold)
            {
                thresholdCrossed = false;
            }

            // Update the wasAboveThreshold state
            wasAboveThreshold = isAboveThreshold;
        }

        // Update previous values
        previousValue = currentValue;
        previousTime = currentTime;
    }


    // Method to reset the tracking (useful if you manually change slider value)
    public void ResetTracking()
    {
        previousValue = slider.value;
        previousTime = Time.time;
        wasAboveThreshold = slider.value >= valueThreshold;
        thresholdCrossed = false;
    }

    // Optional: Methods to connect to slider events
    public void OnSliderPointerDown()
    {
        ResetTracking();
    }

    public void OnSliderPointerUp()
    {
        // Reset the crossing flag when user releases
        thresholdCrossed = false;
    }

    // Public property to change threshold at runtime
    public float ValueThreshold
    {
        get => valueThreshold;
        set => valueThreshold = Mathf.Clamp01(value);
    }

    public float SpeedThreshold
    {
        get => speedThreshold;
        set => speedThreshold = Mathf.Max(0, value);
    }
}