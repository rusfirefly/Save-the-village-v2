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

        GameHadler.ReloadAll += OnReloadAll;
    }

    private void Start()
    {
        InitAgent();
        SetStartPosition();
        GetSoundEntity();
        PlaySoundNewEntity();
    }

    private void Update()
    {
        AgentMove();
    }

    private void OnDestroy()
    {
        GameHadler.ReloadAll -= OnReloadAll;
    }

    private void InitAgent()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }
    
    public void MoveTo(Transform point)
    {
        _targetPosition = point;
    }

    public void NewStartPosition(Transform startPoint)
    {
        _startPosition = startPoint;
    }
    
    public virtual void StartedWork(Collider2D collider)
    {
        Debug.Log("��������!!!");
    }

    private void GetSoundEntity() => _soundEntity = gameObject.GetComponent<SoundEntity>();
    
    private void PlaySoundNewEntity() => _soundEntity.PlaySoundNewEntity();

    private void SetStartPosition()
    {
        transform.position = _startPosition.transform.position;
    }
    
    private void AgentMove()
    {
        if (_targetPosition != null)
        {
            _agent.destination = _targetPosition.transform.position;
        }
    }

    private void OnReloadAll()
    {
        Destroy(gameObject);
    }

}

