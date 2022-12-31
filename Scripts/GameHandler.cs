using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameHandler: MonoBehaviour {
    public string worldName;
    public string saveFileName;
    public PerlinNoiseMap perlinNoiseMap;
    public static GameHandler instance;
    // Start is called before the first frame update
    void Start() {
        saveFileName = string.Format("{0}.save", worldName);
    }

    // Update is called once per frame
    void Update() {
        instance = this;
    }
    public void SaveGame() {
        GameObject player = GameObject.FindObjectOfType < Movement > ().gameObject;

        SaveData data = new SaveData();
        data.playerPosition = player.transform.position;
        data.inventory = player.GetComponent < Building > ().inventory;
        data.time = player.GetComponent < Time > ().currentTime;
        data.seed = PerlinNoiseMap.instance.seed;
        for (int i = 0; i < Building.instance.placed.Count; i++) {
            if (Building.instance.placed[i].GetComponent < Item > () != null) {
                data.placedIDs.Add(Building.instance.placed[i].GetComponent < Item > ().ID);
                data.placedPositions.Add(Building.instance.placed[i].transform.position);
            } else {
                data.placedIDs.Add(11);
                data.placedPositions.Add(Building.instance.placed[i].transform.position);
            }
            data.placedPositions.Add(Building.instance.placed[i].transform.position);
        }
        string json = JsonUtility.ToJson(data);

        // Save the JSON data to a file
        File.WriteAllText("saves/" + saveFileName, json);
        Debug.Log(json);
    }
    public void LoadGame() {
        // Check if the save file exists
        if (File.Exists("saves/" + saveFileName)) {
            // Read the contents of the save file
            string json = File.ReadAllText("saves/" + saveFileName);

            // Convert the JSON data to a SceneData object
            SaveData data = JsonUtility.FromJson < SaveData > (json);
            perlinNoiseMap.seed = data.seed;

            GameObject player = GameObject.FindObjectOfType < Movement > ().gameObject;


            player.transform.position = data.playerPosition;
            player.GetComponent < Time > ().currentTime = data.time;
            player.GetComponent < Building > ().inventory = data.inventory;
            try {
                // for (int i = 0; i < data.placedIDs.Count; i++) {
                //     GameObject placed = new GameObject();
                //     if (data.placedIDs[i] != 7)
                //     {
                //         placed = Instantiate(Building.instance.blocks[data.placedIDs[i - 1]]);
                //     }
                //     else
                //     {
                //         placed = Instantiate(Building.instance.rocks);
                //     }
                //     placed.transform.position = data.placedPositions[i];

                // }
            } catch (System.Exception error) {
                Debug.Log("Having diffuclties lodaing the world");
            }
            Debug.Log("Game Loaded");
        }
    }
}