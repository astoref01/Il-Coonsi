using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KeyCode = UnityEngine.InputSystem.Key;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

[System.Serializable]
public class QTEEventTime
{
    public float startTime;
    public float endTime;
}

public class QTEEventManager : MonoBehaviour
{
    public QTEEvent qteEventPrefab;
    public TextAsset qteEventFile;
    public List<QTEEvent> qteEvents = new List<QTEEvent>();
    public List<QTEEventTime> eventTimes = new List<QTEEventTime>();

    private bool hasFailed = false;
    private bool isActive = false;
    public GameObject timerGameObject;
    private Timer timer;

    void OnEnable()
    {
        isActive = true;
    }

    void OnDisable()
    {
        isActive = false;
    }

    
    void Start()
    {
        if (isActive)
        {
            timer = timerGameObject.GetComponent<Timer>();
            LoadQTEEvents();
        }
    }

    void LoadQTEEvents()
    {
        if (qteEventFile != null)
        {
            string[] lines = qteEventFile.text.Split('\n');

            List<string> validLines = new List<string>();
            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
                {
                    validLines.Add(line.Trim());
                }
            }

            if (validLines.Count < eventTimes.Count)
            {
                Debug.LogError("Not enough lines in the QTE event file to match the number of event times.");
                return;
            }

            System.Random random = new System.Random();
            for (int i = 0; i < eventTimes.Count; i++)
            {
                int randomIndex = random.Next(validLines.Count);
                string line = validLines[randomIndex];
                string[] inputs = line.Split(';');

                QTEEvent qteEvent = Instantiate(qteEventPrefab);

                foreach (string input in inputs)
                {
                    string[] buttons = input.Trim().Split(',');

                    QTEKey qteKey = new QTEKey();

                    foreach (string button in buttons)
                    {
                        string buttonName = button.Trim();
                        KeyCode keyboardKey;
                        GamepadButton gamepadXboxButton;
                        GamepadButton gamepadDualShockButton;

                        if (Enum.TryParse(buttonName, true, out keyboardKey))
                        {
                            qteKey.keyboardKey = keyboardKey;
                        }
                        else if (Enum.TryParse(buttonName, true, out gamepadXboxButton))
                        {
                            qteKey.gamepadXBOXKey = gamepadXboxButton;
                        }
                        else if (Enum.TryParse(buttonName, true, out gamepadDualShockButton))
                        {
                            qteKey.gamepadDualShockKey = gamepadDualShockButton;
                        }
                        else
                        {
                            Debug.LogError("Invalid button: " + buttonName);
                        }
                    }

                    qteEvent.keys.Add(qteKey);
                }

                qteEvent.startTime = eventTimes[i].startTime;
                qteEvent.endTime = eventTimes[i].endTime;

                Debug.Log($"QTE Event {i} -> StartTime: {qteEvent.startTime}, EndTime: {qteEvent.endTime}");

                qteEvent.timeType = QTETimeType.Slow;
                qteEvent.pressType = QTEPressType.Simultaneously;

                qteEvents.Add(qteEvent);
            }
        }
        else
        {
            Debug.LogError("QTE event file not found.");
        }
    }

    void Update()
    {
        if (isActive)
        {  
            if (hasFailed) return;

            float currentTime = timer.ElapsedTime;

            foreach (var qteEvent in qteEvents)
            {
                if (currentTime >= qteEvent.startTime && currentTime < qteEvent.endTime)
                {
                    Debug.Log("QTE Active");
                    StartEvent(qteEvent);
                    qteEvents.Remove(qteEvent);
                    break;
                }
                else if (currentTime >= qteEvent.endTime && !qteEvent.hasEnded)
                {
                    Debug.Log("QTE Failed");
                    qteEvent.hasEnded = true;
                    qteEvent.onFail.Invoke();
                    hasFailed = true;
                    break;
                }
            }
        }
    }
    private void StartEvent(QTEEvent qteEvent)
    {
        if (isActive)
        {
            QTEManager qteManager = FindObjectOfType<QTEManager>();
            if (qteManager != null)
            {
                qteManager.StartEvent(qteEvent);
            }
            else
            {
                Debug.LogError("QTEManager not found.");
            }
        }
    }

    public void StopAllEvents()
    {
        hasFailed = true;
    }

    public bool IsLastEvent(QTEEvent qteEvent)
    {
        return qteEvents.IndexOf(qteEvent) == qteEvents.Count - 1;
    }
}
