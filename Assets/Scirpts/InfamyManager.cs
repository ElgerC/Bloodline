using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        if(infamyInstance == null)
        {
            infamyInstance = this;
            DontDestroyOnLoad(gameObject);
        }else 
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        civilians = FindObjectsOfType<CivilianScript>().ToList();
    }

    private void Update()
    {
        infamyLevel -= (civilians.Count*Time.deltaTime)/infamyReductionTickSpeed;
        infamyLevel = Mathf.Clamp(infamyLevel, 0, 100);

        infamyLevelSlider.value = infamyLevel;
    }

    public void ChangeInfamy(float change)
    {
        infamyLevel += change;
    }
}
