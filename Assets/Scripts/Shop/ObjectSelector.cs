using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static GameManager;

public class ObjectSelector : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] TextMeshProUGUI textPrice;
    [SerializeField] TextMeshProUGUI[] textStats;
    [SerializeField] TextMeshProUGUI textType;
    [SerializeField] TextMeshProUGUI textName;
    [SerializeField] Image imageItems;
    [SerializeField] Image backItems;
    [SerializeField] GameObject imageCombinable;
    [SerializeField] RarityManager rarityManager;
    [SerializeField] SelectorsManager selectorManager;

    private Button buttonForFunctOnCLick;
    private GameObject prefabsItems;
    private Stats statsItem;
    private Shop shopItem;

    private int realPrice;
    private float rarityFactor;

    [Header("Weapon Rarity")]
    [SerializeField] bool isCombinable = false;
    [SerializeField] int indexInViewver = -1;


    [Header("Sound Parameter")]
    [SerializeField] AudioMixerGroup group;
    [SerializeField] AudioClip[] clip;
    private AudioSource source;

    private enum Sound
    {
        Purshase,
        Lock,
    }

    private void Start()
    {
        InitSound();
        InitButtonFunction();
        InitVariable();
        InitRarity();
        UpdateLootablePriceColor();
        InitTextDisplay();
    }

    private void InitVariable()
    {
        // get items
        prefabsItems = transform.GetChild(0).GetChild(0).gameObject;
        selectorManager = GetComponentInParent<SelectorsManager>();

        //parameter
        statsItem = prefabsItems.GetComponent<Stats>();
        shopItem = prefabsItems.GetComponent<Shop>();

        isCombinable = false;
    }

    private void InitRarity()
    {
        // Init Rarity
        shopItem.rarity = rarityManager.GiveARarity();

        //color rarity
        backItems.color = rarityManager.colorRarity[(int)shopItem.rarity];

        //change data on rarity
        rarityFactor = rarityManager.rarityModifier * (int)shopItem.rarity + 1;
        realPrice = (int)(shopItem.price * ((100 - GameManager.instance.GetDataPlayer().stats[11]) / 100) * rarityFactor);

        //new stat on rarity
        for (int i = 0; i < statsItem.GetStatsTab().Length; i++)
        {
            if (statsItem.GetStatsTab()[i] != 0)
            {
                statsItem.GetStatsTab()[i] *= rarityFactor;
                statsItem.GetStatsTab()[i] = (int)statsItem.GetStatsTab()[i];
            }
        }

        //new price on rarity
        shopItem.price = realPrice;
    }

    private void InitButtonFunction()
    {
        //init funct button onclick
        buttonForFunctOnCLick = GetComponent<Button>();

        transform.parent.TryGetComponent<SelectorsManager>(out SelectorsManager selectorsManager);
        transform.parent.parent.Find("StatsDisplay").TryGetComponent<StatsDisplayInMenu>(out StatsDisplayInMenu statsDisplayInMenu);
        transform.parent.parent.Find("WeaponViewer").GetChild(0).TryGetComponent<BoxColliderWeapon>(out BoxColliderWeapon boxColliderWeapon);
        transform.parent.parent.Find("ItemsViewer").GetChild(0).TryGetComponent<BoxColliderItem>(out BoxColliderItem boxColliderItem);

        buttonForFunctOnCLick.onClick.AddListener(selectorsManager.UpdatePricesColor);
        buttonForFunctOnCLick.onClick.AddListener(statsDisplayInMenu.UpdateText);
        buttonForFunctOnCLick.onClick.AddListener(boxColliderWeapon.SetReload);
        buttonForFunctOnCLick.onClick.AddListener(boxColliderItem.SetReload);
    }

    private void InitSound()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
        source.loop = false;
    }

    private void InitTextDisplay()
    {
        //associate prefabs on text
        textType.text = shopItem.type.ToString();
        textName.text = shopItem.nameItems.ToString();
        imageItems.sprite = shopItem.sprite;

        //text price
        textPrice.text = realPrice.ToString();

        //associate text with stats items
        if (shopItem.type == "Item")
        {
            InitItemsStatsDisplay();
        }
        //is as weapon
        else
        {
            InitWeaponStatsDisplay();
        }
    }

    private void InitItemsStatsDisplay()
    {
        int indexText = 0;
        for (int j = 0; j < statsItem.GetStatsTab().Length; j++)
        {
            if (statsItem.GetStatsTab()[j] != 0)
            {
                float value = statsItem.GetStatsTab()[j];
                textStats[indexText].text = ((Stats.PlayerStat)j).ToString() + " : ";
                if (value > 0)
                {
                    textStats[indexText].text += "<color=green>" + value.ToString() + "</color>";
                }
                else
                {
                    textStats[indexText].text += "<color=red>" + value.ToString() + "</color>";
                }
                indexText++;
            }
        }
    }

    private void InitWeaponStatsDisplay()
    {
        GameObject go = GetPrefabs();
        WeaponPrefabs weaponInfo = GameManager.instance.weaponPrefabs.Find(w => w.name == go.name + "(Clone)");
        if (weaponInfo.prefabs.TryGetComponent<Weapon>(out Weapon weap))
        {
            //change value on rarity
            int additionalR = (int)(weap.baseRange * rarityFactor);
            int additionalD = (int)(weap.baseDamage * rarityFactor);
            float additionalC = weap.baseWeaponCooldown / rarityFactor;

            textStats[1].text = "Range : <color=green>" + additionalR.ToString() + "</color>";
            textStats[0].text = "Damage : <color=green>" + additionalD.ToString() + "</color>";
            textStats[2].text = "Cooldown : <color=green>" + additionalC.ToString() + " sec</color>";
        }
        else if (weaponInfo.prefabs.TryGetComponent<MeleeWeapon>(out MeleeWeapon meleeWeap))
        {
            //change value on rarity
            int additionalR = (int)(meleeWeap.baseRange * rarityFactor);
            int additionalD = (int)(meleeWeap.baseDamage * rarityFactor);
            float additionalC = meleeWeap.baseWeaponCooldown / rarityFactor;

            textStats[1].text = "Range : <color=green>" + additionalR.ToString() + "</color>";
            textStats[0].text = "Damage : <color=green>" + additionalD.ToString() + "</color>";
            textStats[2].text = "Cooldown : <color=green>" + additionalC.ToString() + " sec</color>";
        }
        else if (weaponInfo.prefabs.TryGetComponent<FlameThrower>(out FlameThrower flameThrower))
        {
            //change value on rarity
            int additionalR = (int)(flameThrower.baseRange * rarityFactor);
            int additionalD = (int)(flameThrower.baseDamage * rarityFactor);
            float additionalC = 0.1f;

            textStats[1].text = "Range : <color=green>" + additionalR.ToString() + "</color>";
            textStats[0].text = "Damage : <color=green>" + additionalD.ToString() + "</color>";
            textStats[2].text = "Cooldown : <color=green>" + additionalC.ToString() + " sec</color>";
        }

        CombineWeapon();
    }

    public void PurshaseItem()
    {
        //check money
        if (GameManager.instance.GetDataPlayer().money >= realPrice)
        {
            //sound
            source.clip = clip[(int)Sound.Purshase];
            source.Play();

            GameManager.DataPlayer test = GameManager.instance.GetDataPlayer();

            // Add item to the player
            if (!AddItem(test))
            {
                //////////////////// Combine Weapon start ///////////////
                if (isCombinable && shopItem.type != "Item")
                {
                    EnhanceRarityWeapon(test);
                }
                //////////////////// Combine Weapon  end ///////////////
                return;
            }

            if (shopItem.type == "Item")
            {
                //add stats
                for (int i = 0; i < GameManager.instance.GetDataPlayer().stats.Length; i++)
                {
                    GameManager.instance.GetDataPlayer().stats[i] += statsItem.GetStatsTab()[i];
                }
            }

            //remove gold
            RemoveGold(test);

            //play fx gold
            GameObject.Find("VFX_CoinShop").GetComponent<ParticleSystem>().Play();

            //destroy gameObject
            DestroyImmediate(gameObject);

            selectorManager.refreshCombinable = true;
        }
        else
        {
            //sound
            source.clip = clip[(int)Sound.Lock];
            source.Play();
        }
    }

    private bool AddItem(DataPlayer test)
    {
        GameObject go = GetPrefabs();
        if (go != null)
        {
            if (shopItem.type == "Item")
            {
                ItemPlayer item = new ItemPlayer();
                item.item = go;
                item.rarity = (int)shopItem.rarity;

                test.items.Add(item);
            }
            else
            {
                for (int i = 0; i < test.weapons.Length; i++)
                {
                    if (test.weapons[i].weapon == null)
                    {
                        WeaponPrefabs weaponInfo = GameManager.instance.weaponPrefabs.Find(w => w.name == go.name + "(Clone)");

                        WeaponPlayer weap = new WeaponPlayer();
                        weap.weapon = weaponInfo.prefabs;
                        weap.rarity = (int)shopItem.rarity;
                        test.weapons[i] = weap;
                        return true;
                    }
                }
                return false;
            }
            return true;
        }
        Debug.LogError("pb prefabs est pas le même dans le nom et le dico");

        return false;
    }

    private GameObject GetPrefabs()
    {
        SelectorsManager manager = GetComponentInParent<SelectorsManager>();
        foreach (var item in manager.itemsWeapons)
        {
            if (item.name + "(Clone)" == prefabsItems.name)
            {
                return item.go;
            }
        }
        return null;
    }

    public void UpdateLootablePriceColor()
    {
        if (realPrice > GameManager.instance.GetDataPlayer().money)
        {
            textPrice.color = Color.red;
        }
        else
        {
            textPrice.color = Color.white;
        }
    }

    private bool CheckCombineWeapon(DataPlayer test)
    {
        for (int i = 0; i < test.weapons.Length; i++)
        {
            if (test.weapons[i].weapon != null)
            {
                if (test.weapons[i].rarity < (int)RarityManager.Rarity.Legendary && shopItem.rarity != RarityManager.Rarity.Legendary)
                {
                    if (test.weapons[i].weapon.name + "(Clone)" == shopItem.name && test.weapons[i].rarity == (int)shopItem.rarity)
                    {
                        indexInViewver = i;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void CombineWeapon()
    {
        GameManager.DataPlayer tempData = GameManager.instance.GetDataPlayer();
        if (CheckCombineWeapon(tempData))
        {
            isCombinable = true;
            imageCombinable.SetActive(true);
        }
    }

    private void EnhanceRarityWeapon(DataPlayer data)
    {
        data.weapons[indexInViewver].rarity += 1;
        RemoveGold(data);
        DestroyImmediate(gameObject);
    }

    private void RemoveGold(DataPlayer test)
    {
        test.money -= realPrice;
        GameManager.instance.SetValueDataPlayer(test);
    }
}