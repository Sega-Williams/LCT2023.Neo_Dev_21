using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD_Speedometr : MonoBehaviour
{
    TextMeshProUGUI speedometerText, rpmText;
    P_Controller p_Controller;
    private void Start()
    {
        speedometerText = GameObject.FindGameObjectWithTag("speedometer").GetComponent<TextMeshProUGUI>();
        rpmText = GameObject.FindGameObjectWithTag("rpm").GetComponent<TextMeshProUGUI>();
        p_Controller = FindObjectOfType<P_Controller>();
    }
    private void Update() {
        speedometerText.SetText(p_Controller.currentSpeed + " км/ч");
        rpmText.SetText("об/м: " + (int)p_Controller.currentRpm);
    }
}
