using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Stamina Bar Data
    [SerializeField] private GameObject staminaBar = null;
    private UnityEngine.UI.Image staminaBarImage = null;

    //Health Bar Data
    [SerializeField] private GameObject healthBar = null;
    private UnityEngine.UI.Image healthBarImage = null;
    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        //--------------------------Stamina Bar-------------------------------
        //Initializing Stamina Bar Image
        staminaBarImage = staminaBar.GetComponent<UnityEngine.UI.Image>();
        // Adding Stamina Update to event
        GameManager.Instance.UpdateStamina += UpdateStaminaBar;

        //--------------------------Health Bar---------------------------------
        //Initializating Health Bar Image
        healthBarImage = healthBar.GetComponent<UnityEngine.UI.Image>();
        // Adding Health Update to event
        GameManager.Instance.UpdateHealth += UpdateHealthBar;
        currentHealth = 100f;
    }

    
    // Update Stamina Bar 
    void UpdateStaminaBar(float _amount)
    {
        staminaBarImage.fillAmount = PlayerController.Instance.CurrentStamina / 100;
    }

    //Update Health Bar
    void UpdateHealthBar(float _amount)
    {
        currentHealth += _amount;
        healthBarImage.fillAmount = currentHealth / 100;
    }
}
