using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static RarityManager;

public class ActivateSeller : MonoBehaviour
{
    [SerializeField] GameObject[] weaponsPrefabs;
    [SerializeField] TextMeshProUGUI[] textMeshes;
    [SerializeField] SelectorsManager selectorManager;

    private GameObject[] allWeapons;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {

        allWeapons = new GameObject[6];
        for (int i = 0; i < allWeapons.Length; i++)
        {
            allWeapons[i] = transform.GetChild(i).gameObject;

        }
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

        for (int i = 0; i < allWeapons.Length; i++)
        {
            RectTransform canvasRectTransform = allWeapons[i].transform.GetChild(1).GetComponent<RectTransform>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, null, out canvasMousePosition);
            if (canvasRectTransform.rect.Contains(canvasMousePosition) && allWeapons[i].transform.GetChild(1).GetComponent<Image>().sprite != null)
            {
                allWeapons[i].transform.GetChild(0).gameObject.SetActive(true);
                index = i;
                InitDescriptionTexts(allWeapons[i].transform.GetChild(0).gameObject);
            }
            else
            {
                StartCoroutine(WaitBeforeClose(i));
            }
        }
    }

    IEnumerator WaitBeforeClose(int i)
    {
        yield return new WaitForSeconds(0.2f);
        allWeapons[i].transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Sell()
    {
        GameManager.DataPlayer player = GameManager.instance.GetDataPlayer();
        foreach (var weapon in weaponsPrefabs)
        {
            RarityManager.Rarity rarity = (RarityManager.Rarity)player.weapons[index].rarity;
            float rarityFactor = (int)rarity * RarityManager.instance.rarityModifier + 1;

            if (weapon.name == player.weapons[index].weapon.name)
            {
                player.money += (int)(weapon.GetComponent<Shop>().price * rarityFactor) / 2;
                break;
            }
        }
        player.weapons[index].weapon = null;
        player.weapons[index].rarity = 0;


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

        GameManager.instance.SetValueDataPlayer(player);
        selectorManager.UpdatePricesColor();
    }

    void InitDescriptionTexts(GameObject go)
    {
        GameManager.DataPlayer player = GameManager.instance.GetDataPlayer();
        foreach (var weapon in weaponsPrefabs)
        {
            if (weapon.name == player.weapons[index].weapon.name)
            {
                RarityManager.Rarity rarity = (RarityManager.Rarity)player.weapons[index].rarity;
                float rarityFactor = (int)rarity * RarityManager.instance.rarityModifier + 1;

                WeaponPrefabs weaponInfo = GameManager.instance.weaponPrefabs.Find(w => w.name == weapon.name + "(Clone)");

                if (weaponInfo.prefabs.TryGetComponent<Weapon>(out Weapon weap))
                {
                    go.transform.GetChild(0).Find("Range").GetComponent<TextMeshProUGUI>().text = "Range : <color=green>" + ((int)(rarityFactor * weap.baseRange)).ToString() + "</color>";
                    go.transform.GetChild(0).Find("Damage").GetComponent<TextMeshProUGUI>().text = "Damage : <color=green>" + ((int)(rarityFactor * weap.baseDamage)).ToString() + "</color>";
                    go.transform.GetChild(0).Find("Cooldown").GetComponent<TextMeshProUGUI>().text = "Cooldown : <color=green>" + (weap.baseWeaponCooldown / rarityFactor).ToString() + "</color>";
                }
                else if (weaponInfo.prefabs.TryGetComponent<MeleeWeapon>(out MeleeWeapon meleeWeap))
                {
                    go.transform.GetChild(0).Find("Range").GetComponent<TextMeshProUGUI>().text = "Range : <color=green>" + ((int)(rarityFactor * meleeWeap.baseRange)).ToString() + "</color>";
                    go.transform.GetChild(0).Find("Damage").GetComponent<TextMeshProUGUI>().text = "Damage : <color=green>" + ((int)(rarityFactor * meleeWeap.baseDamage)).ToString() + "</color>";
                    go.transform.GetChild(0).Find("Cooldown").GetComponent<TextMeshProUGUI>().text = "Cooldown : <color=green>" + (meleeWeap.baseWeaponCooldown / rarityFactor).ToString() + "</color>";
                }
                else if (weaponInfo.prefabs.TryGetComponent<FlameThrower>(out FlameThrower flameThrower))
                {
                    float cooldown = 0.1f;
                    go.transform.GetChild(0).Find("Range").GetComponent<TextMeshProUGUI>().text = "Range : <color=green>" + ((int)(rarityFactor * flameThrower.baseRange)).ToString() + "</color>";
                    go.transform.GetChild(0).Find("Damage").GetComponent<TextMeshProUGUI>().text = "Damage : <color=green>" + ((int)(rarityFactor * flameThrower.baseDamage)).ToString() + "</color>";
                    go.transform.GetChild(0).Find("Cooldown").GetComponent<TextMeshProUGUI>().text = "Cooldown : <color=green>" + cooldown.ToString() + "</color>";
                }
                go.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Sell : " + (int)(weapon.GetComponent<Shop>().price * rarityFactor) / 2;
            }
        }
    }
}
