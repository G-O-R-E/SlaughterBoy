using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class ShopItem
{
    [SerializeField] public string name;
    [SerializeField] public GameObject go;
}

public class SelectorsManager : MonoBehaviour
{
    [Header("Items & Weapons")]
    [SerializeField] public List<ShopItem> itemsWeapons;
    [SerializeField] GameObject[] panelSelectors;
    [SerializeField] GameObject prefabLootable;
    [SerializeField] TextMeshProUGUI priceRefreshText;
    [SerializeField] int priceRefresh;
    [SerializeField] public bool refreshCombinable;

    [Header("Sound Parameter")]
    [SerializeField] AudioMixerGroup group;
    [SerializeField] AudioClip[] clip;
    private AudioSource source;
    private enum Sound
    {
        Purshase,
        Lock,
    }

    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
        source.loop = false;

        priceRefreshText.text = priceRefresh.ToString();
        ChooseLootables();
        UpdatePricesColor();
    }

    private void ChooseLootables()
    {
        for (int i = 0; i < panelSelectors.Length; i++)
        {
            Random.Range(0, itemsWeapons.Count);

            GameObject prefabs = itemsWeapons[Random.Range(0, itemsWeapons.Count)].go;
            GameObject.Instantiate(prefabs, panelSelectors[i].transform.GetChild(0).transform);
        }
    }
    public void RefreshLootables()
    {
        if (GameManager.instance.GetDataPlayer().money >= priceRefresh)
        {
            //sound
            source.clip = clip[(int)Sound.Purshase];
            source.Play();

            GameManager.DataPlayer dataTemp = GameManager.instance.GetDataPlayer();
            dataTemp.money -= priceRefresh;
            GameManager.instance.SetValueDataPlayer(dataTemp);

            priceRefresh += 5;
            priceRefreshText.text = priceRefresh.ToString();

            // Destroy and create new Object Selector
            for (int i = 0; i < panelSelectors.Length; i++)
            {
                if (panelSelectors[i] != null)
                {
                    DestroyImmediate(panelSelectors[i].gameObject);
                }
                panelSelectors[i] = GameObject.Instantiate(prefabLootable, transform);
            }

            ChooseLootables();
            UpdatePricesColor();
        }
        else
        {
            //sound
            source.clip = clip[(int)Sound.Lock];
            source.Play();
        }
    }

    public void UpdatePricesColor()
    {
        for (int i = 0; i < panelSelectors.Length; i++)
        {
            if (panelSelectors[i] != null)
            {
                panelSelectors[i].GetComponent<ObjectSelector>().UpdateLootablePriceColor();
            }
        }
    }

    private void Update()
    {
        if (refreshCombinable)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<ObjectSelector>().CombineWeapon();
            }
            refreshCombinable = false;
        }
    }
}