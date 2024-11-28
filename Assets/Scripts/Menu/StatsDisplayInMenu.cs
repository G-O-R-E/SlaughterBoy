using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Stats;

public class StatsDisplayInMenu : MonoBehaviour
{
    private float[] playerStats;
    private TextMeshProUGUI[] allText = new TextMeshProUGUI[(int)(PlayerStat.NbStats)];
    [SerializeField] bool isVisible = true;
    private Vector3 pos;
    void Start()
    {
        PlayerStat playerStatEnum;
        playerStats = GameManager.instance.GetDataPlayer().stats;
        pos = transform.position;

        for (int i = 0; i < playerStats.Length; i++)
        {
            playerStatEnum = (PlayerStat)i;
            allText[i] = transform.GetChild(0).GetChild(i).GetComponent<TextMeshProUGUI>();
            allText[i].text = playerStatEnum.ToString() + " : " + MathF.Abs(playerStats[i]);
        }
    }


    void Update()
    {
        if (!isVisible)
        {
            transform.position = -pos;
        }
        else
        {
            transform.position = pos;
        }
        //OverlapeTextFinger();
    }

    public void UpdateText()
    {
        PlayerStat playerStatEnum;

        for (int i = 0; i < playerStats.Length; i++)
        {
            playerStatEnum = (PlayerStat)i;
            allText[i].text = playerStatEnum.ToString() + " : ";

            if (playerStats[i] == 0)
            {
                allText[i].text += MathF.Abs(playerStats[i]);
            }
            else if (playerStats[i] < 0)
            {
                allText[i].text += "<color=red>" + playerStats[i] + "</color>";
            }
            else
            {
                allText[i].text += "<color=green>" + playerStats[i] + "</color>";
            }
        }
    }

    public bool GetIsVisible()
    {
        return isVisible;
    }

    public void InverseIsVisible()
    {
        isVisible = !isVisible;
    }

    void OverlapeTextFinger()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 canvasMousePosition;

        for (int i = 0; i < allText.Length; i++)
        {
            RectTransform canvasRectTransform = allText[i].GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, null, out canvasMousePosition);
            Rect textRect = allText[i].rectTransform.rect;
            if (textRect.Contains(canvasMousePosition))
            {
                Debug.Log(i);
                allText[i].GetComponentInChildren<UnityEngine.UI.Image>().GameObject().SetActive(true);
            }
            else
            {
                allText[i].GetComponentInChildren<UnityEngine.UI.Image>().GameObject().SetActive(false);
            }
        }
    }
}
