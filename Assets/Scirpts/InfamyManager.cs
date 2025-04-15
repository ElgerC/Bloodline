using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfamyManager : MonoBehaviour
{
    public static InfamyManager infamyInstance;
    public float infamyLevel;

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

    public void ChangeInfamy(float change)
    {
        infamyLevel += change;
    }
}
