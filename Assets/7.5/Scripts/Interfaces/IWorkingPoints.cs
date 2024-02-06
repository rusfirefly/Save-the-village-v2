using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorkingPoints
{
    Transform GoldPosition { get; set; }
    Transform MeatPosition { get; set; }
    Transform WoodPosition { get; set; }

    public void NewSpawnPosition(Transform newSpawnPosition);
}
