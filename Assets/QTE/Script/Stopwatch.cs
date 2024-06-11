using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
    public Text stopwatchText; // Riferimento al componente Text che mostrerà il cronometro
    private bool isRunning = false;
    private float startTime;
    private float appStartTime; // Tempo iniziale dell'applicazione

    void Start()
    {
        appStartTime = Time.realtimeSinceStartup; // Salva il tempo di avvio dell'applicazione
        StartStopwatch(); // Avvia il cronometro quando lo script viene avviato
    }

    void Update()
    {
        if (isRunning)
        {
            // Calcola il tempo trascorso dall'avvio dell'applicazione
            float elapsedTime = Time.realtimeSinceStartup - appStartTime;
            UpdateStopwatchText(elapsedTime);
        }
    }

    public void StartStopwatch()
    {
        // Salva il tempo di avvio del cronometro
        startTime = Time.realtimeSinceStartup;
        isRunning = true;
    }

    public void StopStopwatch()
    {
        isRunning = false;
    }

    public void ResetStopwatch()
    {
        // Resetta il tempo di avvio del cronometro
        startTime = Time.realtimeSinceStartup;
        UpdateStopwatchText(0);
    }

    void UpdateStopwatchText(float elapsedTime)
    {
        string minutes = Mathf.Floor(elapsedTime / 60).ToString("00");
        string seconds = (elapsedTime % 60).ToString("00");
        string milliseconds = ((elapsedTime * 100) % 100).ToString("00");
        stopwatchText.text = minutes + ":" + seconds + ":" + milliseconds;
    }
}
