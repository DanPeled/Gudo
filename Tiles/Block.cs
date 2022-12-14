using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Block : MonoBehaviour
{
    public GameObject block;
    public GameObject drop;
    public GameObject replaceWith;
    public int durabilty = 1;
    public int health = 1;
    public enum Rarity
    {
        None,
        Common,
        Uncommon,
        Rare,
        VeryRare,
        Epic,
        Ledendary
    }
    public Rarity rarity = Rarity.Common;
    private void Awake()
    {
        this.block = this.GetComponent<GameObject>();
    }
}