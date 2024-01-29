using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectedBuilding
{
    public static GameObject selectedObject;
    public static event Action<GameObject> Selected;

    public static void OnSelected(GameObject gameObject)
    {
        selectedObject?.GetComponent<ISelecteble>().DeSelected();
        selectedObject = gameObject;
        Selected?.Invoke(gameObject);
    }

    public static void AllCampsDeSelect()
    {
        selectedObject?.GetComponent<ISelecteble>().DeSelected();
        selectedObject = null;
    }
}
