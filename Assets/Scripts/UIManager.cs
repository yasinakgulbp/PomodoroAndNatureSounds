using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject backgroundMusicPanel;
    public Button settingsButton;
    public Button backgroundMusicButton;
    public Button closeSettingsButton;
    public Button closeBackgroundMusicButton;

    void Start()
    {
        // Panelleri baþta kapalý tut
        settingsPanel.SetActive(false);
        backgroundMusicPanel.SetActive(false);

        // Buton click eventleri ekle
        settingsButton.onClick.AddListener(OpenSettingsPanel);
        backgroundMusicButton.onClick.AddListener(OpenBackgroundMusicPanel);
        closeSettingsButton.onClick.AddListener(CloseSettingsPanel);
        closeBackgroundMusicButton.onClick.AddListener(CloseBackgroundMusicPanel);
    }

    void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        backgroundMusicPanel.SetActive(false);
    }

    void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    void OpenBackgroundMusicPanel()
    {
        backgroundMusicPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    void CloseBackgroundMusicPanel()
    {
        backgroundMusicPanel.SetActive(false);
    }
}
