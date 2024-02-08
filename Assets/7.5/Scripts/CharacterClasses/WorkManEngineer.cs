using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkManEngineer : MonoBehaviour, IMovable
{
    private NavMeshAgent _agent;
    private Vector3 targetPosition;

    private void Start()
    {
        InitAgent();
    }

    private void InitAgent()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        //_agent.updateRotation = false;
        //_agent.updateUpAxis = false;
    }

    public void Move(Vector3 position)
    {
        
    }

    public void GoToNewTargetPosition(Transform newPosition)
    {
        
    }


}
