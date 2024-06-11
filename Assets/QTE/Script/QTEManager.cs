using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using KeyCode = UnityEngine.InputSystem.Key;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;

public class QTEManager : MonoBehaviour
{
    [Header("Configuration")]
    public float slowMotionTimeScale = 0.1f;

    private bool isEventStarted;
    private QTEEvent eventData;
    private bool isAllButtonsPressed;
    private bool isFail;
    private bool isEnded;
    private bool isPaused;
    private float currentTime;
    private float smoothTimeUpdate;
    private float rememberTimeScale;
    private List<QTEKey> keys = new List<QTEKey>();
    private float totalMultiplier = 0f;
    private int qteCount = 0;
    private QTEUI GetUI()
    {
        return eventData?.keyboardUI;
    }

    public QTEEventManager qteEventManager;

    protected void Update()
    {
        if (isFail) return;  // Non iniziare nuovi QTE se c'è stato un fallimento

        float currentRealTime = Time.realtimeSinceStartup;


        if (!isEventStarted && eventData != null && currentRealTime >= eventData.startTime)
        {
            StartEvent(eventData);
        }

        if (isEventStarted)
        {
            UpdateTimer();

            if (keys.Count == 0 || isFail || currentRealTime >= eventData.endTime)
            {
                if (currentRealTime >= eventData.endTime)
                {
                    isFail = true;
                }
                DoFinally();
            }
            else
            {
                if (IsGamePadConnected())
                {
                    CheckGamepadInputs();
                }
                else
                {
                    CheckKeyboardInputs();
                }
            }
        }
    }

    void Start()
    {
        if (qteEventManager == null)
        {
            qteEventManager = FindObjectOfType<QTEEventManager>();
            if (qteEventManager == null)
            {
                Debug.LogError("QTEEventManager not found.");
                return;
            }
        }
    }

    public void StartEvent(QTEEvent eventScriptable)
    {
        Debug.Log("StartEvent() called with event: " + eventScriptable.name);

        eventData = eventScriptable;
        keys = new List<QTEKey>(eventData.keys);

        eventData.onStart?.Invoke();

        isAllButtonsPressed = false;
        isEnded = false;
        isFail = false;
        isPaused = false;
        rememberTimeScale = Time.timeScale;

        switch (eventScriptable.timeType)
        {
            case QTETimeType.Slow:
                Time.timeScale = slowMotionTimeScale;
                break;
            case QTETimeType.Paused:
                Time.timeScale = 0;
                break;
        }

        currentTime = eventData.time;
        smoothTimeUpdate = currentTime;

        SetupGUI();
        isEventStarted = true;
    }

    private string GetInterval(float successTime, float startTime, float endTime)
    {
        float totalTime = endTime - startTime;
        float intervalTime = totalTime / 6f;
        float timeSinceStart = successTime - startTime;

        if (timeSinceStart <= intervalTime) return "Ok";
        else if (timeSinceStart <= 2 * intervalTime) return "Good";
        else if (timeSinceStart <= 4 * intervalTime) return "Perfect";
        else if (timeSinceStart <= 5 * intervalTime) return "Good";
        else return "Ok";
    }

    private float GetScore(string interval)
    {
        switch (interval)
        {
            case "Perfect":
                return 1f;
            case "Good":
                return 0.7f;
            case "Ok":
                return 0.3f;
            default:
                return 0f;
        }
    }

    protected void DoFinally()
{
    if (keys.Count == 0)
    {
        isAllButtonsPressed = true;
    }
    isEnded = true;
    isEventStarted = false;
    Time.timeScale = rememberTimeScale;
    GetUI()?.eventUI.SetActive(false);
    eventData?.onEnd.Invoke();

    if (isFail)
    {
        eventData?.onFail.Invoke();
        qteEventManager.StopAllEvents();  // Blocca tutti i QTE successivi in caso di fallimento
    }
    else if (isAllButtonsPressed)
    {
        float successTime = Time.realtimeSinceStartup; // Tempo di successo
        Debug.Log("Success Time: " + successTime);
        string interval = GetInterval(successTime, eventData.startTime, eventData.endTime);
        Debug.Log("Interval: " + interval);
        float score = GetScore(interval);
        Debug.Log("Score: " + score);

        totalMultiplier += score;
        qteCount++;
        Debug.Log("Total Multiplier: " + totalMultiplier);
        Debug.Log("QTE Count: " + qteCount);

        if (qteEventManager != null && qteEventManager.IsLastEvent(eventData))
        {
            eventData?.onSuccess.Invoke(); // Mostra la GUI di successo solo se è l'ultimo QTE

            // Calcolo del moltiplicatore finale
            if (qteCount > 0)
            {
                float finalMultiplier = totalMultiplier / qteCount;
                Debug.Log("Final Multiplier: " + finalMultiplier);
            }
        }
    }
    eventData = null;
}



