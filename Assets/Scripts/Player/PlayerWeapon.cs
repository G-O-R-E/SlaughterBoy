using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] GameObject prefabWeapon;
    List<GameObject> weapons = new List<GameObject>();
    int nbWeapons = 0;

    void Awake()
    {
        AddRangeWeapon(prefabWeapon);
    }

    public void AddRangeWeapon(GameObject _weapon)
    {
        GameObject weapon;
        weapon = Instantiate(_weapon, transform.position, transform.rotation);
        weapon.transform.parent = transform.GetChild(1).transform;
        weapon.GetComponent<Weapon>().InitializeWeapon(nbWeapons);
        nbWeapons++;
        weapons.Add(weapon);
    }

    public void AddMeleeWeapon(GameObject _weapon)
    {
        GameObject weapon;
        weapon = Instantiate(_weapon, transform.position, transform.rotation);
        weapon.transform.parent = transform.GetChild(1).transform;
        weapon.GetComponent<MeleeWeapon>().InitializeWeapon(nbWeapons);
        nbWeapons++;
        weapons.Add(weapon);
    }

    public void AddFlameThrower(GameObject _weapon)
    {
        GameObject weapon;
        weapon = Instantiate(_weapon, transform.position, transform.rotation);
        weapon.transform.parent = transform.GetChild(1).transform;
        weapon.GetComponent<FlameThrower>().InitializeWeapon(nbWeapons);
        nbWeapons++;
        weapons.Add(weapon);
    }

    public GameObject GetWeapon(int _id)
    {
        return weapons[_id];
    }

    public List<GameObject> GetWeapons()
    {
        return weapons;
    }

    public void ResetIDs()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].TryGetComponent<Weapon>(out Weapon component);
            if (component == null)
            {
                weapons[i].GetComponent<MeleeWeapon>().InitializeWeapon(i);
            }
            else
            {
                weapons[i].GetComponent<Weapon>().InitializeWeapon(i);
            }
        }
    }
}
