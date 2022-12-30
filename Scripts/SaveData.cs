using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData {
    public Vector3 playerPosition;
    public float seed;
    public float time;
    public Slot[] inventory = new Slot[10];
}
