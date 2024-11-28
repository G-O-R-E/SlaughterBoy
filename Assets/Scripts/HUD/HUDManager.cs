using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI hpHUD;
    [SerializeField] TextMeshProUGUI wavesHUD;
    [SerializeField] TextMeshProUGUI moneyHUD;
    [SerializeField] TextMeshProUGUI levelHUD;
    [SerializeField] GameObject player;
    [SerializeField] PlayerDataManager playerData;

    [SerializeField] Slider timerBar;
    [SerializeField] GameObject timerGameObject;
    [SerializeField] Slider lifeBar;
    [SerializeField] Slider xpBar;

    private Stats stats;
    private bool hasInit = false;
    private bool hasReInit = false;
    private float maxTimer;
    private float timer;
    private bool coroutineStart = false;

    public float Timer { get => timer; set { timer = value; } }
    private int maxHp;
   
    [SerializeField] int waves = 1;

    public int Waves { get => waves; }

    private GameManager.DataGame data;

    private void Init()
    {
        timer = 30f;

        //HP
        if (player.transform.GetChild(0).childCount > 0)
        {
            stats = player.transform.GetChild(0).GetComponentInChildren<Stats>();
            int valueHP = (int)stats.GetLife();
            maxHp = valueHP;
            hpHUD.text = valueHP + "/" + maxHp;
            hasInit = true;

            lifeBar.maxValue = valueHP;
            lifeBar.value = valueHP;
        }

        //Timer
        if (this.timer > 0f)
        {
            maxTimer = timer;
            timerBar.maxValue = timer;
            timerBar.value = timer;
            coroutineStart = false;
        }

        //Waves
        if (wavesHUD != null)
        {
            wavesHUD.text = "Waves : " + waves.ToString();
        }

        //Level
        if (levelHUD != null)
        {
            levelHUD.text = "Level 1";
        }
    }

    private void Update()
    {
        if (!hasInit && GameManager.instance.HasInitAParty)
        {
            Init();
        }

        if (!hasReInit && GameManager.instance.HasReInitAParty)
        {
            ReInit();
        }

        if (hasInit || hasReInit)
        {
            //update hud if something have change
            UpdateLife();
            UpdateMoney();
            UpdateXp();

            // update timer evry frame
            UpdateTimer();
        }

        if (GameManager.instance.GonnaChangeScene && !GameManager.instance.DataGameIsSave)
        {
            SaveDataGame();
            GameManager.instance.SetValueDataGame(this.data);
            GameManager.instance.DataGameIsSave = true;
        }
    }

    private void ReInit()
    {
        GameManager.DataGame tempGame = GameManager.instance.GetDataGame();
        GameManager.DataPlayer tempPlayer = GameManager.instance.GetDataPlayer();

        this.waves = tempGame.waves;
        this.timer = tempGame.lastTimer;

        //Waves
        if (wavesHUD != null)
        {
            wavesHUD.text = "Waves : " + waves.ToString();
        }

        //Timer
        if (this.timer > 0f)
        {
            maxTimer = timer;
            timerBar.maxValue = timer;
            timerBar.value = timer;
            coroutineStart = false;
        }

        //Xp Bar
        if (xpBar != null)
        {
            xpBar.maxValue = tempPlayer.maxXp;
            xpBar.value = tempPlayer.currentXp;

            xpBar.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Level " + tempPlayer.currentLevel;
        }

        //money
        if (moneyHUD != null)
        {
            moneyHUD.text = tempPlayer.money.ToString();
        }

        //life
        if (lifeBar != null)
        {
            lifeBar.value = GameManager.instance.GetDataPlayer().life; 
            maxHp = stats.GetLife();
            hpHUD.text = lifeBar.value + "/" + maxHp;
        }

        hasReInit = true;
    }

    private void UpdateTimer()
    {
        if (timer > 0f)
        {
            this.timer -= Time.deltaTime;

            if (timer < 0f)
            {              
                timer = 0f;
            }

            FlashTimer();
            timerBar.value = timer;
            CheckTimer();
        }
    }

    private void FlashTimer()
    {
        if (timer <= 5f && !coroutineStart)
        {
            StartCoroutine(ScaleObject());
            coroutineStart = true;
        }
    }

    private IEnumerator ScaleObject()
    {
        while (true) 
        {
            yield return StartCoroutine(ScaleTo(1.5f, 0.5f));
            yield return StartCoroutine(ScaleTo(1f, 0.5f));
        }
    }

    private IEnumerator ScaleTo(float targetScale, float duration)
    {
        Vector3 originalScale = timerGameObject.transform.localScale;
        Vector3 target = new Vector3(targetScale, targetScale, targetScale);
        float elapsed = 0;

        while (elapsed < duration)
        {
            timerGameObject.transform.localScale = Vector3.Lerp(originalScale, target, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        timerGameObject.transform.localScale = target;
    }

    private void UpdateLife()
    {
        if (GameManager.instance.UpdateLife)
        {
            GameManager.instance.UpdateLife = false;
            hpHUD.text = playerData.CurrentLife + "/" + maxHp;
            lifeBar.value = playerData.CurrentLife;
        }
    }
    private void UpdateMoney()
    {
        if (GameManager.instance.UpdateMoney)
        {
            GameManager.instance.UpdateMoney = false;
            moneyHUD.text = playerData.Money.ToString();
        }
    }
    private void UpdateXp()
    {
        if (GameManager.instance.UpdateXp)
        {
            GameManager.instance.UpdateXp = false;
            xpBar.value = playerData.CurrentXp;
        }

        if (GameManager.instance.UpdateLevel)
        {
            GameManager.instance.UpdateLevel = false;
            xpBar.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Level " + playerData.CurrentLevel;
            xpBar.maxValue = playerData.MaxXp;
        }
    }

    private void CheckTimer()
    {
        if (this.timer <= 0f)
        {
            waves++;

            if (GameManager.instance.PlayerIsDeath || waves == 21)
            {
                SaveDataGame();
                GameManager.instance.SetValueDataGame(this.data);
                GameManager.instance.GonnaChangeScene = true;
                GameManager.instance.NameNextScene = "GameOver";
                GameManager.instance.DataGameIsSave = true;
            }
            else if (waves != 21)
            {
                SaveDataGame();
                GameManager.instance.SetValueDataGame(this.data);
                GameManager.instance.GonnaChangeScene = true;
                GameManager.instance.DataGameIsSave = true;
            }
        }
    }

    private void SaveDataGame()
    {
        this.data.waves = this.waves;
        this.data.lastTimer = this.maxTimer;
    }
}
