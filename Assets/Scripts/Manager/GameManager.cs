using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Stats;

[System.Serializable]
public class WeaponPrefabs
{
    [SerializeField] public string name;
    [SerializeField] public GameObject prefabs;
}

[System.Serializable]
public class WeaponPlayer
{
    [SerializeField] public GameObject weapon;
    [SerializeField] public int rarity;
}

[System.Serializable]
public class ItemPlayer
{
    [SerializeField] public GameObject item;
    [SerializeField] public int rarity;
}

public class GameManager : MonoBehaviour
{
    //Singelton
    public static GameManager instance;

    private void Start()
    {
#if !UNITY_EDITOR
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 61;
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        Screen.SetResolution(Screen.width /2, Screen.height / 2, true);
#endif
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;
        dataPlayer.stats = new float[(int)PlayerStat.NbStats];
        dataPlayer.weapons = new WeaponPlayer[6];

        for (int i = 0; i < 6; i++)
        {
            dataPlayer.weapons[i] = new WeaponPlayer();
        }

        DontDestroyOnLoad(this);
        instance = this;
    }
    /////////////////////////////////////


    [System.Serializable]
    public struct DataPlayer
    {
        public float[] stats;
        public int life;
        public int nbLevelToUp;
        public int currentLevel;
        public int currentXp;
        public int maxXp;
        public int money;
        public WeaponPlayer[] weapons;
        public List<ItemPlayer> items;
    }

    [System.Serializable]
    public struct DataGame
    {
        public int waves;
        public float lastTimer;
        public int difficulty;
    }

    [Header("Prefabs")]
    [SerializeField] GameObject[] charactersVisuelPrefabs;
    [SerializeField] public List<WeaponPrefabs> weaponPrefabs;

    [Header("Player Data Between Scene")]
    [SerializeField] string previousScene = "Menu";
    [SerializeField] string activeScene = "Menu";

    [Header("Data Between Scene")]
    [SerializeField] DataPlayer dataPlayer;
    [SerializeField] DataGame dataGame;

    //for update text
    [Header("Need Update")]
    [SerializeField] bool updateLife = false;
    [SerializeField] bool updateMoney = false;
    [SerializeField] bool updateXp = false;
    [SerializeField] bool updateLevel = false;

    public bool hasInitAParty = false;
    private bool hasReInitAParty = false;
    public int characterIndex = -1;
    private bool changeScene = false;
    private bool dataPlayerIsSave = false;
    private bool dataGameIsSave = false;
    [SerializeField] bool playerIsDeath = false;
    private string nameNextScene = null;

    public bool UpdateLife { set => updateLife = value; get => updateLife; }
    public bool UpdateMoney { set => updateMoney = value; get => updateMoney; }
    public bool UpdateXp { set => updateXp = value; get => updateXp; }
    public bool UpdateLevel { set => updateLevel = value; get => updateLevel; }
    public bool HasInitAParty { get => hasInitAParty; }
    public bool HasReInitAParty { get => hasReInitAParty; set => hasReInitAParty = value; }
    public bool GonnaChangeScene { get => changeScene; set => changeScene = value; }
    public bool DataPlayerIsSave { set => dataPlayerIsSave = value; }
    public bool DataGameIsSave { set => dataGameIsSave = value; get => dataGameIsSave; }
    public bool PlayerIsDeath { set => playerIsDeath = value; get => playerIsDeath; }
    public string NameNextScene { set => nameNextScene = value; }

    public void SetValueDataGame(DataGame valueToSet)
    {
        if (valueToSet.waves != dataGame.waves)
        {
            dataGame.waves = valueToSet.waves;
        }

        if (valueToSet.lastTimer != dataGame.lastTimer)
        {
            dataGame.lastTimer = valueToSet.lastTimer;
        }

        if (valueToSet.difficulty != dataGame.difficulty)
        {
            dataGame.difficulty = valueToSet.difficulty;
        }
    }

