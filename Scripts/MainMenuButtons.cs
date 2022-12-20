using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuButtons : MonoBehaviour
{
    public void Quit(){
        Application.Quit();
    }
    public void ChangeToScene(string scene){
        SceneManager.LoadScene(scene);
    }
    public void OpenURL(string URL){
        Application.OpenURL(URL);
    }
}
