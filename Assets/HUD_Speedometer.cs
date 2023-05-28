using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Speedometer : MonoBehaviour
{
    private Image speedometer_Image;   
    [SerializeField] private P_Controller p_Controller;

    // Start is called before the first frame update
    void Start()
    {
        speedometer_Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        speedometer_Image.fillAmount = p_Controller.GetSpeed();
    }
}
