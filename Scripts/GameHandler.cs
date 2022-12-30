using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameHandler : MonoBehaviour
{
    public string worldName;
    public string saveFileName;
    public static GameHandler instance;
    // Start is called before the first frame update
    void Start()
    {
        saveFileName = string.Format("{0}.save", worldName);
    }

    // Update is called once per frame
    void Update()
    {
        instance = this;
    }
    public void SaveGame(){
        GameObject player = GameObject.FindObjectOfType<Movement>().gameObject;

        SaveData data = new SaveData();
        
        data.playerPosition = player.transform.position;
        data.inventory = player.GetComponent<Building>().inventory;
        data.time = player.GetComponent<Time>().currentTime;
        data.seed = PerlinNoiseMap.instance.seed;

        string json = JsonUtility.ToJson(data);

        // Save the JSON data to a file
        File.WriteAllText(saveFileName, json);
    }
    public void LoadGame(){
        // Check if the save file exists
        if (File.Exists(saveFileName))
        {
            // Read the contents of the save file
            string json = File.ReadAllText(saveFileName);

            // Convert the JSON data to a SceneData object
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            GameObject player = GameObject.FindObjectOfType<Movement>().gameObject;
            

            player.transform.position = data.playerPosition;
            player.GetComponent<Time>().currentTime = data.time;
            PerlinNoiseMap.instance.seed = data.seed;
            player.GetComponent<Building>().inventory = data.inventory;
            Debug.Log("Game Loaded");
        }
    }
}
