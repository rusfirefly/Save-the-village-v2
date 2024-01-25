using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestUnitNavMesh : MonoBehaviour
{

    private NavMeshAgent _agent;
    public GameObject target;

    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        _agent.destination = target.transform.position;
    }
}
