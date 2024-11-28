using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    private TextMeshProUGUI text;
    private int wave;
    // Start is called before the first frame update
    void Start()
    {
        wave = GameManager.instance.GetDataGame().waves;
        text = GetComponent<TextMeshProUGUI>();
        if (wave > 20)
        {
            text.text = "Victory ";

        }
        else
        {
            text.text = "Defeat ";
        }
    }
}
