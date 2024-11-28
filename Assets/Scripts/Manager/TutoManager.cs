using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutoManager : MonoBehaviour
{
    private static TutoManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            return;
        }

        DestroyImmediate(gameObject);
    }

    private enum TutoFrame
    {
        CharacterSelection,
        Game,
        LVLUp,
        Shop,
    }

    [Header("PrefabsTuto")]
    [SerializeField] private GameObject[] prefabsCharacterSelection;
    [SerializeField] private GameObject[] prefabsGame;
    [SerializeField] private GameObject[] prefabsLVLUp;
    [SerializeField] private GameObject[] prefabsShop;

    private GameObject actualTuto;
    private GameObject container;

    private int currentIndexPrefabsTuto = 0;
    private int lengthTutoFrame = 0;

    private string previousScene = null;
    private string currentScene = null;

    private TutoFrame actualFrameType;

    [Header("Already Show")]
    [SerializeField] private bool characterSelectionDone = false;
    [SerializeField] private bool gameDone = false;
    [SerializeField] private bool lvlUpDone = false;
    [SerializeField] private bool shopDone = false;
    
    private void Update()
    {
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != previousScene)
        {
            //UpdateInitTutoInMenu();
            UpdateCharacterSelection();
            UpdateGame();
            UpdateLVLUp();
            UpdateShop();

            previousScene = currentScene;
        }
    }

    private void UpdateCharacterSelection()
    {
        if (currentScene == "CharacterSelection" && !characterSelectionDone)
        {
            //container tuto
            container = GameObject.Find("ContainerTuto");

            //init funct on button
            container.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Next);
            container.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Previous);
            container.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(QuitTuto);

            //appear button
            container.transform.GetChild(0).gameObject.SetActive(true);
            container.transform.GetChild(1).gameObject.SetActive(true);
            container.transform.GetChild(2).gameObject.SetActive(true);

            //index tuto frame
            currentIndexPrefabsTuto = 0;
            lengthTutoFrame = prefabsCharacterSelection.Length;
            actualFrameType = TutoFrame.CharacterSelection;

            ChangeActualTuto(prefabsCharacterSelection[currentIndexPrefabsTuto]);
        }
    }

    private void UpdateGame()
    {
        if (currentScene == "Level" && !gameDone)
        {
            //container tuto
            container = GameObject.Find("ContainerTuto");

            //init funct on button
            container.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Next);
            container.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Previous);
            container.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(QuitTuto);

            //appear button
            container.transform.GetChild(0).gameObject.SetActive(true);
            container.transform.GetChild(1).gameObject.SetActive(true);
            container.transform.GetChild(2).gameObject.SetActive(true);

            //index tuto frame
            currentIndexPrefabsTuto = 0;
            lengthTutoFrame = prefabsGame.Length;
            actualFrameType = TutoFrame.Game;

            ChangeActualTuto(prefabsGame[currentIndexPrefabsTuto]);

            Time.timeScale = 0;
        }
    }

    private void UpdateLVLUp()
    {
        if (currentScene == "LVLUp" && !lvlUpDone)
        {
            //container tuto
            container = GameObject.Find("ContainerTuto");

            //init funct on button
            container.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Next);
            container.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Previous);
            container.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(QuitTuto);

            //appear button
            container.transform.GetChild(0).gameObject.SetActive(true);
            container.transform.GetChild(1).gameObject.SetActive(true);
            container.transform.GetChild(2).gameObject.SetActive(true);

            //index tuto frame
            currentIndexPrefabsTuto = 0;
            lengthTutoFrame = prefabsLVLUp.Length;
            actualFrameType = TutoFrame.LVLUp;

            ChangeActualTuto(prefabsLVLUp[currentIndexPrefabsTuto]);
        }
    }

    private void UpdateShop()
    {
        if (currentScene == "Shop" && !shopDone)
        {
            //container tuto
            container = GameObject.Find("ContainerTuto");

            //init funct on button
            container.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(Next);
            container.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Previous);
            container.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(QuitTuto);

            //appear button
            container.transform.GetChild(0).gameObject.SetActive(true);
            container.transform.GetChild(1).gameObject.SetActive(true);
            container.transform.GetChild(2).gameObject.SetActive(true);

            //index tuto frame
            currentIndexPrefabsTuto = 0;
            lengthTutoFrame = prefabsShop.Length;
            actualFrameType = TutoFrame.Shop;

            ChangeActualTuto(prefabsShop[currentIndexPrefabsTuto]);
        }
    }

    public void QuitTuto()
    {
        DestroyImmediate(actualTuto);

        container.transform.GetChild(0).gameObject.SetActive(false);
        container.transform.GetChild(1).gameObject.SetActive(false);
        container.transform.GetChild(2).gameObject.SetActive(false);

        switch (actualFrameType)
        {
            case TutoFrame.CharacterSelection:
                characterSelectionDone = true;
                break;

            case TutoFrame.Game:
                gameDone = true;
                Time.timeScale = 1;
                break;

            case TutoFrame.LVLUp:
                lvlUpDone = true;
                break;

            case TutoFrame.Shop:
                shopDone = true;
                break;

            default:
                break;
        }
    }

    public void Next()
    {
        currentIndexPrefabsTuto++;

        if (currentIndexPrefabsTuto >= lengthTutoFrame)
        {
            currentIndexPrefabsTuto = 0;
        }

        SearchWhichPrefabs();
    }

    public void Previous()
    {
        currentIndexPrefabsTuto--;
        if (currentIndexPrefabsTuto < 0)
        {
            currentIndexPrefabsTuto = lengthTutoFrame - 1;
        }

        SearchWhichPrefabs();
    }

    private void SearchWhichPrefabs()
    {
        switch (actualFrameType)
        {
            case TutoFrame.CharacterSelection:
                ChangeActualTuto(prefabsCharacterSelection[currentIndexPrefabsTuto]);
                break;

            case TutoFrame.Game:
                ChangeActualTuto(prefabsGame[currentIndexPrefabsTuto]);
                break;

            case TutoFrame.LVLUp:
                ChangeActualTuto(prefabsLVLUp[currentIndexPrefabsTuto]);
                break;

            case TutoFrame.Shop:
                ChangeActualTuto(prefabsShop[currentIndexPrefabsTuto]);
                break;

            default:
                break;
        }
    }

    ////////////////////////////////Display Menu//////////////////////////////
    
    [Header("Prefabs Tuto Menu")]
    [SerializeField] GameObject[] allTutoFrame;

    private void UpdateInitTutoInMenu()
    {
        if (currentScene == "Menu")
        {
            GameObject.Find("Tuto").GetComponent<Button>().onClick.AddListener(ShowTutoMenu);

            container = GameObject.Find("ContainerTuto");
            container.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(NextInMenu);
            container.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(PreviousInMenu);
            container.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(QuitTutoMenu);

            currentIndexPrefabsTuto = 0;
        }
    }

    private void ChangeActualTuto(GameObject newTutoFrame)
    {
        if (actualTuto != null)
        {
            DestroyImmediate(actualTuto);
        }

        actualTuto = GameObject.Instantiate(newTutoFrame, container.transform);
        actualTuto.transform.SetAsFirstSibling();
    }

    public void NextInMenu()
    {
        currentIndexPrefabsTuto++;
        currentIndexPrefabsTuto = currentIndexPrefabsTuto % allTutoFrame.Length;

        ChangeActualTuto(allTutoFrame[currentIndexPrefabsTuto]);
    }

    public void PreviousInMenu()
    {
        currentIndexPrefabsTuto--;

        if (currentIndexPrefabsTuto < 0)
        {
            currentIndexPrefabsTuto = allTutoFrame.Length - 1;
        }

        ChangeActualTuto(allTutoFrame[currentIndexPrefabsTuto]);
    }

    public void ShowTutoMenu()
    {
        container.transform.GetChild(0).gameObject.SetActive(true);
        container.transform.GetChild(1).gameObject.SetActive(true);
        container.transform.GetChild(2).gameObject.SetActive(true);

        ChangeActualTuto(allTutoFrame[currentIndexPrefabsTuto]);
    }

    public void QuitTutoMenu()
    {
        DestroyImmediate(actualTuto);

        container.transform.GetChild(0).gameObject.SetActive(false);
        container.transform.GetChild(1).gameObject.SetActive(false);
        container.transform.GetChild(2).gameObject.SetActive(false);
    }
}