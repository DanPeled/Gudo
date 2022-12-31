using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData {
    public string worldName;
    public Vector3 playerPosition;
    public float seed;
    public float time;
    public List<int> placedIDs = new List<int>();
    public List<Vector3> placedPositions = new List<Vector3>();
    public Slot[] inventory = new Slot[10];
}
