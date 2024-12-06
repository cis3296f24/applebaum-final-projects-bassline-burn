using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCarSFXHandler : MonoBehaviour
{
    // Start is called before the first frame update


    public AudioSource screechAudioSource;
    public AudioSource engineAudioSource;
    public AudioSource hitAudioSource;
    public KartController player;

    float enginePitch = 0.5f;
    float screechPitch = 0.5f;

    void awake(){
        player = GetComponentInParent<KartController>();
    }
    
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEngineSFX();
        UpdateScreechSFX();
    }

    void UpdateEngineSFX(){
        float velocityMagnitude = player.GetVelocityMagnitude();
        float engineVolumeMax = velocityMagnitude*0.05f;
        engineVolumeMax = Mathf.Clamp(engineVolumeMax, 0.2f,1.0f);
        engineAudioSource.volume = Mathf.Lerp(engineAudioSource.volume, engineVolumeMax, Time.deltaTime*10);
        enginePitch = velocityMagnitude *0.2f;
        enginePitch = Mathf.Clamp(enginePitch, 0.6f,2.4f);
        engineAudioSource.pitch = Mathf.Lerp(engineAudioSource.pitch, enginePitch, Time.deltaTime*2);

    }
    void UpdateScreechSFX(){
        if(player.IsTireSchreeching(out float lateralVelocity, out bool isBraking, out bool isBoosting)){
            if(isBraking){
                screechAudioSource.volume = Mathf.Lerp(screechAudioSource.volume, 1.0f, Time.deltaTime*10);
                screechAudioSource.pitch = Mathf.Lerp(screechAudioSource.pitch, 0.5f, Time.deltaTime*10);
            }
            else{
                screechAudioSource.volume = Mathf.Abs(lateralVelocity)*0.1f;
                screechAudioSource.pitch = Mathf.Abs(lateralVelocity)*0.02f;
            }

        }
        else{
            screechAudioSource.volume =  Mathf.Lerp(screechAudioSource.volume, 0, Time.deltaTime*10);
        }

    }
}
