using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private static RespawnManager instance;
    public static RespawnManager Instance { get { return instance; } }

    public Vector2 defaultCheckPoint = new Vector2(-4f, -1.96f);
    public Vector2 lastCheckpoint = new Vector2(-4f, -1.96f);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