    private void SetupGUI()
    {
        var ui = GetUI();

        if (ui?.eventUI != null)
        {
            ui.eventUI.SetActive(true);
        }
        if (ui?.eventText != null)
        {
            ui.eventText.text = string.Join(", ", eventData.keys.ConvertAll(key => key.keyboardKey.ToString()));
        }
        if (ui?.eventTimerText != null)
        {
            ui.eventTimerText.text = eventData.time.ToString();
        }
    }

    private bool IsGamePadConnected()
    {
        return Gamepad.current != null;
    }

    private void CheckKeyboardInputs()
    {
        if (eventData.pressType == QTEPressType.Simultaneously)
        {
            CheckKeyboardInputsSimultaneously();
        }
        else
        {
            foreach (var key in new List<QTEKey>(keys))
            {
                CheckKeyboardInput(key);
            }
        }
    }

    private void CheckKeyboardInputsSimultaneously()
    {
        bool allKeysPressed = true;

        foreach (var key in keys)
        {
            if (!Keyboard.current[key.keyboardKey].isPressed)
            {
                allKeysPressed = false;
                break;
            }
        }

        if (allKeysPressed)
        {
            keys.Clear();
        }
        else
        {
            foreach (var keyboardKey in Keyboard.current.allKeys)
            {
                if (keyboardKey.wasPressedThisFrame && !keys.Exists(k => k.keyboardKey == keyboardKey.keyCode))
                {
                    HandleWrongKeyPress();
                    break;
                }
            }
        }
    }

    private void CheckKeyboardInput(QTEKey key)
    {
        if (key == null) return;

        if (Keyboard.current != null)
        {
            if (Keyboard.current[key.keyboardKey].wasPressedThisFrame)
            {
                keys.Remove(key);
            }
            else
            {
                foreach (var keyboardKey in Keyboard.current.allKeys)
                {
                    if (keyboardKey.wasPressedThisFrame && keyboardKey.keyCode != key.keyboardKey)
                    {
                        HandleWrongKeyPress();
                        break;
                    }
                }
            }
        }
    }

    private void CheckGamepadInputs()
    {
        if (eventData.pressType == QTEPressType.Simultaneously)
        {
            CheckGamepadInputsSimultaneously();
        }
        else
        {
            foreach (var key in new List<QTEKey>(keys))
            {
                CheckGamepadInput(key);
            }
        }
    }

    private void CheckGamepadInputsSimultaneously()
    {
        bool allKeysPressed = true;

        foreach (var key in keys)
        {
            if (!IsGamepadKeyPressed(key))
            {
                allKeysPressed = false;
                break;
            }
        }

        if (allKeysPressed)
        {
            keys.Clear();
        }
        else
        {
            foreach (var control in Gamepad.current.allControls)
            {
                if (control is ButtonControl button && button.wasPressedThisFrame && !IsKeyInCurrentQTE(button))
                {
                    HandleWrongKeyPress();
                    break;
                }
            }
        }
    }

    private void CheckGamepadInput(QTEKey key)
    {
        if (key == null) return;

        if (Gamepad.current != null)
        {
            bool correctKeyPressed = IsGamepadKeyPressed(key);

            if (correctKeyPressed)
            {
                keys.Remove(key);
            }
            else
            {
                foreach (var control in Gamepad.current.allControls)
                {
                    if (control is ButtonControl button && button.wasPressedThisFrame && !IsKeyInCurrentQTE(button))
                    {
                        HandleWrongKeyPress();
                        break;
                    }
                }
            }
        }
    }

    private bool IsGamepadKeyPressed(QTEKey key)
    {
        if (key.gamepadXBOXKey != 0 && Gamepad.current[key.gamepadXBOXKey].isPressed)
        {
            return true;
        }
        else if (key.gamepadDualShockKey != 0 && Gamepad.current[key.gamepadDualShockKey].isPressed)
        {
            return true;
        }
        return false;
    }

    private bool IsKeyInCurrentQTE(ButtonControl button)
    {
        foreach (var key in keys)
        {
            if ((key.gamepadXBOXKey != 0 && button.name == Gamepad.current[key.gamepadXBOXKey].name) ||
                (key.gamepadDualShockKey != 0 && button.name == Gamepad.current[key.gamepadDualShockKey].name))
            {
                return true;
            }
        }
        return false;
    }

    private void HandleWrongKeyPress()
    {
        if (isFail) return;

        isFail = true;
        Debug.Log("Wrong key pressed. Event failed.");
        DoFinally();
    }

    private void UpdateTimer()
    {
        currentTime -= Time.unscaledDeltaTime;
        smoothTimeUpdate = Mathf.MoveTowards(smoothTimeUpdate, currentTime, Time.unscaledDeltaTime);
        var ui = GetUI();
        if (ui?.eventTimerText != null)
        {
            ui.eventTimerText.text = Mathf.CeilToInt(smoothTimeUpdate).ToString();
        }
    }
}
