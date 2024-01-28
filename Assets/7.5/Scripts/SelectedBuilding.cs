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
        selectedObject?.GetComponent<ICamp>().DeSelectedCamp();
        selectedObject = gameObject;
        Selected?.Invoke(gameObject);
    }

    public static void AllCampsDeSelect()
    {
        selectedObject?.GetComponent<ICamp>().DeSelectedCamp();
        selectedObject = null;
        /*GameObject[] camps = GameObject.FindGameObjectsWithTag("Camp");
        foreach (GameObject camp in camps)
        {
            ICamp camp2;
            if (camp.TryGetComponent<ICamp>(out camp2))
            {
                camp2.DeSelectedCamp();
            }
        }*/
    }
}
