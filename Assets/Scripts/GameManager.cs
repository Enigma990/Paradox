using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    // Stamina Update Event
    public event UnityAction<float> UpdateStamina;
    public void OnUpdateStamina(float _amount)
    {
        if (UpdateStamina != null)
            UpdateStamina(_amount);
    }

    //Health Update Event
    public event UnityAction<float> UpdateHealth;
    public void OnUpdateHeatlh(float _amount)
    {
        if (PlayerController.Instance.inRageMode)
            return;

        if (UpdateHealth != null)
            UpdateHealth(_amount);
    }
}
