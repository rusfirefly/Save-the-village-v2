using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdownTimer : MonoBehaviour
{
    public float currentTime;
    public float workingTime { get; set; }
    public bool isFinish { get; set; }

    private void Start()
    {
        isFinish = true;
    }

    void Update()
    {
        if (!isFinish)
        {
            Debug.Log("OK");
            currentTime += Time.deltaTime;
            if (currentTime >= workingTime)
            {
                Debug.Log("Finish Mining");
                isFinish = true;
                currentTime = 0;
            }
        }
    }

}
