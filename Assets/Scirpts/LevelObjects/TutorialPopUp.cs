using TMPro;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private TextMeshProUGUI textContainer;

    private void OnTriggerEnter(Collider other)
    {
        textContainer.gameObject.SetActive(true);
        textContainer.text = text;
    }

    private void OnTriggerExit(Collider other)
    {
        textContainer.gameObject.SetActive(false);
    }
}

