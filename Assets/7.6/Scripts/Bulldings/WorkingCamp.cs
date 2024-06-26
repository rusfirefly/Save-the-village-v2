using UnityEngine;
using UnityEngine.EventSystems;
using static Enums;

public class WorkingCamp : MonoBehaviour, ICamp, IWorkingPoints, ISelecteble
{
    [SerializeField] private GameObject _workerPanel;
    private RectTransform _workerPanelPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _prefWorker;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Transform _castlePoints;
    private SoundClip _sound;
    [SerializeField] private AudioClip _audioSelectBuild;
    [SerializeField] private WorkManEngineer _enginerPrefab;

    [SerializeField] private Transform _goldPosition;
    [SerializeField] private Transform _meatPosition;
    [SerializeField] private Transform _woodPosition;
    
    private SpriteRenderer _spriteRender;
    private Material _default;
    private bool _isSelected;
    private WorkMan _working;

    void Start()
    {
        SetDefaultMaterial();
        GetWorkerPosition();
        _sound = gameObject.GetComponent<SoundClip>();
        House.NeedEngineer += OnNeedEngineerEvent;
        TrainigButton.Traiding += OnTarianigFinishEvent;
    }

    private void OnDestroy()
    {
        House.NeedEngineer -= OnNeedEngineerEvent;
        TrainigButton.Traiding -= OnTarianigFinishEvent;
    }

    private void GetWorkerPosition()
    {
        _workerPanelPosition = _workerPanel.GetComponent<RectTransform>();
    }

    private void OnNeedEngineerEvent(Vector3 targetPosition)
    {
        WorkManEngineer unit = Instantiate(_enginerPrefab, _spawnPosition.transform.position, Quaternion.identity).GetComponent<WorkManEngineer>();
        unit.Move(targetPosition);
    }

    private void OnTarianigFinishEvent(UnitType type)
    {
        UnitType typeUnit = UnitType.None;
        switch (type)
        {
            case UnitType.Gold:
                typeUnit = type;
                break;
            case UnitType.Meat:
                typeUnit = type;
                break;
            case UnitType.Wood:
                typeUnit = type;
                break;
        }

        if (typeUnit != UnitType.None)
        {
            Training(typeUnit);
        }
    }

    public void Training(Enums.UnitType type)
    {
        CreateNewWorker();
        SpawnPostion();
        Transform position = GoToTarget(type);
        TargetForWork(position);
    }

    private Transform GoToTarget(UnitType type)
    {
        return type switch
        {
            UnitType.Gold => _goldPosition,
            UnitType.Meat => _meatPosition,
            UnitType.Wood => _woodPosition,
            _ => _spawnPosition
        };
    }

    private void TargetForWork(Transform position)=> _working.SetNewPosition(position);

    public void NewSpawnPosition(Transform newSpawnPosition)=>_spawnPosition = newSpawnPosition;

    public void DeSelected()
    {
        _isSelected = false;
        _spriteRender.material = _default;
        HideWorkerPanel();
        SelectedBuilding.selectedObject = null;
    }

    public WorkMan GetWorking() => _working;

    public void GoCastle()=> _working.GoCastle(_castlePoints);
    private void SetDefaultMaterial()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _default = _spriteRender.material;
    }

    private void CreateNewWorker()
    {
        GameObject newWorker = Instantiate(_prefWorker, _spawnPosition.transform.position, Quaternion.identity);
        _working = newWorker.GetComponent<WorkMan>();
    }
    
    private void SpawnPostion()
    {
        _working.NewStartPosition(_spawnPosition);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Selected();
    }

    public void Selected()
    {
        if (!_isSelected)
        {
            _sound.PlaySound(_audioSelectBuild);
            _isSelected = true;
            _spriteRender.material = _outlineMaterial;
            ShowWorkerPanel();
            SelectedBuilding.OnSelected(gameObject);
        }
    }

    private void ShowWorkerPanel()
    {
        _workerPanelPosition.localPosition = new Vector3(0, _workerPanel.transform.localPosition.y);
        
    }
    private void HideWorkerPanel()
    {
        _workerPanelPosition.localPosition = new Vector3(450, _workerPanel.transform.localPosition.y);
    }
}
