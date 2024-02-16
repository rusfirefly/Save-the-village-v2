using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// правильно раставляем всех юнитов по слоям. 
/// сортировка.
/// </summary>
/// 

public class EntitySortingLayerOrder : MonoBehaviour
{
    [SerializeField] private float _timeUpdateSortingLayer = 1f;

    private EntityInfo[] _entityPosition;
    private Entity[] _entitys;

    private void Start()
    {
        StartCoroutine(UpdateEntityLayerOrder());
    }

    private IEnumerator UpdateEntityLayerOrder()
    {
        SortingLayerEntity();
        yield return new WaitForSeconds(_timeUpdateSortingLayer);
        StartCoroutine(UpdateEntityLayerOrder());
    }

    private void SortingLayerEntity()
    {
        FindAllEntity();
        GetEntityPositionY();
        SortingEntity();
        Clear();
    }

    private void FindAllEntity()=> _entitys = FindObjectsOfType<Entity>();

    private void GetEntityPositionY()
    {
        if (_entitys.Length > 0)
            _entityPosition = new EntityInfo[_entitys.Length];

        int index = 0;
        foreach (Entity entity in _entitys)
        {
            _entityPosition[index].entity = entity;
            _entityPosition[index].positionY = entity.transform.localPosition.y;
            index++;
        }
    }

    private void SortingEntity()
    {
        if (_entityPosition != null)
        {
            QuickSort(_entityPosition, 0, _entityPosition.Length - 1);
            for (int i = 0; i < _entityPosition.Length; i++)
            {
                SetNewLayerOrder(i);
            }
        }
    }

    private void SetNewLayerOrder(int index)=>
                 _entityPosition[index].entity.SetNewLayer(_entityPosition.Length - index);

    private void Clear()
    {
        _entityPosition = null;
        _entitys = null;
    }

    public static void QuickSort(EntityInfo[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right);
            if (pivot > 0)
                QuickSort(arr, left, pivot - 1);
            if (pivot < arr.Length - 1)
                QuickSort(arr, pivot + 1, right);
        }
    }

    public static int Partition(EntityInfo[] arr, int left, int right)
    {
        float pivot = arr[left].positionY;
        while (true)
        {
            while (arr[left].positionY < pivot)
                left++;
            while (arr[right].positionY > pivot)
                right--;
            if (left < right)
            {
                if (arr[left].positionY == arr[right].positionY)
                    return right;

                EntityInfo temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
            }
            else
            {
                return right;
            }
        }
    }
}
