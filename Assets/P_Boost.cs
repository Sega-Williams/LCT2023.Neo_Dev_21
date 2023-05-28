using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Boost : MonoBehaviour
{
    [SerializeField] KeyCode key_Nitro; 
    [SerializeField] private float maxVolumeNitro = 100f;
    [SerializeField] private float nitroRecovery = 3f;
    [SerializeField] private float nitroSpendPerTime = 20f;
    [SerializeField] private float torqMultiplier = 2.0f;

    public float currentNitro;

    private void Start() {
        currentNitro = maxVolumeNitro;
    }

    public float Nitro_TorqBoost_Multiplier(){
        if(Input.GetKey(key_Nitro)){
            if(currentNitro > 0){
                currentNitro -= nitroSpendPerTime * Time.fixedDeltaTime;

                return torqMultiplier; 
            }else{
                return 1;
            }
        }else{
            if(currentNitro < maxVolumeNitro) currentNitro += nitroRecovery * Time.fixedDeltaTime;
            if(currentNitro > maxVolumeNitro) currentNitro = maxVolumeNitro;

            // if nitro don't in use => multiplier = 1
            return 1;
        }
    }
}
