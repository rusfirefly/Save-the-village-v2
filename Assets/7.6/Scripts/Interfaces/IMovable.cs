using UnityEngine;

public interface IMovable
{
    void Move(Vector3 position);

    void GoToNewTargetPosition(Transform newPosition);
}
