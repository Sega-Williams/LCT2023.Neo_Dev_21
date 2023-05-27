using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public Transform leftWheelMesh;
    public Transform rightWheelMesh;
    public bool motor;
    public bool steering;
}
    
public class P_Controller : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;

    // Controller
    [SerializeField] KeyCode key_Brake; 
    [SerializeField] int reversingSpeed = 30; // brake to reverse move


    // For UI
    [HideInInspector]
    public int currentSpeed;
    // Rpm variable
    [Header("Rpm Variable")]
    public float maxRpm;
    public float currentRpm;
    public float startEngineRpm;
    // Torq Variable
    [Header("Torq variable")]
    public float HP;
    [Tooltip("Rpm / Torq")]public AnimationCurve motorTorqGraphic;

    // Gear variable
    [Header ("Gear Settings")]
    public int gear;
    public float[] gearRatios;
    public int reversingGear;

    // Control type
    [Header("Controller")]
    public int nextGearRpm;
    public int previousGearRpm;

    // Private
    Rigidbody rb;

    // Gas Variables
    float motor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider, Transform visualWheel)
    {    
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
    
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
    
    public void FixedUpdate()
    {        
        MoveControl();
        CurrentSpeed();
    }

    // Controller logic
    private void MoveControl(){
        bool gasPedal = Input.GetAxisRaw("Vertical") != 0;
        int reversingGear = (int)Input.GetAxisRaw("Vertical");
        CalculateRpm(reversingGear);
        CalculateGear(reversingGear);
        motor = currentRpm * Input.GetAxisRaw("Vertical");
        
        float currTorq;
        if(motor == 0){
            currTorq = 0;
        }else{
            currTorq = CalculateCurrentTorq(gear, motor);
        }

        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
    
        foreach (AxleInfo axleInfo in axleInfos) {
            // Steering wheels
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            // Visual steering wheels
            ApplyLocalPositionToVisuals(axleInfo.leftWheel, axleInfo.leftWheelMesh);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel, axleInfo.rightWheelMesh);

            // Brake
            bool brake = false;
            if( (motor < 0 && currentSpeed > reversingSpeed) || Input.GetKey(key_Brake) || !gasPedal){
                // Debug.Log("Brake");
                brake = true;
            }

            axleInfo.leftWheel.brakeTorque = brake? maxRpm : !gasPedal? currentRpm : 0;
            axleInfo.rightWheel.brakeTorque = brake? maxRpm : !gasPedal? currentRpm : 0;

            // Gas
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = currTorq * reversingGear;
                axleInfo.rightWheel.motorTorque = currTorq * reversingGear;
            }
        }
    }
    // Gear calculation
    void CalculateGear(int reversingGear){
        if(reversingGear < 0 && currentSpeed < reversingSpeed){
            gear = gearRatios.Length - 1;

            return;
        }
        else if(currentRpm > gear * ((maxRpm - 1000)  / gearRatios.Length)  && gear < gearRatios.Length - 1){
            // Debug.Log("Gear Up");
            ++gear;
        }
        else if(currentRpm < gear * ((maxRpm + 1000) / gearRatios.Length) && gear > 1){
            // Debug.Log("Gear Down");
            --gear;
        }
        else if(currentRpm < startEngineRpm / 2){
            // Debug.Log("Gear Neutral");
            gear = 0;
        }
    }
    // Rpm calculation
    void CalculateRpm(int reversingGear)
    {
        float oran = HP / maxRpm;
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            // Debug.Log("RPM");
            if (currentRpm < maxRpm - 500)
                currentRpm += reversingGear * motor * oran;
            if (currentRpm > maxRpm - 500)
                if (currentRpm != maxRpm - 500)
                    currentRpm -= 50f;
        }
        else
        {
            // Debug.Log("Without RPM");
            if (currentRpm >= startEngineRpm)
                currentRpm -= maxRpm * oran;
            if (currentRpm < startEngineRpm)
                currentRpm = startEngineRpm;
        }
    }
    
    // Torque calculation
    float CalculateCurrentTorq(int gear, float rpm)
    {
        float currentTorqVoid = 0;

        if (gear != 0)
            currentTorqVoid = (gearRatios[0] * gearRatios[gear]) * motorTorqGraphic.Evaluate(rpm);
        else
            currentTorqVoid = 0;

        return currentTorqVoid;
    }

    // Speed calculation 
    public float CurrentSpeed ()
    {
        float speed = rb.velocity.magnitude;
        speed *= 3.6f; //is multiplied by a 3.6 kmh

        currentSpeed = (int)speed;

        return speed;
    }
}

