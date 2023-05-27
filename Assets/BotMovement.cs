using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform[] _pathPoints;
    private int _index = 0;
    private float _minDistance = 10;
    bool stopMove;

    [SerializeField] GameObject path;

    private void Start()
    {
    }
    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();

        _pathPoints = new Transform[path.transform.childCount];
        for (int i = 0; i < _pathPoints.Length; i++)
        {
            _pathPoints[i] = path.transform.GetChild(i);
        }

    }
    private void Update()
    {
        Walk();
    }

    void Walk()
    {
        if (!stopMove)
        {
            if (Vector3.Distance(transform.position, _pathPoints[_index].position) < _minDistance)
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
            _agent.SetDestination(_pathPoints[_index].position);
        }
    }

}
