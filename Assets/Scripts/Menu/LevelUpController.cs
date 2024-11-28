using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Stats;

public class LevelUpController : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] public GameObject[] ButtonList;
    [SerializeField] TextMeshProUGUI[] statDescription;
    [SerializeField] TextMeshProUGUI textLevelToUpgrade;
    [SerializeField] Sprite[] sprite;
    [SerializeField] int numLevelToUpgrade;
    [SerializeField] int priceRefresh;
    [SerializeField] TextMeshProUGUI priceRefreshText;
    [SerializeField] TextMeshProUGUI goldPlayer;

    [Header("Rarity")]
    [SerializeField] Vector2Int[] valueOnRarity;
    [SerializeField] RarityManager rarityManager;

    [Header("Sound Parameter")]
    [SerializeField] AudioMixerGroup group;
    [SerializeField] AudioClip[] clip;
    private AudioSource source;

    public int NumLevelToUpgrade { get => numLevelToUpgrade; set => numLevelToUpgrade = value; }

    private string[] evoNames = {
        "Heart",
        "Pacemaker",
        "Boots",
        "Strength",
        "Rapidity",
        "Sword Master",
        "Sniper",
        "Tormentor",
        "Sharp eyesight",
        "Time bender",
        "Avarice",
        "Barter"
    };

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
        source.loop = false;

        goldPlayer.text = " " + GameManager.instance.GetDataPlayer().money.ToString();
        priceRefreshText.text = priceRefresh.ToString();
        numLevelToUpgrade = GameManager.instance.GetDataPlayer().nbLevelToUp;
        AssignStatsSpritesToImages();
        ChangeTextLevelToUpgrade();
    }

    public void AssignStatsSpritesToImages()
    {
        int[] tempRand = new int[3] { -1, -1, -1 };

        tempRand[0] = Random.Range(0, sprite.Length);
        InitButton(0, tempRand[0]);

        do
        {
            tempRand[1] = Random.Range(0, sprite.Length);
        } while (tempRand[1] == tempRand[0]);
        InitButton(1, tempRand[1]);

        do
        {
            tempRand[2] = Random.Range(0, sprite.Length);
        } while (tempRand[2] == tempRand[0] || tempRand[2] == tempRand[1]);
        InitButton(2, tempRand[2]);
    }

    public void PayToRefresh()
    {
        if (GameManager.instance.GetDataPlayer().money >= priceRefresh)
        {
            GameManager.DataPlayer dataTemp = GameManager.instance.GetDataPlayer();
            dataTemp.money -= priceRefresh;
            GameManager.instance.SetValueDataPlayer(dataTemp);
            priceRefresh *= 2;
            priceRefreshText.text = priceRefresh.ToString();
            goldPlayer.text = " " + GameManager.instance.GetDataPlayer().money.ToString();
            AssignStatsSpritesToImages();
        }
        else
        {
            //sound
            int rndClipFire = Random.Range(0, clip.Length);
            source.clip = clip[rndClipFire];
            source.Play();
        }
    }

    private void InitButton(int index, int spriteIndex)
    {
        Stats.PlayerStat stat = (Stats.PlayerStat)(spriteIndex);
        int rarity = (int)rarityManager.GiveARarity();
        int value = Random.Range(valueOnRarity[rarity].x, valueOnRarity[rarity].y);

        ButtonList[index].GetComponent<Button>().interactable = true;
        ButtonList[index].GetComponentInChildren<LVLUpValue>().Stat = stat;
        ButtonList[index].GetComponentInChildren<LVLUpValue>().ValueStat = value;
        ButtonList[index].transform.GetChild(1).GetComponent<Image>().sprite = sprite[spriteIndex];
        ButtonList[index].transform.GetChild(0).GetComponent<Image>().color = rarityManager.colorRarity[rarity];

        CreateStatDescriptionText(stat, index, value);
    }

    private void CreateStatDescriptionText(PlayerStat stat, int index, int valueStat)
    {
        string statText = evoNames[(int)stat] + " :\n" + "Increase " + stat.ToString() + " by " + "<color=green>" + valueStat + "</color>";
        statDescription[index].text = statText;
    }

    public void ChangeTextLevelToUpgrade()
    {
        textLevelToUpgrade.text = "Levels to upgrade : " + numLevelToUpgrade;
    }
}