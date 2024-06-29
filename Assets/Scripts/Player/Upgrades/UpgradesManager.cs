using System.Collections;
using System.Collections.Generic;
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

    [Header("UI Area")]
    [field: SerializeField] private Button upgradeButton;
    [field: SerializeField] private Button closeButton;
    [field: SerializeField] private GameObject upgradePanel;

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
            DontDestroyOnLoad(gameObject);
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

        smallTruckInactiveButton.onClick.AddListener(() => ChangeContent(typeOfButton.SmallTruck));
        largeTruckInactiveButton.onClick.AddListener(() => ChangeContent(typeOfButton.LargeTruck));
        houseInactiveButton.onClick.AddListener(() => ChangeContent(typeOfButton.House));

        OnOpenUpgradeMenu();
    }
    
    private void onUpgradeButtonClicked()
    {
        upgradePanel.SetActive(true);
        OnOpenUpgradeMenu();
        upgradeButton.gameObject.SetActive(false);
    }

    private void onCloseButtonClicked()
    {
        upgradePanel.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
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
}
