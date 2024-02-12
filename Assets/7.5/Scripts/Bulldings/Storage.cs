using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public static int Gold { get; private set; }
    public static int Meat { get; private set; }
    public static int Wood { get; private set; }

    private int _defGoldCount;
    private int _defMeatCount;
    private int _defWoodCount;

    public Storage(int gold, int meat, int wood)
    {
        Gold = gold;
        Meat = meat;
        Wood = wood;

        _defGoldCount = gold;
        _defMeatCount = meat;
        _defWoodCount = wood;
    }

    public void AddGold(int newGoldCount1) => Gold += newGoldCount1;

    public void AddMeat(int newMeatCount) => Meat += newMeatCount;

    public void AddWood(int newWoodCount) => Wood += newWoodCount;

    public void SpendGold(int price) => Gold -= price;

    public void UseWood(int price) => Wood -= price;

    public void UseMeat(int eatUp)
    {
        Meat -= eatUp;
        if (Meat < 0)
            Meat = 0;
    }

    public void Reload()
    {
        Gold = _defGoldCount;
        Meat = _defMeatCount;
        Wood = _defWoodCount;
    }
}