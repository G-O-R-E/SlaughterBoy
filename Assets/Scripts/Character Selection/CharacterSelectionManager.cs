using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] GameObject[] allButtonPlayers;
    [SerializeField] bool[] isBlock;
    [SerializeField] GameObject panel;

    private int characterIndex = -1;

    private bool updateStat = false;
    public bool UpdateStat { set => updateStat = value; }

    private void Start()
    {
        isBlock = AchievementManager.instance.GetIsCharacterLock();

        for (int i = 0; i < allButtonPlayers.Length; i++)
        {
            allButtonPlayers[i].GetComponent<Button>().interactable = !isBlock[i];
            if (isBlock[i])
            {
                allButtonPlayers[i].GetComponent<Image>().sprite = allButtonPlayers[i].GetComponent<IconCharacterSelection>().lockCharacter;
            }
            else
            {
                allButtonPlayers[i].GetComponent<Image>().sprite = allButtonPlayers[i].GetComponent<IconCharacterSelection>().unlockCharacter;
            }
        }
    }

    public void ChangeCharacter(int index)
    {
        //change character
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
        this.characterIndex = index;
        characters[index].SetActive(true);

        //reset text stat
        Transform allText = panel.transform.Find("AllText");
        for (int i = 0; i < (int)Stats.PlayerStat.NbStats; i++)
        {
            TextMeshProUGUI finalText = allText.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            string text = null;
            finalText.text = text;
            finalText.color = Color.white;
        }

        panel.SetActive(true);
        this.updateStat = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
        PlayerPrefs.SetInt("CharacterIndex", characterIndex);
    }

    public void Update()
    {
        if (characterIndex != -1 && this.updateStat)
        {
            this.updateStat = false;
            ChangeText();
        }
    }

    private void ChangeText()
    {
        int j = 0;
        Transform allText = panel.transform.Find("AllText");
        for (int i = 0; i < (int)Stats.PlayerStat.NbStats; i++)
        {
            TextMeshProUGUI finalText = allText.transform.GetChild(j).GetComponent<TextMeshProUGUI>();
            string text = characters[characterIndex].GetComponent<Stats>().SetDifferenceStats(i);

            if (text != null)
            {
                j++;
            }

            bool isGreen = characters[characterIndex].GetComponent<Stats>().IsColorGreen(i);

            finalText.color = Color.red;
            if (isGreen)
            {
                finalText.color = Color.green;
            }

            finalText.text = AddSpacesBeforeUppercase(text);
        }
    }

    private string AddSpacesBeforeUppercase(string input)
    {
        string result = "";

        if (input != null && input != "")
        {
            for (int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];
                if (char.IsUpper(currentChar) && i != 0) // if a letter is upper add a space
                {
                    result += " ";
                }
                result += currentChar;
            }
        }

        return result;
    }
}
