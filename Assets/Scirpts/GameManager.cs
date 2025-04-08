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
    [SerializeField] private List<string> scenes = new List<string>();
    [SerializeField] private int sceneIndex;
    #endregion
    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
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
