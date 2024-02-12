using UnityEngine;

public interface IWorkingPoints
{
    Transform GoldPosition { get; set; }

    Transform MeatPosition { get; set; }

    Transform WoodPosition { get; set; }

    void NewSpawnPosition(Transform newSpawnPosition);
}
