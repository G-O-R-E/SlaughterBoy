using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] new string name;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image muteButton;
    [SerializeField] Sprite[] muteSprite;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        //get the value in global
        float valueDecibels = PlayerPrefs.GetFloat(name);

        //reinit the audioMixer on good group
        this.audioMixer.SetFloat(name, valueDecibels);

        //transform the value in percent
        valueDecibels = Mathf.Pow(10,(valueDecibels / 20))*100;
        slider.value = valueDecibels;

        if (slider.value != 0)
        {
            muteButton.sprite = muteSprite[0];
        }
        else
        {
            muteButton.sprite = muteSprite[1];
        }
    }

    public void SetVolumeLevel(float sliderValue)
    {
        if (sliderValue != 0)
        {
            //set in decibels in audio mixer
            this.audioMixer.SetFloat(this.name, Mathf.Log10(sliderValue/100) * 20);
        }
        else
        {

            this.audioMixer.SetFloat(this.name, -80f);
        }

        //set value in UI
        float sliderValuePercent = Mathf.RoundToInt(sliderValue);
        text.text = sliderValuePercent.ToString();

        //set the value in global
        float valueDecibels;
        this.audioMixer.GetFloat(name, out valueDecibels);
        PlayerPrefs.SetFloat(name, valueDecibels);

        if (slider.value != 0)
        {
            muteButton.sprite = muteSprite[0];
        }
        else
        {
            muteButton.sprite = muteSprite[1];
        }
    }

    public void MuteUnMuteVolume()
    {
        float value;
        this.audioMixer.GetFloat(this.name, out value);
        
        if (value == -80f)
        {
            this.audioMixer.SetFloat(this.name, 0f);
            slider.value = 100f;
        }
        else
        {
            this.audioMixer.SetFloat(this.name, -80f);
            slider.value = 0f;
        }

        //set the value in global
        float valueDecibels;
        this.audioMixer.GetFloat(name, out valueDecibels);
        PlayerPrefs.SetFloat(name, valueDecibels);


        if (slider.value != 0)
        {
            muteButton.sprite = muteSprite[0];
        }
        else
        {
            muteButton.sprite = muteSprite[1];
        }
    }
}