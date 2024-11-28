using UnityEngine;

public class RarityManager : MonoBehaviour
{
    public static RarityManager instance;

    [Header("Base Rarity Percent")]
    [SerializeField] float commmon = 80f;
    [SerializeField] float uncommon = 15f;
    [SerializeField] float rare = 3f;
    [SerializeField] float epic = 1.5f;
    [SerializeField] float legendary = 0.5f;

    [Header("Modified Rarity Percent")]
    [SerializeField] float commmonModified = 0;
    [SerializeField] float uncommonModified = 0;
    [SerializeField] float rareModified = 0;
    [SerializeField] float epicModified = 0;
    [SerializeField] float legendaryModified = 0;

    [Header("Ratio Adjustement Percent")]
    [SerializeField] static float commonAdjustement = 1f;
    [SerializeField] static float uncommonAdjustement = 0.5f;
    [SerializeField] static float rareAdjustement = 0.75f;
    [SerializeField] static float epicAdjustement = 0.5f;
    [SerializeField] static float legendaryAdjustement = 0.25f;

    [Header("Color Rarity")]
    [SerializeField] public Color[] colorRarity;

    [Header("Stats Increaser")]
    [SerializeField] public float rarityModifier = 0.5f;

    private bool init = false;

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public void Awake()
    {
        Input.ResetInputAxes();
        Input.ClearLastPenContactEvent();
        Input.ResetPenEvents();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public Rarity GiveARarity()
    {
        if (!init)
        {
            commmonModified = commmon;
            uncommonModified = uncommon;
            rareModified = rare;
            epicModified = epic;
            legendaryModified = legendary;
            ChangedPercentOnNbWaves();
            PercentNotGood();

            init = true;
        }

        int rarity = Random.Range(1, 10001);
        float rarityPercent = rarity / 100.00f;

        if (rarityPercent <= commmonModified)
        {
            return Rarity.Common;
        }
        else if (rarityPercent <= commmonModified + uncommonModified && rarityPercent > commmonModified)
        {
            return Rarity.Uncommon;
        }
        else if (rarityPercent <= commmonModified + uncommonModified + rareModified && rarityPercent > commmonModified + uncommonModified)
        {
            return Rarity.Rare;
        }
        else if (rarityPercent <= commmonModified + uncommonModified + rareModified + epicModified && rarityPercent > commmonModified + uncommonModified + rareModified)
        {
            return Rarity.Epic;
        }
        else if (rarityPercent <= 100f && rarityPercent > 100f - legendaryModified)
        {
            return Rarity.Legendary;
        }

        return Rarity.Common;
    }

    private void PercentNotGood()
    {
        if (commmonModified + uncommonModified + rareModified + epicModified + legendaryModified != 100f)
        {
            Debug.LogError("Percent Over or less than 100\nRarityManager");
        }
    }

    private void ChangedPercentOnNbWaves()
    {
        int nbWave = GameManager.instance.GetDataGame().waves - 1;

        commmonModified -= nbWave * commonAdjustement;
        uncommonModified -= nbWave * uncommonAdjustement;
        rareModified += nbWave * rareAdjustement;
        epicModified += nbWave * epicAdjustement;
        legendaryModified += nbWave * legendaryAdjustement;
    }
}