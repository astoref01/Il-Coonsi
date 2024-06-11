using UnityEngine;

public class Timer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool timerRunning = false;

    public float ElapsedTime => elapsedTime;

    private void OnEnable()
    {
        StartTimer();
    }

    private void OnDisable()
    {
        ResetTimer();
    }

    private void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.unscaledDeltaTime;
        }
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void ResetTimer()
    {
        timerRunning = false;
        elapsedTime = 0f;
    }
}
