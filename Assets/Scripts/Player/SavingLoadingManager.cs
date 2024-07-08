using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavingLoadingManager : MonoBehaviour
{
    public static SavingLoadingManager Instance;
    [Header("Resetting Stuff")]
    [SerializeField] private GameObject resetConfirmationPanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private GameObject upgradeButton;

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
        CheckIfNoSaveData();
    }
    private void Start()
    {
        resetConfirmationPanel.SetActive(false);
        resetButton.onClick.AddListener(() => OnClickResetButton());
        confirmButton.onClick.AddListener(() => DeleteAllData());
        cancelButton.onClick.AddListener(() => OnCancel()); 
    }

    public void SaveHousesUnlocked(List<bool> housesUnlocked)
    {
        for (int i = 0; i < housesUnlocked.Count; i++)
        {
            PlayerPrefs.SetInt("HouseUnlocked_" + i, housesUnlocked[i] ? 1 : 0);
        }
        PlayerPrefs.SetInt("HouseCount", housesUnlocked.Count);
    }

    public List<bool> LoadHousesUnlocked()
    {
        List<bool> housesUnlocked = new List<bool>();
        int houseCount = PlayerPrefs.GetInt("HouseCount", 0);
        for (int i = 0; i < houseCount; i++)
        {
            bool isUnlocked = PlayerPrefs.GetInt("HouseUnlocked_" + i, 0) == 1;
            housesUnlocked.Add(isUnlocked);
        }
        return housesUnlocked;
    }

    public void SaveCashInHand(int cashInHand)
    {
        PlayerPrefs.SetInt("CashInHand", cashInHand);
    }

    public int LoadCashInHand()
    {
        return PlayerPrefs.GetInt("CashInHand", 0);
    }

    public void SaveSmallTrucksOwned(int smallTrucksOwned)
    {
        PlayerPrefs.SetInt("SmallTrucksOwned", smallTrucksOwned);
    }

    public int LoadSmallTrucksOwned()
    {
        return PlayerPrefs.GetInt("SmallTrucksOwned", 1); // Default value is 1
    }

    public void SaveLargeTrucksOwned(int largeTrucksOwned)
    {
        PlayerPrefs.SetInt("LargeTrucksOwned", largeTrucksOwned);
    }

    public int LoadLargeTrucksOwned()
    {
        return PlayerPrefs.GetInt("LargeTrucksOwned", 0);
    }

    public void SaveSmallTruckLevel(int smallTruckLevel)
    {
        PlayerPrefs.SetInt("SmallTruckLevel", smallTruckLevel);
    }

    public int LoadSmallTruckLevel()
    {
        return PlayerPrefs.GetInt("SmallTruckLevel", 0);
    }

    public void SaveLargeTruckLevel(int largeTruckLevel)
    {
        PlayerPrefs.SetInt("LargeTruckLevel", largeTruckLevel);
    }

    public int LoadLargeTruckLevel()
    {
        return PlayerPrefs.GetInt("LargeTruckLevel", 0);
    }

    public void SaveAll(int cashInHand, int smallTrucksOwned, int largeTrucksOwned, List<bool> housesUnlocked, int smallTruckLevel, int largeTruckLevel)
    {
        SaveCashInHand(cashInHand);
        SaveSmallTrucksOwned(smallTrucksOwned);
        SaveLargeTrucksOwned(largeTrucksOwned);
        SaveHousesUnlocked(housesUnlocked);
        SaveSmallTruckLevel(smallTruckLevel);
        SaveLargeTruckLevel(largeTruckLevel);
        PlayerPrefs.Save();
    }

    public void LoadAll(out int cashInHand, out int smallTrucksOwned, out int largeTrucksOwned, out List<bool> housesUnlocked, out int smallTruckLevel, out int largeTruckLevel)
    {
        cashInHand = LoadCashInHand();
        smallTrucksOwned = LoadSmallTrucksOwned();
        largeTrucksOwned = LoadLargeTrucksOwned();
        housesUnlocked = LoadHousesUnlocked();
        smallTruckLevel = LoadSmallTruckLevel();
        largeTruckLevel = LoadLargeTruckLevel();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CheckIfNoSaveData()
    {
        if (!PlayerPrefs.HasKey("CashInHand"))
        {
            int defaultCashInHand = 0;
            int defaultSmallTrucksOwned = 1;
            int defaultLargeTrucksOwned = 0;
            List<bool> defaultHousesUnlocked = new List<bool>(); 
            int defaultSmallTruckLevel = 0;
            int defaultLargeTruckLevel = 0;

            for (int i = 0; i < 6; i++) 
            {
                if(i == 0)
                {
                    defaultHousesUnlocked.Add(true);
                }
                else
                {
                    defaultHousesUnlocked.Add(false);
                }
            }

            SaveAll(defaultCashInHand, defaultSmallTrucksOwned, defaultLargeTrucksOwned, defaultHousesUnlocked, defaultSmallTruckLevel, defaultLargeTruckLevel);

            Debug.Log("No save data found. Initialized with default values.");
        }
        else
        {
            Debug.Log("Save data exists.");
        }
    }

    #region Reset Stuff
    private void OnClickResetButton()
    {
        resetConfirmationPanel.SetActive(true);
        upgradeButton.SetActive(false);
        resetButton.gameObject.SetActive(false);
    }
    private void OnCancel()
    {
        resetConfirmationPanel.SetActive(false);
        upgradeButton.SetActive(true);
        resetButton.gameObject.SetActive(true);
    }
    #endregion
}
