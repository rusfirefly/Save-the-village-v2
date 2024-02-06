using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WarriorCamp : MonoBehaviour, ICamp, ISelecteble
{
    private bool _isSelected;

    [SerializeField] private GameObject _warriorPanel;
    private RectTransform _warriorPanelPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _prefKnight;
    [SerializeField] private GameObject _prefArch;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _archerPosition;
    [SerializeField] private PlayerData _playerData;
  
    private SpriteRenderer _spriteRender;
    private Material _default;
    private Warrior _warrior;
    private Archer _archer;
    private SoundClip _sound;

    [SerializeField] private GameObject _pointPref;
    [SerializeField] private GameObject _archerPointPref;

    void Start()
    {
        SetDefaultMaterial();
        GetWorkerPosition();
        _sound = gameObject.GetComponent<SoundClip>();
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
        if (type == Enums.UnitType.Archer)
            CreateNewArch();
        else 
            CreateNewKnight();
    }

    private void CreateNewKnight()
    {
        _warrior = Instantiate(_prefKnight, _spawnPosition.transform.position, Quaternion.identity).GetComponent<Warrior>(); 
        _warrior.GoToNewTargetPosition(_pointPref.transform);
    }
    private void CreateNewArch()
    {
        _archer = Instantiate(_prefArch, _spawnPosition.transform.position, Quaternion.identity).GetComponent<Archer>(); 
        _archer.GoToNewTargetPosition(_archerPointPref.transform);
    }

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
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Selected();
    }

    private void SetDefaultMaterial()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _default = _spriteRender.material;
    }

    public void Selected()
    {
        if (!_isSelected)
        {
            _isSelected = true;
            _sound.PlaySound();
            _spriteRender.material = _outlineMaterial;
            ShowWorkerPanel();
            SelectedBuilding.OnSelected(gameObject);
        }
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
