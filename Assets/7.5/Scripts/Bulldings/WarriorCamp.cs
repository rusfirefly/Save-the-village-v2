using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorCamp : MonoBehaviour, ICamp, ISelecteble
{
    public float trainingTime { get; set; }
    public float trainingPrice { get; set; }
    public bool isTrainig { get; set; }
    private bool _isSelected;

    [SerializeField] private GameObject _warriorPanel;
    private RectTransform _warriorPanelPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _prefKnight;
    [SerializeField] private GameObject _prefArch;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private PlayerData _playerData;
  
    private SpriteRenderer _spriteRender;
    private Material _default;
    private Warrior _warrior;

    void Start()
    {
        SetDefaultMaterial();
        GetWorkerPosition();
    }

    private void GetWorkerPosition()
    {
        _warriorPanelPosition = _warriorPanel.GetComponent<RectTransform>();
    }

    public void NewSpawnPosition(Transform newSpawnPosition)
    {
        _spawnPosition = newSpawnPosition;
    }

    public void Training(Enums.UnitType type)
    {
        
        CreateNewKnight();
    }

    private void CreateNewKnight()
    {
        GameObject newWarrior = Instantiate(_prefKnight, _spawnPosition.transform.position, Quaternion.identity);
        _warrior = newWarrior.GetComponent<Warrior>();
        SetCharacteristicsWarrior(2,3,4);
        _warrior.GoToNewTargetPosition(_targetPosition);
    }

    private void SetCharacteristicsWarrior(int hp, int def, int atk)
    {
        _warrior.UpdateCharater(hp, def, atk, 1);
        _playerData.powerWarriors += _warrior.GetPowerWarror();
    }
    
    public void SetFirstWarrior() => _warrior.firstWarrior = true;

    public bool IsSelected()
    {
        return _isSelected;
    }

    public void DeSelected()
    {
        _isSelected = false;
        _spriteRender.material = _default;
        HideWorkerPanel();
        SelectedBuilding.selectedObject = null;
    }

    private void OnMouseDown()
    {
        Selectet();
    }

    private void SetDefaultMaterial()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _default = _spriteRender.material;

    }

    public void Selectet()
    {
        _isSelected = true;
        _spriteRender.material = _outlineMaterial;
        ShowWorkerPanel();
        SelectedBuilding.OnSelected(gameObject);
    }

    private void ShowWorkerPanel()
    {
        _warriorPanelPosition.localPosition = new Vector3(0, _warriorPanel.transform.localPosition.y);

    }
    private void HideWorkerPanel()
    {
        _warriorPanelPosition.localPosition = new Vector3(450, _warriorPanel.transform.localPosition.y);
    }

}
