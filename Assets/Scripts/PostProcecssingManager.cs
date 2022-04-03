using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcecssingManager : MonoBehaviour
{
    //Instance
    private static PostProcecssingManager instance;
    public static PostProcecssingManager Instance { get { return instance; } }

    //Post Process Data
    [SerializeField] private VolumeProfile defaultPostProcessing = null;
    [SerializeField] private VolumeProfile RageModePostProcessing = null;

    private Volume postProcessVolume = null;

    //ColorAdjustments colorAdjustment;
    //Vignette vignette;
    //ChromaticAberration chromaticAberration;

    private void Awake()
    {
        instance = this;


        postProcessVolume = GetComponent<Volume>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //postProcessVolume.profile.TryGet(out colorAdjustment);
        //postProcessVolume.profile.TryGet(out vignette);
        //postProcessVolume.profile.TryGet(out chromaticAberration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRageMode()
    {
        //Rage Mode FX
        //colorAdjustment.active = true;
        //vignette.active = true;
        //chromaticAberration.active = true;
        postProcessVolume.profile = RageModePostProcessing;

    }
    public void DeactivateRageMode()
    {
        //Rage Mode FX
        //colorAdjustment.active = false;
        //vignette.active = false;
        //chromaticAberration.active = false;
        postProcessVolume.profile = defaultPostProcessing;
    }
}
