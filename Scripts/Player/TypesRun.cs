using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypesRun: MonoBehaviour {
    public GameObject[] sprites;
    public GameObject typeSetup(int index) {
        Debug.Log(Types.instance.names[index] + " " + Types.instance.dmg[index].ToString());
        return sprites[index];
    }
    
}