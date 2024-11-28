using UnityEngine;

public class CombineWeapon : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] int sameWeapon;
    [SerializeField] BoxColliderWeapon viewer;
    [SerializeField] int indexThis;
    [SerializeField] GameObject button;
    [SerializeField] SelectorsManager selectorsManager;

    public void CombineTwoWeapon()
    {
        GameManager.instance.GetDataPlayer().weapons[sameWeapon].weapon = null;
        GameManager.instance.GetDataPlayer().weapons[sameWeapon].rarity = 0;

        GameManager.instance.GetDataPlayer().weapons[indexThis].rarity++;

        viewer.SetReload(sameWeapon);
        selectorsManager.refreshCombinable = true;
        button.SetActive(false);
    }

    public void CanCombineWeapon(int otherWeapon)
    {
        sameWeapon = otherWeapon;
        button.SetActive(true);
    }

    public void DesactivateCombine()
    {
        button.SetActive(false);
    }
}