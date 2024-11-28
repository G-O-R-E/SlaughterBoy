using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] public Sprite sprite;
    [SerializeField] public string nameItems;
    [SerializeField] public string type;
    [SerializeField] public int price;
    [SerializeField] public RarityManager.Rarity rarity;
}
