using System;
using System.Collections;
using UnityEngine;
using SmartConsole;
using UnityEngine.Rendering.Universal;
public class Time : MonoBehaviour{
    // A day goes from 0 to 90, ~ 26 minutes
    [Range(0f,90f)] public float currentTime = 0;
    public static Time instance;
    public GameObject clock;
    public enum Times{
        day = 0,
        noon = 550,
        night = 900
    }
    public bool day;
    public Times times;
    public GameObject playerAround, global, playerMiddle;
    public void setPlayerLightState(bool state){
        playerAround.SetActive(state);
        playerMiddle.SetActive(state);
    }
    public void Start(){
        setPlayerLightState(false);
        SetStateForAllTorches(false);
    }
    // Update is called once per frame
    void Update()
    {
        instance = this;
        if(0 <= currentTime && currentTime < 30){
            times = Times.day;
        }
        else if(30 >= currentTime && currentTime < 70){
            times = Times.noon;
        }
        else if(currentTime >= 70) times = Times.night;
        if(GetComponent<Movement>().playerActive)
            StartCoroutine(UpdateTime());
        global.GetComponent<Light2D>().intensity = 1 - (currentTime / 100);

    }
    IEnumerator UpdateTime()
    {
        if(day){
            currentTime += 0.0005f;
        }
        else currentTime -= 0.07f;
        yield return new WaitForSeconds(1);
        if(currentTime >= 90 && !day){
            currentTime = 80;
        }
        else if(currentTime >= 90){
            day = false;
        }
        else if(currentTime <= 0){
            currentTime = 0;
            day = true;
        }
        if(currentTime >= 30){
        setPlayerLightState(true);
        playerAround.GetComponent<Light2D>().intensity = -.5f + (currentTime / 100);  
        playerMiddle.GetComponent<Light2D>().intensity = -.5f + (currentTime / 100);
        
        setIntensityForAllTorches(playerMiddle.GetComponent<Light2D>().intensity);
        SetStateForAllTorches(true); // Makes all the torches active in the scene
        }
        else{
            playerAround.GetComponent<Light2D>().intensity = 0;
            playerMiddle.GetComponent<Light2D>().intensity = 0;
            setIntensityForAllTorches(0);
            SetStateForAllTorches(false);
        }
        if(playerAround.GetComponent<Light2D>().intensity <= 0){
            playerAround.GetComponent<Light2D>().intensity = 0;
        }
        if(playerMiddle.GetComponent<Light2D>().intensity <= 0){
            playerMiddle.GetComponent<Light2D>().intensity = 0;
            setIntensityForAllTorches(0);
        }
    }
    void setIntensityForAllTorches(float intensity){
        try{
            for(int i = 0; i < GameObject.FindGameObjectsWithTag("torch").Length; i++){ // Loops through every torch in the game
                    GameObject.FindGameObjectsWithTag("torch")[i].GetComponentInChildren<Light2D>().intensity = intensity; // Sets the intensity of the torch to the intesity var.
            }
        }
        catch{
            
        }
    }
    void SetStateForAllTorches(bool state){
        try{
            for(int i = 0; i < GameObject.FindGameObjectsWithTag("torch").Length; i++){ // Loops through every torch in the game
                    GameObject.FindGameObjectsWithTag("torch")[i].GetComponentInChildren<Light2D>().enabled = state; // Sets the torch's light enables state for the state var
            }
        }
        catch{
            
        }
    }
    
}
