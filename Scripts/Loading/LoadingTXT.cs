using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTXT : MonoBehaviour
{
    public Text txt;
    public static int precent = 0;
    private void Start()
    {
        txt = GetComponent<Text>();
    }
    private void Update()
    {
        
    }
}
