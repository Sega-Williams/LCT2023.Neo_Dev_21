using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_RPM : MonoBehaviour
{
    private Image rpm_Image;   
    [SerializeField] private P_Controller p_Controller;

    // Start is called before the first frame update
    void Start()
    {
        rpm_Image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        rpm_Image.fillAmount = p_Controller.GetRPM();
    }
}
