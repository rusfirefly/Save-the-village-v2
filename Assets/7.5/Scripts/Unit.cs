using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    //---------IAtributes------------------
    public int def { get; set; }
    public int atk { get; set; }
    public int hp { get; set; }
    //-------------end IAtributes-----------

    private protected NavMeshAgent _agent;
    private Transform _startPosition;
    private Transform _targetPosition;

    private void Awake()
    {
        NewStartPosition(_startPosition);
    }

    private void Start()
    {
        InitAgent();
        SetStartPosition();
    }
    
    private void InitAgent()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        AgentMove();
    }

    public void MoveTo(Transform point)
    {
        _targetPosition = point;
    }

    public void NewStartPosition(Transform startPoint)
    {
        _startPosition = startPoint;
    }

    private void SetStartPosition()
    {
        transform.position = _startPosition.transform.position;
    }

    public virtual void StartedWork(Collider2D collider)
    {
        Debug.Log("��������!!!");
    }

    private void AgentMove()
    {
        if (_targetPosition != null)
        {
            _agent.destination = _targetPosition.transform.position;
        }
    }

}

