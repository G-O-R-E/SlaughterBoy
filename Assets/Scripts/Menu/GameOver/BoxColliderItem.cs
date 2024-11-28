using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderItem : MonoBehaviour
{
    RectTransform[] childTransform;
    RectTransform containerTransform;
    RectTransform thisRectTransform;
    const byte NB_ITEM_DISPLAYABLE = 6;
    public bool reload = false;
    public bool reloadForChild = false;
    Dictionary<GameObject, int> itemDic;
    float divider;

    void Start()
    {
        RefreshItemList();
    }

    private void Update()
    {
        if (reload)
        {
            reload = false;
            RefreshItemList();
            reloadForChild = true;
        }
        if (itemDic.Count > NB_ITEM_DISPLAYABLE)
        {
            if (childTransform[0].position.x - childTransform[0].rect.width / 2f > thisRectTransform.position.x)
            {
                containerTransform.position = new Vector3(thisRectTransform.position.x + 30f, thisRectTransform.position.y);
            }
            else if (childTransform[itemDic.Count - 1].position.x + childTransform[0].rect.width / 2f < thisRectTransform.position.x + thisRectTransform.rect.width)
            {
                containerTransform.position = new Vector3(thisRectTransform.position.x - childTransform[0].rect.width * itemDic.Count / divider, containerTransform.position.y);

            }
        }
        else
        {
            containerTransform.position = new Vector3(thisRectTransform.position.x + 30f, thisRectTransform.position.y, thisRectTransform.position.z);
        }
    }

    private void RefreshItemList()
    {
        int value = 0;
        itemDic = new Dictionary<GameObject, int>();
        for (int i = 0; i < GameManager.instance.GetDataPlayer().items.Count; i++)
        {
            if (itemDic.TryGetValue(GameManager.instance.GetDataPlayer().items[i].item, out value) == false)
            {
                itemDic.Add(GameManager.instance.GetDataPlayer().items[i].item, 1);
            }
            else
            {
                itemDic.TryGetValue(GameManager.instance.GetDataPlayer().items[i].item, out value);
                value++;
            }
        }


        childTransform = new RectTransform[itemDic.Count];
        int j = 0;
        foreach (var item1 in itemDic)
        {
            childTransform[j] = transform.GetChild(0).GetChild(j).GetComponent<RectTransform>();
            j++;
        }

        containerTransform = transform.GetChild(0).GetComponent<RectTransform>();
        thisRectTransform = gameObject.GetComponent<RectTransform>();

        //Magic Number
        switch (itemDic.Count)
        {
            case 15:
                divider = 1.45f;
                break;
            case 14:
                divider = 1.55f;
                break;
            case 13:
                divider = 1.62f;
                break;
            case 12:
                divider = 1.76f;
                break;
            case 11:
                divider = 1.9f;
                break;
            case 10:
                divider = 2.17f;
                break;
            case 9:
                divider = 2.6f;
                break;
            case 8:
                divider = 3.5f;
                break;
            case 7:
                divider = 5.5f;
                break;
            default: 
                break;

        }
    }

    public void SetReload()
    {
        reload = true;
    }
}
