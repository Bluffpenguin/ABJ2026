using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [System.Serializable]
    public struct SFXClip
    {
        public AudioClip file;
        public float upperPitch; // The upper pitch range for a sfx
        public float lowerPitch; // The lower pitch range for a sfx
    }
    [Header("Audio Clips")]
    public AudioClip music_mainTheme;
    public SFXClip sfx_buttonClick;

    [Header("SFX Volumes")]
    public float jumpVolume = 0.45f;
    public float coinVolume = 0.3f;


	private void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
	}

#if UNITY_EDITOR
	private void Update()
	{
        
	}
#endif


	public void PlayMusic(AudioClip track)
    {
        musicSource.clip = track;
        musicSource.Play();
    }

    public AudioClip GetCurrentMusicClip()
    {
        return musicSource.clip;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(SFXClip clip, float volume = 1f)
    {
        if (SFXSource == null || clip.file == null) return;

        SFXSource.pitch = Random.Range(clip.lowerPitch, clip.upperPitch);
        SFXSource.PlayOneShot(clip.file, volume);
    }
}
