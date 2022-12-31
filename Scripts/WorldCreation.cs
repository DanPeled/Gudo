using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
public class WorldCreation: MonoBehaviour {
    // Start is called before the first frame update
    public string[] saveFiles;
    public GameObject worldPrefab;
    void Start() {
        saveFiles = Directory.GetFiles("saves/", "*.save", SearchOption.TopDirectoryOnly);
        List < GameObject > worlds = new List < GameObject > ();
        saveFiles[0] = saveFiles[0].Replace(".save", "").Replace("saves/", "");
        worlds.Add(new GameObject());
        worlds[0] = Instantiate(worldPrefab);
        worlds[0].transform.SetParent(transform.parent);
        worlds[0].GetComponentInChildren < TextMeshProUGUI > ().text = saveFiles[0];
        worlds[0].transform.position = new Vector3(200, 900);
        if (saveFiles.Length > 1) {
            for (int i = 1; i < saveFiles.Length; i++) {
                saveFiles[i] = saveFiles[i].Replace(".save", "").Replace("saves/", "");
                worlds.Add(new GameObject());
                worlds[i] = Instantiate(worldPrefab);
                worlds[i].transform.SetParent(transform.parent);
                worlds[i].GetComponentInChildren < TextMeshProUGUI > ().text = saveFiles[i];
                worlds[i].transform.position = new Vector3(200, worlds[i-1].transform.position.y - 100);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}