using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [Header("Volume Control")]
    [SerializeField]
    [Range(0.0001f, 1f)]
    public float player = 1.0f;

    [SerializeField]
    [Range(0.0001f, 1f)]
    public float background = 1.0f;

    [SerializeField] AudioMixer mixer;

    private int currentAudioSourceIndex = 0;
    private List<AudioSource> audioSources;
    private AudioSource backgroundSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        audioSources = new List<AudioSource>();
        for (int i = 0; i < 5; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(source);
        }

        backgroundSource = gameObject.AddComponent<AudioSource>();

        SetAudioMixerVolume();
    }

    private void SetAudioMixerVolume()
    {
        if (mixer != null)
        {
            mixer.SetFloat("playerVolume", Mathf.Log10(player) * 20);
            mixer.SetFloat("backgroundVolume", Mathf.Log10(background) * 20);
        }
    }

    public void PlayRandomSound(SoundData soundData)
    {
        bool empty = CheckIfSoundDataEmpty(soundData);
        if (empty) return;

        SoundData.SoundEntry randomEntry = soundData.sounds[Random.Range(0, soundData.sounds.Length)];

        PlayClip(randomEntry.clip, randomEntry.loop, randomEntry.mixer);
    }

    private bool CheckIfSoundDataEmpty(SoundData soundData)
    {
        if (soundData == null || soundData.sounds.Length == 0)
        {
            Debug.LogWarning("SoundData is null or contains no sounds.");
            return true;
        }
        return false;
    }

    public void PlayBackgroundMusic(SoundData soundData)
    {
        bool empty = CheckIfSoundDataEmpty(soundData);
        if (empty) return;

        SoundData.SoundEntry randomEntry = soundData.sounds[Random.Range(0, soundData.sounds.Length)];

        if (backgroundSource.isPlaying)
        {
            backgroundSource.Stop();
        }
        backgroundSource.clip = randomEntry.clip;
        backgroundSource.loop = randomEntry.loop;
        backgroundSource.outputAudioMixerGroup = randomEntry.mixer;

        backgroundSource.Play();
    }

    private void PlayClip(AudioClip clip, bool loop, AudioMixerGroup mixer)
    {

        AudioSource currentSource = audioSources[currentAudioSourceIndex];

        currentSource.clip = clip;
        currentSource.loop = loop;
        currentSource.outputAudioMixerGroup = mixer;

        currentSource.Play();

        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % audioSources.Count;
    }

    private void OnValidate()
    {
        SetAudioMixerVolume();
    }
}