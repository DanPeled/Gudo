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
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(UpdateTime());
        switch(times){
            case Times.day:
                if(global.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity != 0){
                    ResetLights();
            }
                setPlayerLightState(false);
                break;
            case Times.noon:
                setPlayerLightState(true);
                playerAround.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
                playerMiddle.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0.3f;
                break;
        }

    }
    IEnumerator UpdateTime()
    {
        yield return new WaitForSeconds(0.1f);
        currentTime += 0.1f;
    }
}
