using UnityEngine;
using System;
using SmartConsole;
public class Commands : CommandBehaviour {
    [Command]
    public void SetTime(float time){
        Time.instance.currentTime = time;
    }
    [Command]
    public void tp(float x, float y){
        if(x >= -194.13999938964845 && y >= -94.69999694824219 && x <= 201.86000061035157 && y <= 102.30000305175781){
            Movement.instance.gameObject.transform.position = new Vector3(x,y,Movement.instance.gameObject.transform.position.z);
        } else {
            Debug.Log("Invalid Position");
        }
    }
    [Command]
    public void Save(){
        GameHandler.instance.SaveGame();
        Debug.Log("Game Saved");
    }
    [Command]
    public void Load(){
        GameHandler.instance.LoadGame();
    }
}