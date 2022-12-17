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
    public bool isChanging;
    public bool day;
    public Times times;
    public GameObject playerAround, global, playerMiddle;
    public void setPlayerLightState(bool state){
        playerAround.SetActive(state);
        playerMiddle.SetActive(state);
    }
    public void ResetLights(){
        for(float i = 0; i<1; i += 0.1f){
            StartCoroutine(ResetLight(i));
        }
        IEnumerator ResetLight(float i){
            playerAround.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
            playerMiddle.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
            global.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = i;
            yield return new WaitForSeconds(0.5f);
        }
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
        else currentTime -= 2;
        yield return new WaitForSeconds(1);
        if(currentTime >= 90 || !day){
            currentTime = 80;
        }
        if(currentTime <= 0){
            day = true;
        }
    }
}
