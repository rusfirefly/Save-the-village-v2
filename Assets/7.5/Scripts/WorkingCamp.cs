using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkingCamp : MonoBehaviour, ICamp, IWorkingPoints
{
    public bool isTrainig { get; set; }

    [SerializeField] private GameObject _workerPanel;
    private RectTransform _workerPanelPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _prefWorker;
    [SerializeField] private Material _outlineMaterial;

    [SerializeField] private Transform _castlePoints;
    
    public Transform goldPosition { get; set; }
    public Transform meatPosition { get; set; }
    public Transform woodPosition { get; set; }
    public Transform castlePosition { get; set; }
    

    private SpriteRenderer _spriteRender;
    private Material _default;
    private bool isSelected;
    private WorkMan _working;

    void Start()
    {
        SetDefaultMaterial();
        GetWorkerPosition();
    }

    private void GetWorkerPosition()
    {
        _workerPanelPosition = _workerPanel.GetComponent<RectTransform>();
    }

    public void Training(Enums.UnitType type)
    {
        CreateNewWorker();
        SpawnPostion();
        Transform position = GoToTarget(type);
        TargetForWork(position);
    }

    private Transform GoToTarget(Enums.UnitType type)
    {
        return type switch
        {
            Enums.UnitType.Gold => goldPosition,
            Enums.UnitType.Meat => meatPosition,
            Enums.UnitType.Wood => woodPosition,
            _ => _spawnPosition
        };
    }

    private void TargetForWork(Transform position)=> _working.SetNewPosition(position);
    public void NewSpawnPosition(Transform newSpawnPosition)=>_spawnPosition = newSpawnPosition;
    public bool IsSelectedCamp()=> isSelected;

    public void DeSelectedCamp()
    {
        isSelected = false;
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

    private void OnMouseDown()=>SelectetCamp();

    public void SelectetCamp()
    {
        isSelected = true;
        _spriteRender.material = _outlineMaterial;
        ShowWorkerPanel();
        SelectedBuilding.OnSelected(gameObject);
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
