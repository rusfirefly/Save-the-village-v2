using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{
    public static int WorkersCount { get; private set; }
    public static int WorkersCountTotal { get; private set; }
    public static int WarriorsCount { get; private set; }
    public static int WarriorsCountTotal { get; private set; }
    public static int ArcherCount { get; private set; }
    public static int ArcherCountTotal { get; private set; }
    public static int WarriorsCountDeath { get; private set; }

    public static int EngineerCount { get; private set; }
    public static int EngineerCountTotal { get; private set; }

    public static int CountGoldWorker { get; private set; }
    public static int CountMeatWorker { get; private set; }
    public static int CountWoodWorker { get; private set; }

    public Population() => ReloadValue();

    public void AddOneWorker() => WorkersCount++;

    public void AddOneGoldWorker() => CountGoldWorker++;

    public void AddOneMeatWorker() => CountMeatWorker++;

    public void AddOneWoodWorker() => CountWoodWorker++;

    public void AddOneWarrior()
    {
        WarriorsCount++;
    }
    public void AddOneArcher()
    {
        ArcherCount++;
    }
    public void DeathWarrior()
    {
        WarriorsCount--;
        WarriorsCountDeath++;
    }

    public void DeathArcher()
    {
        ArcherCount--;
        WarriorsCountDeath++;
    }

    public void ReloadValue()
    {
        WorkersCount = 0;
        WarriorsCount = 0;
        CountGoldWorker = 0;
        CountMeatWorker = 0;
        CountWoodWorker = 0;
        ArcherCount = 0;

        WorkersCountTotal = 10;
        WarriorsCountTotal = 10;
        ArcherCountTotal = 1;
        EngineerCountTotal = 1;
    }

    public void UpPopulation()
    {
        WarriorsCountTotal += 5;
        WorkersCountTotal += 5;
        ArcherCountTotal += 1;
    }
}