    public void SetValueDataPlayer(DataPlayer valueToSet)
    {
        if (valueToSet.stats != dataPlayer.stats)
        {
            for (int i = 0; i < (int)PlayerStat.NbStats; i++)
            {
                dataPlayer.stats[i] = valueToSet.stats[i];
            }
        }

        if (valueToSet.life != dataPlayer.life)
        {
            dataPlayer.life = valueToSet.life;
        }

        if (valueToSet.currentLevel != dataPlayer.currentLevel)
        {
            dataPlayer.currentLevel = valueToSet.currentLevel;
        }

        if (valueToSet.currentXp != dataPlayer.currentXp)
        {
            dataPlayer.currentXp = valueToSet.currentXp;
        }

        if (valueToSet.currentLevel != dataPlayer.currentLevel)
        {
            dataPlayer.currentLevel = valueToSet.currentLevel;
        }

        if (valueToSet.maxXp != dataPlayer.maxXp)
        {
            dataPlayer.maxXp = valueToSet.maxXp;
        }

        if (valueToSet.nbLevelToUp != dataPlayer.nbLevelToUp)
        {
            dataPlayer.nbLevelToUp = valueToSet.nbLevelToUp;
        }

        if (valueToSet.money != dataPlayer.money)
        {
            dataPlayer.money = valueToSet.money;
        }

        for (int i = 0; i < valueToSet.weapons.Length; i++)
        {
            GameObject weapon = valueToSet.weapons[i].weapon;
            dataPlayer.weapons[i].weapon = weapon;
        }
    }

    public void ResetValue()
    {
        previousScene = "Level";
        updateLife = false;
        updateMoney = false;
        updateXp = false;
        hasInitAParty = false;
        hasReInitAParty = false;
        characterIndex = -1;
        changeScene = false;
        dataPlayerIsSave = false;
        dataGameIsSave = false;
        nameNextScene = null;
        playerIsDeath = false;

        dataGame.waves = 1;
        dataGame.lastTimer = 0;
        dataGame.difficulty = 0;

        dataPlayer.stats = new float[(int)PlayerStat.NbStats];
        dataPlayer.life = 0;
        dataPlayer.nbLevelToUp = 0;
        dataPlayer.currentLevel = 0;
        dataPlayer.currentXp = 0;
        dataPlayer.maxXp = 0;
        dataPlayer.money = 0;
        dataPlayer.weapons = new WeaponPlayer[6];
        dataPlayer.items.Clear();
    }

    public DataGame GetDataGame() { return dataGame; }
    public DataPlayer GetDataPlayer() { return dataPlayer; }

    private void Update()
    {
        CheckScene();
        IsPlayerDeath();
        CanInitParty();
        CanReInitParty();
        ChangeSceneGame();
    }

    private void IsPlayerDeath()
    {
        if (playerIsDeath)
        {
            GameManager.instance.GonnaChangeScene = true;
            GameManager.instance.NameNextScene = "GameOver";

            if (dataPlayerIsSave && dataGameIsSave)
            {
                changeScene = false;
                dataPlayerIsSave = false;
                dataGameIsSave = false;
                Input.ResetInputAxes();
                Input.ClearLastPenContactEvent();
                Input.ResetPenEvents();
                SceneManager.LoadScene(this.nameNextScene);

                nameNextScene = null;
            }
        }
    }

    private void CheckScene()
    {
        if (SceneManager.GetActiveScene().name != this.activeScene)
        {
            previousScene = activeScene;
            activeScene = SceneManager.GetActiveScene().name;
        }
    }

    private void LoadCharacter()
    {
        this.characterIndex = PlayerPrefs.GetInt("CharacterIndex");
        GameObject playerStat = GameObject.Instantiate(charactersVisuelPrefabs[this.characterIndex]);
        playerStat.transform.parent = GameObject.Find("Player").transform.GetChild(0).transform;
        AddNormalStat(playerStat);
    }

