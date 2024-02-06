using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICamp
{
    void Training(Enums.UnitType type);

    void NewSpawnPosition(Transform newSpawnPosition);
}
