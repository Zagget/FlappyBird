using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Sound Data", fileName = "New Sound Data")]
public class SoundData : ScriptableObject
{
    [System.Serializable]
    public class SoundEntry
    {
        public string name;
        public AudioClip clip;
        public bool loop = false;
        public AudioMixerGroup mixer;
    }

    public SoundEntry[] sounds;
}