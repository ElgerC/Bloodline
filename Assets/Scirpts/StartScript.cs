using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Level tutorial");
    }   
    
    public void QuitButton()
    {
        Application.Quit();
    }

    public void CreditsButton()
    {

    }
}
