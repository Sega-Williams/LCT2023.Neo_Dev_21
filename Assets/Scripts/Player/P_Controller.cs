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
    public float steeringAnglePerSecond;    

    // Controller
    [SerializeField] KeyCode key_Brake; 
    [SerializeField] int reversingSpeed = 30; // brake to reverse move
    [SerializeField] public float maxSpeed = 250f;


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
    P_Boost nitro;

    // Gas Variables
    float motor;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nitro = GetComponent<P_Boost>();
        manualBrake = true;
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
    bool manualBrake = true;
    private void MoveControl(){
        bool gasPedal = Input.GetAxisRaw("Vertical") != 0;

        // Manual brake
        if(gasPedal) manualBrake = false;
        if(Input.GetKey(key_Brake)){
            manualBrake = true;
        }

        // Calculatings
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

        float steering = maxSteeringAngle * Input.GetAxisRaw("Horizontal");
        
        // formula of steering by speed
        steering *= (maxSpeed + 10 - currentSpeed) / maxSpeed;

        float motorAxleTorq = currTorq * reversingGear * nitro.Nitro_TorqBoost_Multiplier();
        float front_BrakeTorque = manualBrake? maxRpm * 0.5f : maxRpm;
        float rear_BrakeTorque = manualBrake? maxRpm * 2 : maxRpm;

        foreach (AxleInfo axleInfo in axleInfos) {
            // Steering wheels
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = Mathf.Lerp(axleInfo.leftWheel.steerAngle, steering, steeringAnglePerSecond * Time.fixedDeltaTime);
                axleInfo.rightWheel.steerAngle = Mathf.Lerp(axleInfo.rightWheel.steerAngle, steering, steeringAnglePerSecond * Time.fixedDeltaTime);
            }
            // Visual steering wheels
            ApplyLocalPositionToVisuals(axleInfo.leftWheel, axleInfo.leftWheelMesh);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel, axleInfo.rightWheelMesh);

            // Brake
            bool brake = false;
            if( manualBrake || (motor != (motor * reversingGear) && currentSpeed > reversingSpeed)){
                // Debug.Log("Brake");
                brake = true;
            }

            // brake torque on steering wheels is less than rear
            if(axleInfo.steering){
                axleInfo.leftWheel.brakeTorque = brake? front_BrakeTorque : 0;
                axleInfo.rightWheel.brakeTorque = brake? front_BrakeTorque : 0;
            }else{
                axleInfo.leftWheel.brakeTorque = brake? rear_BrakeTorque : 0;
                axleInfo.rightWheel.brakeTorque = brake? rear_BrakeTorque : 0;
            }

            // Gas
            if(currentSpeed < maxSpeed){
                if (axleInfo.motor) {
                    axleInfo.leftWheel.motorTorque = motor != 0? motorAxleTorq : axleInfo.leftWheel.motorTorque * 0.83f; // 0.83 - speed of lost torque
                    axleInfo.rightWheel.motorTorque = motor != 0? motorAxleTorq : axleInfo.leftWheel.motorTorque * 0.83f; // 0.83 - speed of lost torque
                }
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
            currentTorqVoid = (gearRatios[0] / gearRatios[gear]) * motorTorqGraphic.Evaluate(rpm);
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

    public float GetRPM(){
        return currentRpm < maxRpm? currentRpm / maxRpm : Random.Range(.95f, 1f);
    }
    public float GetSpeed(){
        return currentSpeed < maxSpeed? currentSpeed / maxSpeed : Random.Range(.95f, 1f);
    }
}

