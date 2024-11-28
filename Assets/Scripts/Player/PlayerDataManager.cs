using UnityEngine;
using static Stats;

public class PlayerDataManager : MonoBehaviour
{
    private Stats playerStats;
    private GameManager.DataPlayer data;
    private bool lifeInit = false;
    [SerializeField] int currentLife = 20;
    float regenTimer = 0f;
    float regenMaxTimer = 5f;
    [SerializeField] float animDamageTime = 0.25f;
    public int CurrentLife
    {
        get => currentLife;

        set
        {

            currentLife = value;
            if (currentLife < 0 ) 
            { 
                currentLife = 0;
            }
            GameManager.instance.UpdateLife = true;

        }
    }

    [SerializeField] int money;
    public int Money
    {
        get => money;
        set
        {
            money = value;
            GameManager.instance.UpdateMoney = true;
        }
    }

    [SerializeField] int currentLevel = 1;
    public int CurrentLevel { get => currentLevel; }

    [SerializeField] int nbLevelUp = 0;
    public int NbLevelUp { get => nbLevelUp; }
    [SerializeField] int currentXp;
    public int CurrentXp
    {
        get => currentXp;
        set
        {
            currentXp = value;
            GameManager.instance.UpdateXp = true;
        }
    }

    [SerializeField] int maxXp = 200;
    public int MaxXp { get => maxXp; set => maxXp = value; }

    private bool isDataInit = false;

    private void InitData()
    {
        playerStats = transform.GetChild(0).GetComponentInChildren<Stats>();
        if (!lifeInit)
        {
            currentLife = playerStats.GetLife();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetChild(0).childCount > 0 && !isDataInit)
        {
            InitData();
            isDataInit = true;
        }

        UpdateLevel();
        PlayerRegeneration();
        IsPlayerDead();
        SaveDataPlayer();
    }

    private void UpdateLevel()
    {
        if (currentXp >= maxXp)
        {
            currentXp = Mathf.Abs(maxXp - currentXp);
            currentLevel++;
            maxXp += 50 * currentLevel;
            nbLevelUp++;
            GameManager.instance.UpdateXp = true;
            GameManager.instance.UpdateLevel = true;
        }
    }
    private void SaveDataPlayer()
    {
        if (GameManager.instance.GonnaChangeScene)
        {
            data.stats = new float[(int)PlayerStat.NbStats];
            playerStats.SaveStat(data);
            SaveWeapon();
            data.life = currentLife;
            data.currentXp = currentXp;
            data.maxXp = maxXp;
            data.money = money;
            data.currentLevel = currentLevel;
            data.nbLevelToUp = nbLevelUp;

            GameManager.instance.DataPlayerIsSave = true;
            GameManager.instance.SetValueDataPlayer(data);
        }
    }

    private void SaveWeapon()
    {
        data.weapons = new WeaponPlayer[transform.GetChild(1).transform.childCount];
        for (int i = 0; i < transform.GetChild(1).transform.childCount; i++)
        {
            GameObject weaponGameObject = transform.GetChild(1).GetChild(i).gameObject;
            WeaponPrefabs weaponInfo = GameManager.instance.weaponPrefabs.Find(w => w.name == weaponGameObject.name);

            if (weaponInfo != null)
            {
                data.weapons[i] = new WeaponPlayer();
                data.weapons[i].weapon = new GameObject();
                data.weapons[i].weapon = weaponInfo.prefabs;
            }
        }
    }

    public void SetPlayerData(GameManager.DataPlayer data)
    {
        currentLife = data.life;
        currentXp = data.currentXp;
        maxXp = data.maxXp;
        money = data.money;
        currentLevel = data.currentLevel;
        nbLevelUp = data.nbLevelToUp;

        lifeInit = true;
    }

    public void IsPlayerDead()
    {
        if (currentLife <= 0 && !GameManager.instance.PlayerIsDeath)
        {
            GameManager.instance.PlayerIsDeath = true;
            currentLife = 0;
            GameManager.instance.UpdateLife = true;
        }
    }

    private void PlayerRegeneration()
    {
        if (regenMaxTimer != 5f - (5f * playerStats.GetRegen() / 100f))
        {
            regenMaxTimer = 5f - (5f * playerStats.GetRegen() / 100f);
        }

        if (currentLife < playerStats.GetLife())
        {
            regenTimer += Time.deltaTime;
        }
        else if (currentLife > playerStats.GetLife())
        {
            currentLife = playerStats.GetLife();
            GameManager.instance.UpdateLife = true;
        }

        if (regenTimer >= regenMaxTimer)
        {
            regenTimer = 0f;
            currentLife++;
            GameManager.instance.UpdateLife = true;
        }
    }

    public bool DodgeAttack()
    {
        bool dodgeAttack = false;
        if ((int)Random.Range(0f, 100f) <= playerStats.GetDodge())
        {
            dodgeAttack = true;
        }
        return dodgeAttack;
    }
    public bool CritAttack()
    {
        bool critAttack = false;
        if ((int)Random.Range(0f, 100f) <= playerStats.GetCrit())
        {
            critAttack = true;
        }
        return critAttack;
    }
    public void PlayAnimDamage()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.red;
        Invoke("StopAnimDamage", animDamageTime);
    }
    void StopAnimDamage()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }
}