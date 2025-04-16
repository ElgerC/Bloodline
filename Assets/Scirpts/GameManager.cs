using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region general
    public static GameManager instance;
    #endregion

    #region SceneManegment
    [Header("SceneManegment")]
    public int sceneIndex;
    #endregion
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneIndex);
        sceneIndex++;
    }
}
