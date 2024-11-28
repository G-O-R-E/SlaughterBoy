using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LVLUpValue : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] Stats.PlayerStat stat;
    [SerializeField] int valueStat;
    [SerializeField] LevelUpController controller;
    [SerializeField] Button button;

    [Header("Action")]
    [SerializeField] StatsDisplayInMenu display;
    [SerializeField] LevelUpController levelUpController;

    public Stats.PlayerStat Stat { set => stat = value; }
    public int ValueStat { set => valueStat = value; }

    public void UpgradeStat()
    {
        GameManager.instance.GetDataPlayer().stats[(int)this.stat] += this.valueStat;

        controller.NumLevelToUpgrade--;
        controller.ChangeTextLevelToUpgrade();
        button.interactable = false;

        if (controller.NumLevelToUpgrade <= 0)
        {
            SceneManager.LoadScene("Shop");

            //set data player
            GameManager.DataPlayer data = GameManager.instance.GetDataPlayer();
            data.nbLevelToUp = 0;
            GameManager.instance.SetValueDataPlayer(data);
        }
        else
        {
            display.UpdateText();
            levelUpController.AssignStatsSpritesToImages();
        }
    }
}