using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sets_Audio : MonoBehaviour
{
    [SerializeField] private Slider VolumeSlider;

    private void Start() {
        VolumeSlider.onValueChanged.AddListener((float value)=>{
            Global_AudioController.Instance.GlobalVolumeChanged.Invoke(value);
        });
    }
}
