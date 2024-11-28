using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed;
    private TextMeshProUGUI text;
    private bool isFadeIn = false;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        text.alpha += Time.deltaTime * speed;

        if (text.alpha >= 1) 
        {
            this.isFadeIn = false;
        }
    }

    private void FadeOut()
    {
        text.alpha -= Time.deltaTime * speed;

        if (text.alpha <= 0)
        {
            this.isFadeIn = true;
        }
    }
}
