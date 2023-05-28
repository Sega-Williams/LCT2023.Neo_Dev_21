using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCar : MonoBehaviour
{
    float _speed = 10;
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * _speed * Time.deltaTime);
    }
}
