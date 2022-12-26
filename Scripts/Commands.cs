using UnityEngine;
using System;
using SmartConsole;
public class Commands : CommandBehaviour {
    [Command]
    public void SetTime(float time){
        Time.instance.currentTime = time;
    }
    
}