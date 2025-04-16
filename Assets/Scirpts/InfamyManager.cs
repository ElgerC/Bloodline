using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InfamyManager : MonoBehaviour
{
    public static InfamyManager infamyInstance;
    public float infamyLevel;
    public float infamyReductionTickSpeed;

    public List<CivilianScript> civilians = new List<CivilianScript>();

    [SerializeField] private Slider infamyLevelSlider;

    private void OnEnable()
    {
        if (infamyInstance == null)
        {
            infamyInstance = this;

            SceneManager.sceneLoaded += OnSceneLoaded;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        civilians = FindObjectsOfType<CivilianScript>().ToList();
        infamyLevelSlider = GameObject.FindWithTag("InfamyBar").GetComponent<Slider>();
    }

    private void Update()
    {
        if (civilians.Count > 0)
            infamyLevel -= (civilians.Count * Time.deltaTime) / infamyReductionTickSpeed;
        else
            infamyLevel -= (1 * Time.deltaTime) / infamyReductionTickSpeed;
        infamyLevel = Mathf.Clamp(infamyLevel, 0, 100);

        infamyLevelSlider.value = infamyLevel;
    }

    public void ChangeInfamy(float change)
    {
        infamyLevel += change;
    }
}
