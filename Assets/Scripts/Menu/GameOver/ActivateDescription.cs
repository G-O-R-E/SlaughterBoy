using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class ActivateDescription : MonoBehaviour
{
    RectTransform rect;
    
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
       
    }

    // Update is called once per frame
    void Update()
    {
        OverlapeTextFinger();
    }

    void OverlapeTextFinger()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 canvasMousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, mousePosition, null, out canvasMousePosition);
        Rect textRect = rect.rect;
        if (textRect.Contains(canvasMousePosition))
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }

    }
}
