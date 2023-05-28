using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B_Controller : MonoBehaviour
{
    private float _minDistance = 10;
    private Transform[] _pathPoints;
    [SerializeField] GameObject path;
    private int _index = 0;
    bool stopMove;


    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    // Controller
    [SerializeField] KeyCode key_Brake;
    [SerializeField] KeyCode key_Boost;
    [SerializeField] int reversingSpeed = 3; // brake to reverse move


    // Rpm variable
    [Header("Rpm Variable")]
    public float maxRpm;
    public float currentRpm;
    public float startEngineRpm;
    // Torq Variable
    [Header("Torq variable")]
    public float HP;
    [Tooltip("Rpm / Torq")] public AnimationCurve motorTorqGraphic;

    // Gear variable
    [Header("Gear Settings")]
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

    //Agent
    NavMeshAgent _agent;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _pathPoints = new Transform[path.transform.childCount];
        for (int i = 0; i < _pathPoints.Length; i++)
        {
            _pathPoints[i] = path.transform.GetChild(i);
        }
        rb = GetComponent<Rigidbody>();
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

    public void Update()
    {
        MoveControl();
        CurrentSpeed();
    }

    // Controller logic
    bool manualBrake = true;
    private void MoveControl()
    {
        bool gasPedal = Vector3.Distance(transform.position, _pathPoints[_index].position) > _minDistance;

        // Manual brake
        if (gasPedal) manualBrake = false;

        // Calculatings
        //int reversingGear = (int)Input.GetAxisRaw("Vertical");
        int reversingGear = Vector3.Distance(transform.position, _pathPoints[_index].position) > _minDistance ? (int)_agent.speed : 0;
        CalculateRpm(reversingGear);
        CalculateGear(reversingGear);
        motor = currentRpm * _agent.acceleration;

        float currTorq;
        if (motor == 0)
        {
            currTorq = 0;
        }
        else
        {
            currTorq = CalculateCurrentTorq(gear, motor);
        }

        float steering = 0;
        //float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        //надо сделать зависимость от чего-то, что в какой-то момент будет +1, в какой то -1
        float motorAxleTorq = currTorq * reversingGear;
        float rear_BrakeTorque = manualBrake ? maxRpm * 2 : maxRpm;

        foreach (AxleInfo axleInfo in axleInfos)
        {
            // Steering wheels
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            // Visual steering wheels
            ApplyLocalPositionToVisuals(axleInfo.leftWheel, axleInfo.leftWheelMesh);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel, axleInfo.rightWheelMesh);

            // Brake
            bool brake = false;
            if (manualBrake || (motor != (motor * reversingGear) && _agent.speed > reversingSpeed))
            {
                // Debug.Log("Brake");
                brake = true;
            }

            // brake torque on steering wheels is less than rear
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.brakeTorque = brake ? maxRpm / 3 : 0;
                axleInfo.rightWheel.brakeTorque = brake ? maxRpm / 3 : 0;
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = brake ? rear_BrakeTorque : 0;
                axleInfo.rightWheel.brakeTorque = brake ? rear_BrakeTorque : 0;
            }

            // Gas
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor != 0 ? motorAxleTorq : axleInfo.leftWheel.motorTorque * 0.83f; // 0.83 - speed of lost torque
                axleInfo.rightWheel.motorTorque = motor != 0 ? motorAxleTorq : axleInfo.leftWheel.motorTorque * 0.83f; // 0.83 - speed of lost torque
                if (!stopMove && Vector3.Distance(transform.position, _pathPoints[_index].position) < _minDistance)
                {
                    if (_index + 1 != _pathPoints.Length)
                    {
                        _index++;
                    }
                    else
                    {
                        stopMove = true;
                        _agent.enabled = false;
                    }
                }
            }
            _agent.SetDestination(_pathPoints[_index].position);
        }
    }
    // Gear calculation
    void CalculateGear(int reversingGear)
    {
        if (reversingGear < 0 && _agent.speed < reversingSpeed)
        {
            gear = gearRatios.Length - 1;

            return;
        }
        else if (currentRpm > gear * ((maxRpm - 1000) / gearRatios.Length) && gear < gearRatios.Length - 1)
        {
            // Debug.Log("Gear Up");
            ++gear;
        }
        else if (currentRpm < gear * ((maxRpm + 1000) / gearRatios.Length) && gear > 1)
        {
            // Debug.Log("Gear Down");
            --gear;
        }
        else if (currentRpm < startEngineRpm / 2)
        {
            // Debug.Log("Gear Neutral");
            gear = 0;
        }
    }
    // Rpm calculation
    void CalculateRpm(int reversingGear)
    {
        float oran = HP / maxRpm;
        if (Vector3.Distance(transform.position, _pathPoints[_index].position) < _minDistance)
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
    public float CurrentSpeed()
    {
        //float speed = rb.velocity.magnitude;
        float speed = _agent.velocity.magnitude;
        speed *= 3.6f; //is multiplied by a 3.6 kmh

        _agent.speed = (int)speed;

        return speed;
    }


}

