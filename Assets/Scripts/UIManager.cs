using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject backgroundMusicPanel;
    public GameObject infoPanel;
    public Button settingsButton;
    public Button backgroundMusicButton;
    public Button infoButton;
    public Button closeSettingsButton;
    public Button closeBackgroundMusicButton;
    public Button closeInfoButton;

    void Start()
    {
        Application.runInBackground = true;
        // Panelleri baþta kapalý tut
        settingsPanel.SetActive(false);
        backgroundMusicPanel.SetActive(false);
        infoPanel.SetActive(false);

        // Buton click eventleri ekle
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        backgroundMusicButton.onClick.AddListener(OpenBackgroundMusicPanel);
        infoButton.onClick.AddListener(OpenInfoPanel);
        closeSettingsButton.onClick.AddListener(CloseSettingsPanel);
        closeBackgroundMusicButton.onClick.AddListener(CloseBackgroundMusicPanel);
        closeInfoButton.onClick.AddListener(CloseInfoPanel);
    }

    void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        backgroundMusicPanel.SetActive(false);
    }

    void CloseSettingsPanel()
    {
        Application.targetFrameRate = 10;
        settingsPanel.SetActive(false);
    }

    void OpenBackgroundMusicPanel()
    {
        Application.targetFrameRate = 60;
        backgroundMusicPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    void CloseBackgroundMusicPanel()
    {
        Application.targetFrameRate = 10;
        backgroundMusicPanel.SetActive(false);
    }

    void OpenInfoPanel()
    {
        Application.targetFrameRate = 60;
        infoPanel.SetActive(true);
        settingsPanel.SetActive(false);
        backgroundMusicPanel.SetActive(false);
    }

    void CloseInfoPanel()
    {
        Application.targetFrameRate = 10;
        infoPanel.SetActive(false);
    }
}
