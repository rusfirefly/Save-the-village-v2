using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class WorkManEngineer : MonoBehaviour, IMovable
{
    
    public static int CountWork { get; private set; }
    public static Action PopulationUpdate;
    [SerializeField] private Animator _animator;
    private Vector3 _spawnPosition;
    private bool _complete;
    private NavMeshAgent _agent;

    private void Awake()
    {
        InitAgent();
        CountWork++;
        PopulationUpdate?.Invoke();
    }

    private void Start()
    {
        _spawnPosition = transform.position;
        if (transform.rotation.x == -90)
            transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void OnValidate()
    {
        _animator ??= gameObject.GetComponent<Animator>();
    }
    private void InitAgent()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        if (_agent == null) return;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    public void Move(Vector3 position)
    {
        if (_agent)
            _agent.destination = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Camp" && _complete)
        {
            CountWork--;
            PopulationUpdate?.Invoke();
            Destroy(gameObject);
        }
    }

    public void GoToNewTargetPosition(Transform newPosition)
    {
        Move(newPosition.position);
    }

    public void StartWork()
    {
        _animator.SetBool("StartBuilding", true);
    }

    public void StopWork()
    {
        _animator.SetBool("StartBuilding", false);
        _agent.destination = _spawnPosition;
        _complete = true;
    }
}
