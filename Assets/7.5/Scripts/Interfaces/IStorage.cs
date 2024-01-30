using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage
{
    void UpdateGold(int newGoldCount);

    void UpdateMeat(int newMeatCount);

    void UpdateWood(int newWoodCount);

}
