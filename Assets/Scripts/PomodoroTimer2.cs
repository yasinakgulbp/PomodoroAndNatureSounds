using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PomodoroTimer2 : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public Button startStopButton;
    public Button resetButton;
    public TextMeshProUGUI statusText;

    private int workDuration = 25 * 60;  // 25 minutes
    private int shortBreakDuration = 5 * 60;  // 5 minutes
    private int longBreakDuration = 15 * 60;  // 15 minutes
    private int cyclesBeforeLongBreak = 4;

    private int currentTime;
    private bool isRunning = false;
    private bool isWorkSession = true;
    private int cycleCount = 0;

    private Coroutine timerCoroutine;

    void Awake()
    {
        Application.targetFrameRate = 10;  // Pil tüketimini azaltmak için frame rate düþürülür
    }


    void Start()
    {
        timerText.text = FormatTime(workDuration);
        statusText.text = "Work Session";
        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        startStopButton.onClick.AddListener(ToggleTimer);
        resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset";
        resetButton.onClick.AddListener(ResetTimer);
    }

    void ToggleTimer()
    {
        if (isRunning)
        {
            StopTimer();
        }
        else
        {
            StartTimer();
        }
    }

    void StartTimer()
    {
        isRunning = true;
        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stop";
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(RunTimer());
    }

    void StopTimer()
    {
        isRunning = false;
        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }

    void ResetTimer()
    {
        StopTimer();
        cycleCount = 0;
        isWorkSession = true;
        currentTime = workDuration;
        timerText.text = FormatTime(currentTime);
        statusText.text = "Work Session";
    }

    IEnumerator RunTimer()
    {
        currentTime = isWorkSession ? workDuration : (cycleCount % cyclesBeforeLongBreak == 0 ? longBreakDuration : shortBreakDuration);
        while (currentTime > 0)
        {
            timerText.text = FormatTime(currentTime);
            yield return new WaitForSeconds(1);
            currentTime--;
        }
        TimerEnded();
    }

    void TimerEnded()
    {
        if (isWorkSession)
        {
            cycleCount++;
            if (cycleCount % cyclesBeforeLongBreak == 0)
            {
                statusText.text = "Long Break";
                currentTime = longBreakDuration;
            }
            else
            {
                statusText.text = "Short Break";
                currentTime = shortBreakDuration;
            }
        }
        else
        {
            statusText.text = "Work Session";
            currentTime = workDuration;
        }

        isWorkSession = !isWorkSession;
        StartTimer();
    }

    string FormatTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
