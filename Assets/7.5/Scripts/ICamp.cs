using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICamp
{
   
    bool isTrainig { get; set; }

    void Training(Enums.UnitType type);

    void NewSpawnPosition(Transform newSpawnPosition);

    bool IsSelectedCamp();

    void DeSelectedCamp();
    void SelectetCamp();

}
