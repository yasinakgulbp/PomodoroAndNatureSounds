using UnityEngine;

public static class SettingsManager
{
    private const string WorkDurationKey = "WorkDuration";
    private const string ShortBreakDurationKey = "ShortBreakDuration";
    private const string LongBreakDurationKey = "LongBreakDuration";
    private const string CycleCountKey = "CycleCount";

    public static int GetWorkDuration(int defaultValue)
    {
        return PlayerPrefs.GetInt(WorkDurationKey, defaultValue);
    }

    public static void SetWorkDuration(int value)
    {
        PlayerPrefs.SetInt(WorkDurationKey, value);
    }

    public static int GetShortBreakDuration(int defaultValue)
    {
        return PlayerPrefs.GetInt(ShortBreakDurationKey, defaultValue);
    }

    public static void SetShortBreakDuration(int value)
    {
        PlayerPrefs.SetInt(ShortBreakDurationKey, value);
    }

    public static int GetLongBreakDuration(int defaultValue)
    {
        return PlayerPrefs.GetInt(LongBreakDurationKey, defaultValue);
    }

    public static void SetLongBreakDuration(int value)
    {
        PlayerPrefs.SetInt(LongBreakDurationKey, value);
    }

    public static int GetCycleCount(int defaultValue)
    {
        return PlayerPrefs.GetInt(CycleCountKey, defaultValue);
    }

    public static void SetCycleCount(int value)
    {
        PlayerPrefs.SetInt(CycleCountKey, value);
    }

    public static void ResetToDefault()
    {
        PlayerPrefs.DeleteKey(WorkDurationKey);
        PlayerPrefs.DeleteKey(ShortBreakDurationKey);
        PlayerPrefs.DeleteKey(LongBreakDurationKey);
        PlayerPrefs.DeleteKey(CycleCountKey);
    }
}
