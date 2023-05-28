using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraTarget : MonoBehaviour
{
    [SerializeField] P_Controller p_Controller;
    [SerializeField] float maxFrontDistance = 5f;

    private float startPosX;
    private float startPosY;
    private float startPosZ;

    private void Start() {
        startPosX = transform.localPosition.x;
        startPosY = transform.localPosition.y;
        startPosZ = transform.localPosition.z;
    }
    private void Update() {
        transform.localPosition = new Vector3(startPosX, startPosY, startPosZ + maxFrontDistance * (p_Controller.currentSpeed / p_Controller.maxSpeed));
    }
}
