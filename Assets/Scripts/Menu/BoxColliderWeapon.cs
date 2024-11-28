using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxColliderWeapon : MonoBehaviour
{
    Transform[] weapons;
    RectTransform[] childTransform;
    RectTransform containerTransform;
    RectTransform thisRectTransform;
    public bool reload = false;
    public bool reloadForChild = false;
    int weaponLenght;
    int index = -1;

    void Start()
    {
        RefreshWeaponList();
    }

    private void Update()
    {

        if (reload)
        {
            RefreshWeaponList();
            reloadForChild = true;
            reload = false;
        }
        containerTransform.position = thisRectTransform.position;

    }

    private void RefreshWeaponList()
    {
        if (index != -1)
        {
            GameManager.DataPlayer player = GameManager.instance.GetDataPlayer();
            WeaponPlayer[] allWeapons = GameManager.instance.GetDataPlayer().weapons;
            for (int i = index; i < allWeapons.Length; i++)
            {
                if (i < allWeapons.Length - 1)
                {
                    player.weapons[i].weapon = player.weapons[i + 1].weapon;
                    player.weapons[i].rarity = player.weapons[i + 1].rarity;
                }
                else
                {
                    player.weapons[i].weapon = null;
                    player.weapons[i].rarity = 0;
                }
            }
        }

        weaponLenght = 0;
        for (int i = 0; i < GameManager.instance.GetDataPlayer().weapons.Length; i++)
        {
            if (GameManager.instance.GetDataPlayer().weapons[i].weapon != null)
            {
                weaponLenght++;
            }
        }
        weapons = new Transform[weaponLenght];
        childTransform = new RectTransform[weaponLenght];
        for (int i = 0; i < weaponLenght; i++)
        {
            weapons[i] = GameManager.instance.GetDataPlayer().weapons[i].weapon.transform;
            childTransform[i] = transform.GetChild(0).GetChild(i).GetComponent<RectTransform>();
        }


        containerTransform = transform.GetChild(0).GetComponent<RectTransform>();
        thisRectTransform = gameObject.GetComponent<RectTransform>();
        index = -1;

        EneableCombinable();
    }

    public void SetReload()
    {
        reload = true;
        this.index = -1;
    }
    public void SetReload(int index)
    {
        reload = true;
        this.index = index;
    }

    private void EneableCombinable()
    {
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<CombineWeapon>().DesactivateCombine();
        }

        Dictionary<string, int[]> keyValuePairs = new Dictionary<string, int[]>();
        for (int i = 0; i < GameManager.instance.GetDataPlayer().weapons.Length; i++)
        {
            if (GameManager.instance.GetDataPlayer().weapons[i].weapon != null && GameManager.instance.GetDataPlayer().weapons[i].rarity != (int)RarityManager.Rarity.Legendary)
            {
                if (!keyValuePairs.ContainsKey(GameManager.instance.GetDataPlayer().weapons[i].weapon.name + GameManager.instance.GetDataPlayer().weapons[i].rarity.ToString()))
                {
                    int[] index = new int[2] { -1, -1 };
                    index[0] = i;

                    keyValuePairs.Add(GameManager.instance.GetDataPlayer().weapons[i].weapon.name + GameManager.instance.GetDataPlayer().weapons[i].rarity.ToString(), index);
                }
                else
                {
                    if (keyValuePairs[GameManager.instance.GetDataPlayer().weapons[i].weapon.name + GameManager.instance.GetDataPlayer().weapons[i].rarity.ToString()][1] == -1)
                    {
                        keyValuePairs[GameManager.instance.GetDataPlayer().weapons[i].weapon.name + GameManager.instance.GetDataPlayer().weapons[i].rarity.ToString()][1] = i;
                    }
                    else
                    {
                        //print("oups");
                    }
                }
            }
        }

        foreach (var value in keyValuePairs)
        {
            if (value.Value[0] != -1 && value.Value[1] != -1)
            {
                if (transform.GetChild(0).GetChild(value.Value[0]).GetChild(0).TryGetComponent<CombineWeapon>(out CombineWeapon combineWeaponFirst) &&
                    transform.GetChild(0).GetChild(value.Value[1]).GetChild(0).TryGetComponent<CombineWeapon>(out CombineWeapon combineWeaponSecond))
                {
                    //print("good");
                    combineWeaponFirst.CanCombineWeapon(value.Value[1]);
                    combineWeaponSecond.CanCombineWeapon(value.Value[0]);
                }
                else
                {
                    //print("not good");
                }
            }
        }

        //print("end");
    }
}