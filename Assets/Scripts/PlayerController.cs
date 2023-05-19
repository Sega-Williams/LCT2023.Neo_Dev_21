using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject centerOfMass;

    private Rigidbody _rb;
    private TextMeshProUGUI speedometerText, rpmText;
    private float turnSpeed = 90.0f, speed; 
    private float horizontalInput, verticalInput;  
    private float horsePower = 1000; 
    public float rpm; 

    void Start()
    {
        _rb = GetComponent<Rigidbody>(); 
        _rb.centerOfMass = centerOfMass.transform.localPosition;
        speedometerText = GameObject.FindGameObjectWithTag("speedometer").GetComponent<TextMeshProUGUI>();
        rpmText = GameObject.FindGameObjectWithTag("rpm").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        _rb.AddRelativeForce(Vector3.forward * Time.deltaTime * horsePower * verticalInput);
        transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed * horizontalInput);
        speed = Mathf.Round(_rb.velocity.magnitude * 2.237f);
        rpm = Mathf.Round((speed % 30) * 40);
        speedometerText.SetText(speed + " км/ч");
        rpmText.SetText("об/м: " + rpm);
    }

}
