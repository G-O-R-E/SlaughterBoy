using TMPro;
using UnityEngine;
using static Stats;
public class BoostAttack : MonoBehaviour
{
    [SerializeField]
    Sprite sprite;

    public string type;

    // Stats
    public int attack;
    public int attackSpeed;
    public int price;
    //

    public TextMeshProUGUI[] descriptionTexts = null;

    // Start is called before the first frame update
    void Start()
    {
        this.name = "Boost attack";
        this.type = "Item";

        // Stats
        attack = 3;
        attackSpeed = 2;
        price = 17;
        //

        descriptionTexts = new TextMeshProUGUI[4];
        descriptionTexts[0].text = "Increase attack by " + attack.ToString();
        descriptionTexts[1].text = "Increase attackSpeed by " + attackSpeed.ToString();
        descriptionTexts[2].text = "";
        descriptionTexts[3].text = "";
    }

    public TextMeshProUGUI[] GetDescriptionTexts()
    {
        return descriptionTexts;
    }
}
