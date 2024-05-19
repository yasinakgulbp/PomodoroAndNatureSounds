using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    #region Variables
    public AudioClip[] backgroundMusic;
    public AudioClip sessionChangeSound;
    public AudioClip buttonClickSound;

    public AudioSource backgroundMusicSource;
    public AudioSource effectsSource;
    public Slider volumeSlider;

    private int currentMusicIndex = -1;
    #endregion

    #region Initialization
    void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        SetVolume(volumeSlider.value);
    }
    #endregion

    #region Background Music Control
    public void PlayBackgroundMusic(int index)
    {
        if (index < 0 || index >= backgroundMusic.Length)
        {
            Debug.LogError("Invalid music index");
            return;
        }

        if (currentMusicIndex == index)
        {
            StopBackgroundMusic();
            return;
        }

        backgroundMusicSource.clip = backgroundMusic[index];
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
        currentMusicIndex = index;
    }

    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
        currentMusicIndex = -1;
    }
    #endregion

    #region Sound Effects
    public void PlaySessionChangeSound()
    {
        effectsSource.PlayOneShot(sessionChangeSound);
    }

    public void PlayButtonClickSound()
    {
        effectsSource.PlayOneShot(buttonClickSound);
    }
    #endregion

    #region Volume Control
    public void SetVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
        effectsSource.volume = volume;
    }
    #endregion
}
