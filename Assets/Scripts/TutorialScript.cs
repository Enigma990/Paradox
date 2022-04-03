using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI tutorialText = null;
    [SerializeField] private GameObject panel = null;
    [SerializeField] private string text = null;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorialText.text = text;
            panel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            panel.SetActive(false);
        }
    }
}
