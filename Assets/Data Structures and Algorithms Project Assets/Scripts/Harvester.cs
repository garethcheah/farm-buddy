using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [SerializeField] public Harvest _harvest;
    [SerializeField] public Seed _seed;

    // Harvest Analytics
    private Dictionary<string, int> _harvests = new Dictionary<string, int>();

    // Harvest to sell
    // Assignment 2 - Data structure to hold collected harvests
    private List<CollectedHarvest> collectedHarvestList = new List<CollectedHarvest>();

    public static Harvester _instance;
       
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    // Assignment 2
    public List<CollectedHarvest> GetCollectedHarvest()
    {
        return collectedHarvestList;
    }

    // Assignment 2
    public void RemoveHarvest(CollectedHarvest harvest)
    {
        collectedHarvestList.Remove(harvest);
    }

    // Assignment 2 - CollectHarvest method to collect the harvest when picked up
    public void CollectHarvest(string plantName, string harvestedTime, int harvestedAmount)
    {
        int totalHarvestedAmount = harvestedAmount;
        CollectedHarvest collectedHarvestToRemove = new();

        // Determine if harvest is already part of the CollectedHarvestList
        foreach (CollectedHarvest harvest in collectedHarvestList)
        {
            if (harvest._name == plantName)
            {
                // Harvest is already in collected list. Add previously harvested amount to current amount.
                totalHarvestedAmount += harvest._amount;

                // Mark harvest for removal
                collectedHarvestToRemove = harvest;
                break;
            }
        }

        // Remove harvest from collected list
        if (collectedHarvestToRemove._name == plantName)
        {
            collectedHarvestList.Remove(collectedHarvestToRemove);
        }

        CollectedHarvest collectedHarvest = new()
        {
            _name = plantName,
            _time = harvestedTime,
            _amount = totalHarvestedAmount
        };

        collectedHarvestList.Add(collectedHarvest);

        UIManager._instance.UpdateStatus($"At {harvestedTime}, {totalHarvestedAmount} {plantName} were harvested.");
    }
    

    public void ShowHarvest(string plantName, int harvestAmount, int seedAmount, Vector2 position)
    {
        // initiate a harvest with random amount
        Harvest harvest = Instantiate(_harvest, position + Vector2.up + Vector2.right, Quaternion.identity);
        harvest.SetHarvest(plantName, harvestAmount);
        
        // initiate one seed object
        Seed seed = Instantiate(_seed, position + Vector2.up + Vector2.left, Quaternion.identity);
        seed.SetSeed(plantName, seedAmount);
    }

    //Assignment 3
    public void SortHarvestByAmount()
    {
        // Sort the collected harvest using Quick sort
    }

}

// For Assignment 2, this holds a collected harvest object
[System.Serializable]
public struct CollectedHarvest
{
    public string _name;
    public string _time;
    public int _amount;
    
    public CollectedHarvest(string name, string time, int amount)
    {
        _name = name;
        _time = time;
        _amount = amount;
    }
}