    private void ReLoadCharacter()
    {
        GameObject player = GameObject.Instantiate(charactersVisuelPrefabs[this.characterIndex]);
        GameObject playerContainer = GameObject.Find("Player");
        player.transform.parent = playerContainer.transform.GetChild(0).transform;

        //reinit stat
        if (player.TryGetComponent<Stats>(out Stats statsPlayer))
        {
            for (int i = 0; i < (int)PlayerStat.NbStats; i++)
            {
                statsPlayer.GetStatsTab()[i] = dataPlayer.stats[i];
            }
        }

        //reinit data
        if (playerContainer.TryGetComponent<PlayerDataManager>(out PlayerDataManager dataPlayerManager))
        {
            dataPlayerManager.SetPlayerData(dataPlayer);
        }

        //erase weapon
        if (playerContainer.transform.GetChild(1).transform.childCount > 0)
        {
            int nbChlid = playerContainer.transform.GetChild(1).transform.childCount;
            for (int i = 0; i < nbChlid; i++)
            {
                DestroyImmediate(playerContainer.transform.GetChild(1).GetChild(i).gameObject);
            }
        }

        //add weapon saved
        for (int i = 0; i < dataPlayer.weapons.Length; i++)
        {
            if (dataPlayer.weapons[i].weapon != null)
            {
                float rarityFactor = 0.5f * (int)dataPlayer.weapons[i].rarity + 1;

                GameObject weapon = GameObject.Instantiate(dataPlayer.weapons[i].weapon, playerContainer.transform.GetChild(1).transform);
                if (weapon.TryGetComponent<Weapon>(out Weapon weap))
                {

                    weap.baseDamage *= (int)rarityFactor;
                    weap.InitializeWeapon(i);
                }
                else if (weapon.TryGetComponent<MeleeWeapon>(out MeleeWeapon melleeWeap))
                {
                    melleeWeap.baseDamage *= (int)rarityFactor;
                    melleeWeap.InitializeWeapon(i);
                }
                else if(weapon.TryGetComponent<FlameThrower>(out FlameThrower thrower))
                {
                    thrower.baseDamage *= (int)rarityFactor;
                    thrower.InitializeWeapon(i);
                }
            }
        }
    }

    private void AddNormalStat(GameObject playerStat)
    {
        playerStat.GetComponent<Stats>().InitStats();
    }

    private void CanInitParty()
    {
        if (previousScene == "CharacterSelection" && activeScene == "Level" && !hasInitAParty)
        {
            LoadCharacter();
            hasInitAParty = true;
        }
    }

    private void CanReInitParty()
    {
        if ((previousScene == "LVLUp" || previousScene == "Shop") && activeScene == "Level" && !hasReInitAParty)
        {
            ReLoadCharacter();
            hasReInitAParty = true;
            nameNextScene = null;
        }
    }

    private void ChangeSceneGame()
    {
        if (dataPlayerIsSave && dataGameIsSave && IsAllGoldCollected() && !playerIsDeath)
        {
            changeScene = false;
            dataPlayerIsSave = false;
            dataGameIsSave = false;
            hasReInitAParty = false;

            if (nameNextScene == null)
            {
                if (dataPlayer.nbLevelToUp > 0)
                {
                    NameNextScene = "LVLUp";
                }
                else
                {
                    NameNextScene = "Shop";
                }
            }
            Input.ResetInputAxes();
            Input.ClearLastPenContactEvent();
            Input.ResetPenEvents();
            SceneManager.LoadScene(this.nameNextScene);

            nameNextScene = null;
        }
    }

    private bool IsAllGoldCollected()
    {
        if (dataGameIsSave)
        {
            if (GameObject.Find("Coins").transform.childCount <= 0)
            {
                dataPlayer.money++;
                return true;
            }
        }
        return false;
    }
}