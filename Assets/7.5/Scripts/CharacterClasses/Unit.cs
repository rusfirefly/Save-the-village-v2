using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private protected NavMeshAgent _agent;
    private Transform _startPosition;
    private Transform _targetPosition;
    private SoundEntity _soundEntity;

    private void Awake()
    {
        NewStartPosition(_startPosition);
    }

    private void Start()
    {
        InitAgent();
        SetStartPosition();
        GetSoundEntity();
        PlaySoundNewEntity();
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

    private void GetSoundEntity() => _soundEntity = gameObject.GetComponent<SoundEntity>();
    private void PlaySoundNewEntity() => _soundEntity.PlaySoundNewEntity();

    private void SetStartPosition()
    {
        transform.position = _startPosition.transform.position;
    }

    public virtual void StartedWork(Collider2D collider)
    {
        Debug.Log("Работать!!!");
    }

    private void AgentMove()
    {
        if (_targetPosition != null)
        {
            _agent.destination = _targetPosition.transform.position;
        }
    }

}

