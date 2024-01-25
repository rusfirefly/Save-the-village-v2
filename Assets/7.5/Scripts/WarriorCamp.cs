using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorCamp : MonoBehaviour,ICamp
{
    public Action Selected;
    public float trainingTime { get; set; }
    public float trainingPrice { get; set; }
    public bool isTrainig { get; set; }
    private bool isSelected;

    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _prefKnight;
    [SerializeField] private GameObject _prefArch;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Transform _targetPosition;

    private SpriteRenderer _spriteRender;
    private Material _default;
    private Warrior _warrior;

    void Start()
    {
        SetDefaultMaterial();

    }

    public void NewSpawnPosition(Transform newSpawnPosition)
    {

    }

    public void Training(Enums.UnitType type)
    {
        CreateNewKnight();
    }

    private void CreateNewKnight()
    {
        GameObject newWarrior = Instantiate(_prefKnight, _spawnPosition.transform.position, Quaternion.identity);
        _warrior = newWarrior.GetComponent<Warrior>();
        SetCharacteristicsWarrior(2,2,4);
        _warrior.SetTargetPosition(_targetPosition);

    }

    private void SetCharacteristicsWarrior(int hp, int def, int atk)=>_warrior.InitEnemy(hp, def, atk, 1);
    
    public void SetFirstWarrior() => _warrior.firstWarrior = true;

    public bool IsSelectedCamp()
    {
        return isSelected;
    }

    public void DeSelectedCamp()
    {
        isSelected = false;
        _spriteRender.material = _default;
    }

    private void OnMouseDown()
    {
        isSelected = true;
        _spriteRender.material = _outlineMaterial;
        Selected?.Invoke();
    }

    private void SetDefaultMaterial()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _default = _spriteRender.material;

    }

    public void Training()
    {
        
    }
}
