using UnityEngine;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour
{
    [Header("Sound Parameter")]
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioMixerGroup mixerGroup;
    private AudioSource source;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.outputAudioMixerGroup = mixerGroup;
        source.loop = false;
    }

    public void PlayStep()
    {
        int rndStepIndex = Random.Range(0,clips.Length);
        source.clip = clips[rndStepIndex];
        source.Play();
    }
}