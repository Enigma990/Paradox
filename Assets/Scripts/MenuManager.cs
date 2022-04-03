using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private static MenuManager instance = null;
    public static MenuManager Instance { get { return instance; } }

    [SerializeField] private GameObject deathMenu = null;

    private void Awake()
    {
        instance = this;
    }

    public void ActivateDeathMenu()
    {
        deathMenu.SetActive(true);
    }
    
    public void DeactivateDeathMenu()
    {
        deathMenu.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
