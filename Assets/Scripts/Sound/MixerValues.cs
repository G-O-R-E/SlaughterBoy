using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerValues : MonoBehaviour
{

    [SerializeField] AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        //get the value in global
        float valueDecibelsMaster = PlayerPrefs.GetFloat("Master");
        float valueDecibelsMusic = PlayerPrefs.GetFloat("Music");
        float valueDecibelsFx = PlayerPrefs.GetFloat("Fx");
        float valueDecibelsUI = PlayerPrefs.GetFloat("UI");

        //reinit the audioMixer on good group
        this.audioMixer.SetFloat("Master", valueDecibelsMaster);
        this.audioMixer.SetFloat("Music", valueDecibelsMusic);
        this.audioMixer.SetFloat("Fx", valueDecibelsFx);
        this.audioMixer.SetFloat("UI", valueDecibelsUI);
    }
}
