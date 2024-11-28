using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI TextCoinsCount;

    [SerializeField]
    TMPro.TextMeshProUGUI WaveCount;

    [SerializeField]
    Type[] lootableObjects;

    [SerializeField]
    GameObject panelStats;

    // Start is called before the first frame update
    void Start()
    {
        TextCoinsCount.text = GameManager.instance.GetDataPlayer().money.ToString();
        WaveCount.text = "Wave " + GameManager.instance.GetDataGame().waves.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TextCoinsCount.text = GameManager.instance.GetDataPlayer().money.ToString();
    }

    public void ActivePanelStats()
    {
        panelStats.SetActive(!panelStats.activeSelf);
    }
}
