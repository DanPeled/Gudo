using System.Diagnostics;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time : MonoBehaviour {
    public float currentTime = 0;
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
        StartCoroutine(UpdateTime());
        global.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 1 - (currentTime / 100);

    }
    IEnumerator UpdateTime()
    {
        if(day){
            currentTime += 0.01f;
        }
        else currentTime -= 1;
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
        playerAround.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0 + (currentTime / 100);  
        playerMiddle.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0 + (currentTime / 100);
        }
    }
}
