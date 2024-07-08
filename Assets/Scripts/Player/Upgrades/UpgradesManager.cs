using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public enum typeOfButton { SmallTruck, LargeTruck, House };
    public static UpgradesManager Instance;

    public int smallTrucksOwned = 1;
    public int largeTrucksOwned = 0;
    public List<bool> housesUnlocked = new List<bool>();

    public int smallTruckLevel = 0;
    public int largeTrucksLevel = 0;

    public int currentCostOfSmallTruckQty;
    public int currentCostOfSmallTruckSpeed;

    public int currentCostOfLargeTruckQty;
    public int currentCostOfLargeTruckSpeed;

    public List<int> currentHouseCosts;

    [Header("UI Area")]
    [field: SerializeField] private Button upgradeButton;
    [field: SerializeField] private Button closeButton;
    [field: SerializeField] private GameObject upgradePanel;
    [field: SerializeField] private GameObject resetButton;

    [field: SerializeField] private Button smallTruckActiveButton;
    [field: SerializeField] private Button smallTruckInactiveButton;
    [field: SerializeField] private GameObject smallTruckContent;

    [field: SerializeField] private Button largeTruckActiveButton;
    [field: SerializeField] private Button largeTruckInactiveButton;
    [field: SerializeField] private GameObject largeTruckContent;

    [field: SerializeField] private Button houseActiveButton;
    [field: SerializeField] private Button houseInactiveButton;
    [field: SerializeField] private GameObject houseContent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        upgradeButton.gameObject.SetActive(true);
        upgradePanel.SetActive(false);

        upgradeButton.onClick.AddListener(() => onUpgradeButtonClicked());
        closeButton.onClick.AddListener(() => onCloseButtonClicked());

        smallTruckActiveButton.interactable = false;
        largeTruckActiveButton.interactable = false;
        houseActiveButton.interactable = false;
        LoadSavedUpgradeValues();
        setInitialPrices();

        smallTruckInactiveButton.onClick.AddListener(() => ChangeContent(typeOfButton.SmallTruck));
        largeTruckInactiveButton.onClick.AddListener(() => ChangeContent(typeOfButton.LargeTruck));
        houseInactiveButton.onClick.AddListener(() => ChangeContent(typeOfButton.House));

        OnOpenUpgradeMenu();
    }
    
    private void setInitialPrices()
    {
        if (smallTruckLevel > 0)
        {
            for (int i = 0; i < smallTruckLevel; i++)
            {
                currentCostOfSmallTruckSpeed = Mathf.RoundToInt(currentCostOfSmallTruckSpeed * 1.5f);
                currentCostOfSmallTruckSpeed = RoundToNearestMultiple(currentCostOfSmallTruckSpeed);
            }
        }
        if (smallTrucksOwned > 1)
        {
            for (int i = 0; i < smallTrucksOwned - 1; i++)
            {
                currentCostOfSmallTruckQty = Mathf.RoundToInt(currentCostOfSmallTruckQty * 1.5f);
                currentCostOfSmallTruckQty = RoundToNearestMultiple(currentCostOfSmallTruckQty);
            }
        }

        if (largeTrucksLevel > 0)
        {
            for (int i = 0; i < largeTrucksLevel; i++)
            {
                currentCostOfLargeTruckSpeed = Mathf.RoundToInt(currentCostOfLargeTruckSpeed * 1.5f);
                currentCostOfLargeTruckSpeed = RoundToNearestMultiple(currentCostOfLargeTruckSpeed);
            }
        }
        if (largeTrucksOwned > 1)
        {
            for (int i = 0; i < largeTrucksOwned - 1; i++)
            {
                currentCostOfLargeTruckQty = Mathf.RoundToInt(currentCostOfLargeTruckQty * 1.5f);
                currentCostOfLargeTruckQty = RoundToNearestMultiple(currentCostOfLargeTruckQty);
            }
        }
    }

    private void onUpgradeButtonClicked()
    {
        upgradePanel.SetActive(true);
        OnOpenUpgradeMenu();
        upgradeButton.gameObject.SetActive(false);
        resetButton.SetActive(false);
    }

    private void onCloseButtonClicked()
    {
        upgradePanel.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        resetButton.SetActive(true);
    }

    public void OnOpenUpgradeMenu()
    {
        ChangeContent(typeOfButton.SmallTruck);
    }

    public void LoadSavedUpgradeValues()
    {
        int cashInHand = 0;
        SavingLoadingManager.Instance.LoadAll(out cashInHand, out smallTrucksOwned, out largeTrucksOwned, out housesUnlocked, out smallTruckLevel, out largeTrucksLevel);
    }
    public void SaveUpgradeValues()
    {
        SavingLoadingManager.Instance.SaveSmallTrucksOwned(smallTrucksOwned);
        SavingLoadingManager.Instance.SaveLargeTrucksOwned(largeTrucksOwned);
        SavingLoadingManager.Instance.SaveHousesUnlocked(housesUnlocked);
        SavingLoadingManager.Instance.SaveSmallTruckLevel(smallTruckLevel);
        SavingLoadingManager.Instance.SaveLargeTruckLevel(largeTrucksLevel);
    }
    public void ChangeContent(typeOfButton typeOfButton)
    {
        switch (typeOfButton)
        {
            case typeOfButton.SmallTruck:
                smallTruckInactiveButton.gameObject.SetActive(false);
                smallTruckActiveButton.gameObject.SetActive(true);

                largeTruckActiveButton.gameObject.SetActive(false);
                largeTruckInactiveButton.gameObject.SetActive(true);

                houseActiveButton.gameObject.SetActive(false);
                houseInactiveButton.gameObject.SetActive(true);

                smallTruckContent.SetActive(true);
                largeTruckContent.SetActive(false);
                houseContent.SetActive(false);
                Debug.Log("Small Truck Button Pressed!");
                break;
            case typeOfButton.LargeTruck:
                smallTruckActiveButton.gameObject.SetActive(false);
                smallTruckInactiveButton.gameObject.SetActive(true);

                largeTruckActiveButton.gameObject.SetActive(true);
                largeTruckInactiveButton.gameObject.SetActive(false);

                houseActiveButton.gameObject.SetActive(false);
                houseInactiveButton.gameObject.SetActive(true);

                smallTruckContent.SetActive(false);
                largeTruckContent.SetActive(true);
                houseContent.SetActive(false);
                Debug.Log("Large Truck Button Pressed!");
                break;
            case typeOfButton.House:
                smallTruckActiveButton.gameObject.SetActive(false);
                smallTruckInactiveButton.gameObject.SetActive(true);

                largeTruckActiveButton.gameObject.SetActive(false);
                largeTruckInactiveButton.gameObject.SetActive(true);

                houseActiveButton.gameObject.SetActive(true);
                houseInactiveButton.gameObject.SetActive(false);

                smallTruckContent.SetActive(false);
                largeTruckContent.SetActive(false);
                houseContent.SetActive(true);
                Debug.Log("House Button Pressed!");
                break;
            default:
                Debug.LogError("Unknown button type: " + typeOfButton);
                break;
        }
    }

    private void OnApplicationQuit()
    {
        SavingLoadingManager.Instance.SaveHousesUnlocked(housesUnlocked);
        SavingLoadingManager.Instance.SaveLargeTruckLevel(largeTrucksLevel);
        SavingLoadingManager.Instance.SaveLargeTrucksOwned(largeTrucksOwned);
        SavingLoadingManager.Instance.SaveSmallTruckLevel(smallTruckLevel);
        SavingLoadingManager.Instance.SaveSmallTrucksOwned(smallTrucksOwned);
    }

    public static int RoundToNearestMultiple(int value)
    {
        if (value <= 100)
        {
            return Mathf.CeilToInt(value / 50f) * 50;
        }
        else if (value <= 1000)
        {
            return Mathf.CeilToInt(value / 100f) * 100;
        }
        else
        {
            return Mathf.CeilToInt(value / 1000f) * 1000;
        }
    }

    #region Manage Upgrades
    public void OnSmallTruckUpgraded()
    {
        List<TruckMovementScript> smallTrucks = TrucksManager.Instance.totalTrucks.Where(truck => truck.truckCapacity == 1).ToList();
        foreach(TruckMovementScript truck in smallTrucks)
        {
            truck.stopDuration -= 0.2f;
        }
        currentCostOfSmallTruckSpeed = Mathf.RoundToInt(currentCostOfSmallTruckSpeed * 1.5f);
        currentCostOfSmallTruckSpeed = RoundToNearestMultiple(currentCostOfSmallTruckSpeed);
        SaveUpgradeValues();
    }
    
    public void OnSmallTruckQtyPurchased()
    {
        TrucksManager.Instance.OnSmallTruckPurchase();
        currentCostOfSmallTruckQty = Mathf.RoundToInt(currentCostOfSmallTruckQty * 1.5f);
        currentCostOfSmallTruckQty = RoundToNearestMultiple(currentCostOfSmallTruckQty);
        SaveUpgradeValues();
    }

    public void OnLargeTruckUpgraded()
    {
        List<TruckMovementScript> largeTrucks = TrucksManager.Instance.totalTrucks.Where(truck => truck.truckCapacity == 2).ToList();
        foreach (TruckMovementScript truck in largeTrucks)
        {
            truck.stopDuration -= 0.2f;
        }
        currentCostOfLargeTruckSpeed = Mathf.RoundToInt(currentCostOfLargeTruckSpeed * 1.5f);
        currentCostOfLargeTruckSpeed = RoundToNearestMultiple(currentCostOfLargeTruckSpeed);
        SaveUpgradeValues();
    }

    public void OnLargeTruckQtyPurchased()
    {
        TrucksManager.Instance.OnLargeTruckPurchase();
        currentCostOfLargeTruckQty = Mathf.RoundToInt(currentCostOfLargeTruckQty * 1.5f);
        currentCostOfLargeTruckQty = RoundToNearestMultiple(currentCostOfLargeTruckQty);
        SaveUpgradeValues();
    }
    #endregion
}
