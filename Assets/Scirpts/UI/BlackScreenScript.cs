using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenScript : MonoBehaviour
{
    [SerializeField] private AnimationCurve transperCurve;
    [SerializeField] private float curveTime = 0;
    [SerializeField] private int direction = 1;

    private Image blackScreen;
    private void Awake()
    {
        blackScreen = GetComponent<Image>();
    }
    public void Fade(int dir)
    {
        direction = dir;
    }

    private void Update()
    {
        curveTime += Time.deltaTime * direction;
        curveTime = Mathf.Clamp(curveTime, 0, 1);

        blackScreen.color = new Vector4(0, 0, 0, transperCurve.Evaluate(curveTime));
    }
}
