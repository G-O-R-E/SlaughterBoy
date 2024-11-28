using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
    //singleton
    public static AchievementManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
        instance = this;
    }
    ///////////////////

    [Header("Stats Player On The Game")]
    [SerializeField] float nbEnnemyKill;
    [SerializeField] float nbMoneySpend;
    [SerializeField] float nbShoot;
    [SerializeField] float timeSpend; //in sec

    [Header("Player Unlock")]
    [SerializeField] bool[] isCharacterLock;

    private void Update()
    {
        CheckAchievement();
    }

    private void CheckAchievement()
    {
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            KamikazeCharacter();
            ScientifistCharacter();
            WizardCharacter();
        }
    }

    private void KamikazeCharacter()
    {
        if (!isCharacterLock[1])
        {
            if (nbEnnemyKill >= 1000)
            {
                isCharacterLock[1] = false;
            }
        }
    }

    private void ScientifistCharacter()
    {
        if (!isCharacterLock[2])
        {
            if (timeSpend >= 10000)
            {
                isCharacterLock[2] = false;
            }
        }
    }
    private void WizardCharacter()
    {
        if (!isCharacterLock[3])
        {
            if (nbShoot >= 1000)
            {
                isCharacterLock[3] = false;
            }
        }
    }

    public bool[] GetIsCharacterLock()
    {
        return isCharacterLock;
    }

    public void AddEnnemyKill()
    {
        this.nbEnnemyKill++;
    }
}