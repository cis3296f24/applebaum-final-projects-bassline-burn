using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkWheelTrail : MonoBehaviour
{
    KartController player;
    TrailRenderer trailRenderer;

    void Awake(){
        player= GetComponentInParent<KartController>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.IsTireSchreeching(out float lateralVelocity, out bool isBoosting, out bool isBraking)){
            trailRenderer.emitting = true;
        }
        else{
            trailRenderer.emitting = false;
        }
    }
}
