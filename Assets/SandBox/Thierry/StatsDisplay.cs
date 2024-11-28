using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Stats;

public class StatsDisplay : MonoBehaviour
{
    private float[] playerStats;
    private Stats playerStatsInstance;
    private TextMeshProUGUI[] allText = new TextMeshProUGUI[(int)(PlayerStat.NbStats)];
    void Start()
    {
        PlayerStat playerStatEnum;
        playerStatsInstance = GameObject.Find("Player").GetComponentInChildren<Stats>();
        playerStats = playerStatsInstance.GetStatsTab();
        for (int i = 0; i < playerStats.Length; i++) 
        {
          playerStatEnum = (PlayerStat)i;
          allText[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
          allText[i].text = playerStatEnum.ToString() + " : " + MathF.Abs(playerStats[i]);   
        }
    }


    void Update()
    {
    }
}
