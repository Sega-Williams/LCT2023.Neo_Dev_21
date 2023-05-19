using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Speedometr : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedometerText, rpmText;
    [SerializeField] P_Controller p_Controller;

    private void Update() {
        speedometerText.SetText(p_Controller.currentSpeed + " км/ч");
        rpmText.SetText("об/м: " + (int)p_Controller.currentRpm);
    }
}
