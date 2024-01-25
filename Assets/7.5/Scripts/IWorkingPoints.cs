using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorkingPoints
{
    Transform goldPosition { get; set; }
    Transform meatPosition { get; set; }
    Transform woodPosition { get; set; }

    public void NewSpawnPosition(Transform newSpawnPosition);
}
