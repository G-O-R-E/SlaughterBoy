using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] int nbAudioSource;
    private AudioSource[] source;

    private void Start()
    {
        source = new AudioSource[nbAudioSource];
        for (int i = 0; i < nbAudioSource; i++)
        {
            source[i] = gameObject.AddComponent<AudioSource>();
            source[i].playOnAwake = false;
            source[i].loop = false;
        }
    }

    public void PlaySound(AudioClip clip,AudioMixerGroup mixerGroup)
    {
        for (int i = 0; i < nbAudioSource; i++)
        {
            if (!source[i].isPlaying)
            {
                source[i].clip = clip;
                source[i].outputAudioMixerGroup = mixerGroup;
                source[i].Play();
                return;
            }
        }
    }
}