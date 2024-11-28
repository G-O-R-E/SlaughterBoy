using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NbWaveText : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int wave;
    // Start is called before the first frame update
    void Start()
    {
        wave = GameManager.instance.GetDataGame().waves;
        text = GetComponent<TextMeshProUGUI>();

        text.text = "Wave : " + (wave -1);
    }
}
