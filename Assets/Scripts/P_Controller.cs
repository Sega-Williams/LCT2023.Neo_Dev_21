using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Controller : MonoBehaviour
{
    float gasPedal;
    public float breakPower;

    // Values to be assigned
    [Header("Assigned")]
    public WheelCollider[] wheelColls;
    public Transform[] wheelMeshs;
    

    // Rpm variable
    [Header("Rpm Variable")]
    public float maxRpm;
    public float currentRpm;
    public float fixedRpm;
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
    WheelHit wheelHit;
    Rigidbody rb;

    private void Start()
    {
        wheelHit = new WheelHit(); 

        rb = GetComponent<Rigidbody>();
        SelectWheelHit();
    }

    private void FixedUpdate() {
        // Automatic gear control
        AutomaticControl();

        // Rpm calculation
        CalculateRpm(startEngineRpm);

        // the process of wheel rotation     
        WheelSteer(30f * Input.GetAxis("Horizontal"));

        //Mesh tracking
        MeshMove();

        // Depending on the speed and calculate wheel rpm
        fixedRpm = ((CurrentSpeed() * 518) / 60) * (gearRatios[0] * gearRatios[gear]);
    }

    // void AutomaticControl ()
    // {
    //     gasPedal = Input.GetAxis("Gas") * maxRpm;
    //     AddBreakTorq(Input.GetAxisRaw("Break") * breakPower);

    //     if (fixedRpm < startEngineRpm - 100 && gear != gearRatios.Length - 1)
    //         gear = 0;

    //     if (Input.GetKeyDown(KeyCode.LeftShift))
    //     {
    //         gear = 0;
    //         reversingGear = 1;
    //     }
    //     if (Input.GetKeyDown(KeyCode.LeftControl) && gear < 2)
    //     {
    //         gear = gearRatios.Length - 1;
    //         reversingGear = -1;
    //     }

    //     if (gear > 0)
    //     {
    //         if (fixedRpm > nextGearRpm)
    //             if (gear < gearRatios.Length - 2)
    //             {
    //                 gear++;
    //                 reversingGear = 1;
    //             }
    //         if (fixedRpm < previousGearRpm)
    //             if (gear > 1 && gear != gearRatios.Length - 1)
    //             {
    //                 gear--;
    //                 reversingGear = 1;
    //             }
    //     }
    //     else
    //     {
    //         if (currentRpm > startEngineRpm + 100)
    //             gear = 1;
    //         reversingGear = 1;
    //     }

        
    //     if (fixedRpm < maxRpm - 500) // -500 olma sebebi kesiciye girmesi
    //         AddTorq(CalculateCurrentTorq(gear, gasPedal) * reversingGear);
    //     else
    //         AddTorq(CalculateCurrentTorq(gear, fixedRpm) * reversingGear);
    // }
        void AutomaticControl ()
    {
        gasPedal = Input.GetAxis("Vertical") * maxRpm;
        AddBreakTorq(Input.GetAxisRaw("Vertical") * breakPower);

        if (fixedRpm < startEngineRpm - 100 && gear != gearRatios.Length - 1)
            gear = 0;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            gear = 0;
            reversingGear = 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl) && gear < 2)
        {
            gear = gearRatios.Length - 1;
            reversingGear = -1;
        }

        if (gear > 0)
        {
            if (fixedRpm > nextGearRpm)
                if (gear < gearRatios.Length - 2)
                {
                    gear++;
                    reversingGear = 1;
                }
            if (fixedRpm < previousGearRpm)
                if (gear > 1 && gear != gearRatios.Length - 1)
                {
                    gear--;
                    reversingGear = 1;
                }
        }
        else
        {
            if (currentRpm > startEngineRpm + 100)
                gear = 1;
            reversingGear = 1;
        }

        
        if (fixedRpm < maxRpm - 500) // -500 olma sebebi kesiciye girmesi
            AddTorq(CalculateCurrentTorq(gear, gasPedal) * reversingGear);
        else
            AddTorq(CalculateCurrentTorq(gear, fixedRpm) * reversingGear);
    }
    
    // Break
    public void AddBreakTorq (float torq)
    {        
        if (torq == -1)
        {
            for (int i = 0; i < wheelColls.Length; i++)
                wheelColls[i].brakeTorque = fixedRpm;
        }
        else
        {
            for (int i = 0; i < wheelColls.Length; i++)
                wheelColls[i].brakeTorque = 0;

            wheelColls[wheelColls.Length - 1].brakeTorque = torq;
            wheelColls[wheelColls.Length - 2].brakeTorque = torq;
        }
    }
    // Add torq
    void AddTorq (float torq)
    {       
        for (int i = 0; i < wheelColls.Length; i++)
            wheelColls[i].motorTorque = torq;
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

    //  rpm calculation
    void CalculateRpm(float startEngineRpm)
    {
        float oran = HP / maxRpm;
        if (Input.GetAxisRaw("Gas") > 0)
        {
            if (currentRpm < maxRpm - 500)
                currentRpm += gasPedal * oran;
            if (currentRpm > maxRpm - 500)
                if (currentRpm != maxRpm - 500)
                    currentRpm -= 50f;
        }
        else
        {
            if (currentRpm >= startEngineRpm)
                currentRpm -= maxRpm * oran;
            if (currentRpm < startEngineRpm)
                currentRpm = startEngineRpm;
        }
    }
    
    // the process of wheel rotation
    void WheelSteer (float angel)
    {
        wheelColls[0].steerAngle = angel;
        wheelColls[1].steerAngle = angel;
    }
    void MeshMove ()
    {
        Vector3 pos;
        Quaternion quat;

        for(int i = 0; i < wheelColls.Length; i++)
        {
            wheelColls[i].GetWorldPose(out pos, out quat);
            wheelMeshs[i].position = pos;
            wheelMeshs[i].rotation = quat;
        }
    }

    // speed calculation 
    public float CurrentSpeed ()
    {
        float speed = rb.velocity.magnitude;
        speed *= 3.6f; //is multiplied by a 3.6 kmh
        return speed;
    }

    // Given the power to the wheels, selects one of 
    void SelectWheelHit()
    {
        wheelColls[wheelColls.Length - 1].GetGroundHit(out wheelHit);
    }
}
