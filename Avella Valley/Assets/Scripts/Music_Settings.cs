using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Music_Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    const string MIXER_ALLMUSIC = "AllMusic";
    const string MIXER_MUSIC = "Music";
    const string MIXER_SOUND = "Sounds";

    bool mute = false;

    [SerializeField] private Sprite soundSprite;
    [SerializeField] private Sprite muteSprite;
    [SerializeField] private Image soundImg;

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    void SetSoundVolume(float value)
    {
        mixer.SetFloat(MIXER_SOUND, Mathf.Log10(value) * 20);
    }

    public void Mute()
    {
        if (mute)
        {
            mixer.SetFloat(MIXER_ALLMUSIC, 0);
            soundImg.sprite = muteSprite;
        }
        else
        {
            mixer.SetFloat(MIXER_ALLMUSIC, -80);
            soundImg.sprite = soundSprite;
        }
        mute = !mute;
    }
}
