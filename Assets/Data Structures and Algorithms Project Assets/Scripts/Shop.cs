using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private float _coins = 0;

    public Action<float> OnCoinsChanged;

    public void BuySeed(string name, float price)
    {
        if(_coins >= price)
        {
            _coins -= price;
            Planter._instance.AddSeeds(name, 1);
            OnCoinsChanged(_coins);
        }
        else
        {
            Debug.Log("Not rnough coins to buy seeds");
            UIManager._instance.UpdateStatus("Not enough coins");
        }
        
    }

    //Assignment 2
    // Get the harvest, add coins for the value, update UI and remove the item from the data structure
    public void SellHarvest(string name, float pricePerItem)
    {
        List <CollectedHarvest> collectedHarvestList = Harvester._instance.GetCollectedHarvest();
        CollectedHarvest collectedHarvestToRemove = new();
        int amountOfHarvestToSell = 0;

        foreach (CollectedHarvest harvest in collectedHarvestList)
        {
            if (harvest._name == name)
            {
                amountOfHarvestToSell += harvest._amount;
                collectedHarvestToRemove = harvest;
            }
        }

        if (collectedHarvestToRemove._name == name)
        {
            Harvester._instance.RemoveHarvest(collectedHarvestToRemove);
        }

        _coins += amountOfHarvestToSell * pricePerItem;
        OnCoinsChanged(_coins);
    }
}
