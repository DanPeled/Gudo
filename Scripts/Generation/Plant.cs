using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public string PlantName;
    public GameObject drop;
    public enum States
    {
        Small,
        NoLeaves,
        Grown
    }
    public States state;

    private void OnDestroy()
    {
        
    }
}
