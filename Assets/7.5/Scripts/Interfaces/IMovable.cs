using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    void Move(Transform position);
    void GoToNewTargetPosition(Transform newPosition);
}
