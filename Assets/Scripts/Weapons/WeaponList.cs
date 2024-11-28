using UnityEngine;
using UnityEngine.UI;

public class WeaponList : MonoBehaviour
{
    Transform[] weapons;
    Transform[] childTransform;
    private int weaponLenght;
    BoxColliderWeapon parent;

    void Start()
    {
        RefreshWeaponList();
        parent = GetComponentInParent<BoxColliderWeapon>();
    }
    private void Update()
    {
        if (parent.reloadForChild)
        {
            RefreshWeaponList();
            parent.reloadForChild = false;
        }


    }
    public void RefreshWeaponList()
    {

        weaponLenght = 0;
        for (int i = 0; i < GameManager.instance.GetDataPlayer().weapons.Length; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = null;
            transform.GetChild(i).GetComponent<Image>().color = Color.clear;
            transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = null;
            transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.clear;

            if (GameManager.instance.GetDataPlayer().weapons[i].weapon != null)
            {
                weaponLenght++;
            }
        }
        weapons = new Transform[weaponLenght];
        childTransform = new Transform[weaponLenght];

        for (int i = 0; i < weaponLenght; i++)
        {
            weapons[i] = GameManager.instance.GetDataPlayer().weapons[i].weapon.transform;
            transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = weapons[i].GetComponent<SpriteRenderer>().sprite;
            transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
            transform.GetChild(i).GetComponent<Image>().color = RarityManager.instance.colorRarity[GameManager.instance.GetDataPlayer().weapons[i].rarity];

            childTransform[i] = transform.GetChild(i).transform;
        }
    }
}
