using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{

    Transform[] childTransform;

    Dictionary<GameObject, int> itemDic;

    BoxColliderItem parent;
    void Start()
    {
        RefreshItemList();
        parent = GetComponentInParent<BoxColliderItem>();
    }

    private void Update()
    {
       if (parent.reloadForChild)
        {
            parent.reloadForChild = false;
            RefreshItemList();
        }
    }

    public void RefreshItemList()
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
                itemDic[GameManager.instance.GetDataPlayer().items[i].item] += 1;
            }
        }
        childTransform = new Transform[itemDic.Count];

        int j = 0;
        foreach (var item1 in itemDic)
        {
            transform.GetChild(j).GetComponent<Image>().sprite = item1.Key.GetComponent<Shop>().sprite;
            transform.GetChild(j).GetComponent<Image>().color = Color.white;
            transform.GetChild(j).GetComponent<Image>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "X" + item1.Value;
            childTransform[j] = transform.GetChild(j).transform;
            j++;
        }
    }
}
