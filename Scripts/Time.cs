using System;
using System.Collections;
using UnityEngine;

public class Time : MonoBehaviour {
    // A day goes from 0 to 90, ~ 26 minutes
    public float currentTime = 0;
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
    }
    // Update is called once per frame
    void Update()
    {
        if(0 <= currentTime && currentTime < 30){
            times = Times.day;
        }
        else if(30 >= currentTime && currentTime < 70){
            times = Times.noon;
        }
        else if(currentTime >= 70) times = Times.night;

        StartCoroutine(UpdateTime());
        global.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 1 - (currentTime / 100);

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
        playerAround.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = -.5f + (currentTime / 100);  
        playerMiddle.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = -.5f + (currentTime / 100);
        }
        else{
            playerAround.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
            playerMiddle.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
        }
        if(playerAround.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity <= 0){
            playerAround.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
        }
        if(playerMiddle.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity <= 0){
            playerMiddle.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
        }
    }
}
