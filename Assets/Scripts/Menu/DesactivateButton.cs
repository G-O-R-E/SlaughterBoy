using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesactivateButton : MonoBehaviour
{
    [SerializeField] GameObject condition;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (button != null)
        {
            if (button.interactable && condition.activeInHierarchy) 
            {
                button.interactable = false;
            }
            else if (!button.interactable && !condition.activeInHierarchy)
            {
                button.interactable = true;
            }
        }
    }
}
