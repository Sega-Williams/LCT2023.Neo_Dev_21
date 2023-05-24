using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class CarDriverAgent : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawnPosition;
    private CarDriver carDriver;
    private void Awake()
    {
        carDriver = GetComponent<CarDriver>();
    }
    public override void OnEpisodeBegin()
    {
        transform.position = spawnPosition.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        transform.forward = spawnPosition.forward;
        //trackCheckpoints.ResetCheckpoint(transform);
        carDriver.StopCompletely();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //Vector3 checkpointForward = trackCheckpoints.GetNextCheckpoint(transform).transform.forward;
        //float directionDot = Vector3.Dot(transform.forward, 0, checkpointForward);
        //sensor.AddObservation(directionDot);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = 0f;
        float turnAmount = 0f;
        switch (actions.DiscreteActions[0])
        {
            case 0:forwardAmount = 0f; break;
            case 1: forwardAmount = +1f;break;
            case 2:forwardAmount = -1f;break;
        }
        switch (actions.DiscreteActions[1])
        {
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;
        }
        carDriver.SetInputs(forwardAmount, turnAmount);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            forwardAction = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            forwardAction = 2;
        }
        int turnAction = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turnAction = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turnAction = 2;
        }
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.5f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Wall>(out Wall wall))
        {
            AddReward(-0.1f);
        }
    }
}
