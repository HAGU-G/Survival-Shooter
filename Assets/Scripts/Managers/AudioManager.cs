using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public AudioSource backgroundMusic;

    public static readonly string masterVol = "masterVol";
    public static readonly string musicVol = "musicVol";
    public static readonly string sfxVol = "sfxVol";
    public static readonly string soundOn = "mute";
    public static readonly float defaultVol = 0f;

    public float MusicVol
    {
        get
        {
            if (masterMixer.GetFloat(musicVol, out float volume))
                return volume;
            else
                return float.MaxValue;
        }
        set
        {
            masterMixer.SetFloat(musicVol, value);
            PlayerPrefs.SetFloat(musicVol, value);
            GameManager.Instance.uiManager.musicVol.value = value;
        }
    }

    public float SfxVol
    {
        get
        {
            if (masterMixer.GetFloat(sfxVol, out float volume))
                return volume;
            else
                return float.MaxValue;
        }
        set
        {
            masterMixer.SetFloat(sfxVol, value);
            PlayerPrefs.SetFloat(sfxVol, value);
            GameManager.Instance.uiManager.sfxVol.value = value;
        }
    }

    public bool SoundOn
    {
        get
        {
            if (masterMixer.GetFloat(masterVol, out float volume))
                return volume != -80f;
            else
                return false;
        }
        set
        {
            masterMixer.SetFloat(masterVol, value ? 0f : -80f);
            PlayerPrefs.SetInt(soundOn, value ? 1 : 0);
            GameManager.Instance.uiManager.soundOn.isOn = value;
        }
    }

    private void Start()
    {
        SoundOn = PlayerPrefs.GetInt(soundOn, 1) != 0 ? true : false;
        MusicVol = PlayerPrefs.GetFloat(musicVol, defaultVol);
        SfxVol = PlayerPrefs.GetFloat(sfxVol, defaultVol);
    }
}
