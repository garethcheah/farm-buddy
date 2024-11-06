using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _seedButtonUI;
    [SerializeField] private Transform _seedsUIHolder;
    [SerializeField] private TMP_Text _txtStatus;

    [Header("Shop-BUY")]
    [SerializeField] private Transform _buySeedsHolder;
    [SerializeField] private SeedsBuyUIElement _buySeedsUIElement;

    [Header("Shop-SELL")]
    [SerializeField] private Transform _sellHarvestHolder;
    [SerializeField] private SellHarvestUIElement _sellHarvestUIElement;

    public static UIManager _instance { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
    }

    public void UpdateStatus(string text)
    {
        _txtStatus.SetText(text);
    }

    public void InitializePlantUIs(PlantTypeScriptableObject[] _plantTypes)
    {
        foreach (var item in _plantTypes)
        {
            GameObject seedButton = Instantiate(_seedButtonUI, _seedsUIHolder);

            seedButton.GetComponent<Image>().sprite = item._seedSprite;

            seedButton.GetComponent<Button>().onClick.AddListener(() => { 
                Planter._instance.ChoosePlant(item._plantTypeName); 
            });

            seedButton.GetComponent<UpdateSeedsUI>().SetSeedName(item._plantTypeName);



            SeedsBuyUIElement buySeedUIElement = Instantiate(_buySeedsUIElement, _buySeedsHolder);
            buySeedUIElement.SetElement(item._plantTypeName, item._pricePerSeed, item._seedSprite);
            buySeedUIElement.GetButton().onClick.AddListener(() =>
            {
                GameManager._instance.GetShop().BuySeed(item._plantTypeName, item._pricePerSeed);
            });

        }
    }

    public void ShowTotalHarvest()
    {
        //Assignment 2

        // Clear previously populated harvest items for sale
        foreach (Transform child in _sellHarvestHolder)
        {
            Destroy(child.gameObject);
        }

        List<CollectedHarvest> collectedHarvestList = Harvester._instance.GetCollectedHarvest();

        // Populate harvest items for sale
        foreach (CollectedHarvest harvest in collectedHarvestList)
        {
            PlantTypeScriptableObject plantType = Planter._instance.GetPlantResourseByName(harvest._name);

            if (plantType != null)
            {
                SellHarvestUIElement sellHarvestUIElement = Instantiate(_sellHarvestUIElement, _sellHarvestHolder);
                sellHarvestUIElement.SetElement(harvest, plantType._plantTypeName, harvest._time, plantType._pricePerHarvest, harvest._amount, plantType._harvestSprite);
                sellHarvestUIElement.GetButton().onClick.AddListener(() =>
                {
                    GameManager._instance.GetShop().SellHarvest(plantType._plantTypeName, plantType._pricePerHarvest);
                });
            }
        }
    }    
}
