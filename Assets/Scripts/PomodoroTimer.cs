using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PomodoroTimer : MonoBehaviour
{
    #region Variables
    public TextMeshProUGUI timerText;
    public Button startStopButton;
    public Button resetButton;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI sessionStatusText;
    public TMP_InputField workDurationInput;
    public TMP_InputField shortBreakInput;
    public TMP_InputField longBreakInput;
    public TMP_InputField cycleCountInput;
    public Button applyButton;
    public GameObject resetConfirmationPanel;
    public Button yesButton;
    public Button cancelButton;

    public AudioManager audioManager;

    private int defaultWorkDuration = 25 * 60;  // 25 minutes
    private int defaultShortBreakDuration = 5 * 60;  // 5 minutes
    private int defaultLongBreakDuration = 15 * 60;  // 15 minutes
    private int defaultCycleCount = 4;

    private int workDuration;
    private int shortBreakDuration;
    private int longBreakDuration;
    private int cyclesBeforeLongBreak;

    private int currentTime;
    private bool isRunning = false;
    private bool isWorkSession = true;
    private int cycleCount = 0;
    private Coroutine timerCoroutine;
    #endregion

    #region Initialization
    void Awake()
    {
        Application.targetFrameRate = 30;  // Pil tüketimini azaltmak için frame rate düþürülür

        // Süreleri PlayerPrefs'ten yükleyin veya varsayýlan deðerlere ayarlayýn
        LoadSettings();
    }

    void Start()
    {
        InitializeTimer();
        UpdateInputFields();

        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
        startStopButton.onClick.AddListener(() =>
        {
            ToggleTimer();
            audioManager.PlayButtonClickSound();
        });

        resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Reset";
        resetButton.onClick.AddListener(() =>
        {
            ShowResetConfirmationPanel();
            audioManager.PlayButtonClickSound();
        });

        applyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Apply";
        applyButton.onClick.AddListener(() =>
        {
            ApplySettings();
            audioManager.PlayButtonClickSound();
        });

        yesButton.onClick.AddListener(() =>
        {
            ConfirmReset();
            audioManager.PlayButtonClickSound();
        });

        cancelButton.onClick.AddListener(() =>
        {
            HideResetConfirmationPanel();
            audioManager.PlayButtonClickSound();
        });
    }

    void InitializeTimer()
    {
        currentTime = workDuration;
        timerText.text = FormatTime(currentTime);
        statusText.text = "Work Session";
        UpdateSessionStatusText();
    }
    #endregion

    #region Timer Control
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
    #endregion

    #region Reset Confirmation
    void ShowResetConfirmationPanel()
    {
        resetConfirmationPanel.SetActive(true);
    }

    void HideResetConfirmationPanel()
    {
        resetConfirmationPanel.SetActive(false);
    }

    void ConfirmReset()
    {
        resetConfirmationPanel.SetActive(false);
        ResetTimer();
    }

    void ResetTimer()
    {
        // Varsayýlan deðerlere dönün
        SettingsManager.ResetToDefault();
        LoadSettings();
        UpdateInputFields();

        StopTimer();
        cycleCount = 0;
        isWorkSession = true;
        currentTime = workDuration;
        timerText.text = FormatTime(currentTime);
        statusText.text = "Work Session";
        UpdateSessionStatusText();
    }
    #endregion

    #region Settings
    void ApplySettings()
    {
        int work, shortBreak, longBreak, cycles;

        if (int.TryParse(workDurationInput.text, out work) && work > 0)
        {
            workDuration = work * 60;
            SettingsManager.SetWorkDuration(workDuration);
        }

        if (int.TryParse(shortBreakInput.text, out shortBreak) && shortBreak > 0)
        {
            shortBreakDuration = shortBreak * 60;
            SettingsManager.SetShortBreakDuration(shortBreakDuration);
        }

        if (int.TryParse(longBreakInput.text, out longBreak) && longBreak > 0)
        {
            longBreakDuration = longBreak * 60;
            SettingsManager.SetLongBreakDuration(longBreakDuration);
        }

        if (int.TryParse(cycleCountInput.text, out cycles) && cycles > 0)
        {
            cyclesBeforeLongBreak = cycles;
            SettingsManager.SetCycleCount(cyclesBeforeLongBreak);
        }

        InitializeTimer();
    }

    void LoadSettings()
    {
        workDuration = SettingsManager.GetWorkDuration(defaultWorkDuration);
        shortBreakDuration = SettingsManager.GetShortBreakDuration(defaultShortBreakDuration);
        longBreakDuration = SettingsManager.GetLongBreakDuration(defaultLongBreakDuration);
        cyclesBeforeLongBreak = SettingsManager.GetCycleCount(defaultCycleCount);
    }

    void UpdateInputFields()
    {
        workDurationInput.text = (workDuration / 60).ToString();
        shortBreakInput.text = (shortBreakDuration / 60).ToString();
        longBreakInput.text = (longBreakDuration / 60).ToString();
        cycleCountInput.text = cyclesBeforeLongBreak.ToString();
    }

    void UpdateSessionStatusText()
    {
        sessionStatusText.text = string.Format("{0}/{1}", cycleCount, cyclesBeforeLongBreak);
    }
    #endregion

    #region Timer Logic
    IEnumerator RunTimer()
    {
        while (currentTime > 0)
        {
            timerText.text = FormatTime(currentTime);
            yield return new WaitForSeconds(1);
            currentTime--;
        }

        OnTimerEnd();
    }

    void OnTimerEnd()
    {
        isRunning = false;
        startStopButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start";

        if (isWorkSession)
        {
            cycleCount++;
            audioManager.PlaySessionChangeSound();
        }

        if (cycleCount >= cyclesBeforeLongBreak)
        {
            isWorkSession = false;
            currentTime = longBreakDuration;
            cycleCount = 0;
            statusText.text = "Long Break";
        }
        else
        {
            isWorkSession = !isWorkSession;
            currentTime = isWorkSession ? workDuration : shortBreakDuration;
            statusText.text = isWorkSession ? "Work Session" : "Short Break";
        }

        UpdateSessionStatusText();
        timerText.text = FormatTime(currentTime);
    }

    string FormatTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    #endregion
}
