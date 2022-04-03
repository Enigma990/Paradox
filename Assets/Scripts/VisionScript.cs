using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionScript : MonoBehaviour
{
    [SerializeField] private GameObject vision = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            vision.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
