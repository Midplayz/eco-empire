using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    private enum buttonAction { SmallTruckQty, SmallTruckSpeed, LargeTruckQty, LargeTruckSpeed, House };
    [field: SerializeField] private buttonAction currentAction;
    [field: SerializeField] private int houseIndex = 0;
    [field: SerializeField] private TextMeshProUGUI priceText;
    [field: SerializeField] private TextMeshProUGUI nameText;

    private void Start()
    {
        gameObject.GetComponentInChildren<Button>().onClick.AddListener(() => OnButtonPressed());
        StatsManager.Instance.OnCurrencyAdjusted += UpdateButtonState;
    }
    private void OnEnable()
    {
        UpdateButtonState();
    }
    private void OnButtonPressed()
    {
        switch (currentAction)
        {
            case buttonAction.SmallTruckQty:
                if(StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfSmallTruckQty)
                {
                    UpgradesManager.Instance.smallTrucksOwned += 1;
                    StatsManager.Instance.AdjustCurrency(-UpgradesManager.Instance.currentCostOfSmallTruckQty);
                    SavingLoadingManager.Instance.SaveSmallTrucksOwned(UpgradesManager.Instance.smallTrucksOwned);
                    UpgradesManager.Instance.OnSmallTruckQtyPurchased();
                }
                break;

            case buttonAction.SmallTruckSpeed:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfSmallTruckSpeed)
                {
                    UpgradesManager.Instance.smallTruckLevel += 1;
                    StatsManager.Instance.AdjustCurrency(-UpgradesManager.Instance.currentCostOfSmallTruckSpeed);
                    SavingLoadingManager.Instance.SaveSmallTruckLevel(UpgradesManager.Instance.smallTruckLevel);
                    UpgradesManager.Instance.OnSmallTruckUpgraded();
                }
                break;

            case buttonAction.LargeTruckQty:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfLargeTruckQty)
                {
                    UpgradesManager.Instance.largeTrucksOwned += 1;
                    StatsManager.Instance.AdjustCurrency(-UpgradesManager.Instance.currentCostOfLargeTruckQty);
                    SavingLoadingManager.Instance.SaveLargeTrucksOwned(UpgradesManager.Instance.largeTrucksOwned);
                    UpgradesManager.Instance.OnLargeTruckQtyPurchased();
                }
                break;

            case buttonAction.LargeTruckSpeed:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfLargeTruckSpeed)
                {
                    UpgradesManager.Instance.largeTrucksLevel += 1;
                    StatsManager.Instance.AdjustCurrency(-UpgradesManager.Instance.currentCostOfLargeTruckSpeed);
                    SavingLoadingManager.Instance.SaveLargeTruckLevel(UpgradesManager.Instance.largeTrucksLevel);
                    UpgradesManager.Instance.OnLargeTruckUpgraded();
                }
                break;

            case buttonAction.House:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentHouseCosts[houseIndex - 1])
                {
                    UpgradesManager.Instance.housesUnlocked[houseIndex] = true;
                    HouseStateManager.Instance.HouseManagerList[houseIndex].OnUnlock();
                    StatsManager.Instance.AdjustCurrency(-UpgradesManager.Instance.currentHouseCosts[houseIndex - 1]);
                    SavingLoadingManager.Instance.SaveHousesUnlocked(UpgradesManager.Instance.housesUnlocked);
                }
                break;

            default:
                Debug.LogError("Unknown button type: " + currentAction);
                break;
        }
    }

    private void UpdateButtonState()
    {
        switch (currentAction)
        {
            case buttonAction.SmallTruckQty:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfSmallTruckQty && UpgradesManager.Instance.smallTrucksOwned <5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = true;
                    nameText.text = "Qty x" + (UpgradesManager.Instance.smallTrucksOwned).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfSmallTruckQty;
                }
                else if(UpgradesManager.Instance.smallTrucksOwned >= 5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Qty x" + (UpgradesManager.Instance.smallTrucksOwned).ToString();
                    priceText.text = "Max!";
                }
                else
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Qty x" + (UpgradesManager.Instance.smallTrucksOwned).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfSmallTruckQty;
                }
                break;

            case buttonAction.SmallTruckSpeed:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfSmallTruckSpeed && UpgradesManager.Instance.smallTruckLevel < 5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = true;
                    nameText.text = "Load Speed Lvl x" + (UpgradesManager.Instance.smallTruckLevel).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfSmallTruckSpeed;
                }
                else if (UpgradesManager.Instance.smallTruckLevel >= 5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Load Speed Lvl x" + (UpgradesManager.Instance.smallTruckLevel).ToString();
                    priceText.text = "Max!";
                }
                else
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Load Speed Lvl x" + (UpgradesManager.Instance.smallTruckLevel).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfSmallTruckSpeed;
                }
                break;

            case buttonAction.LargeTruckQty:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfLargeTruckQty && UpgradesManager.Instance.largeTrucksOwned < 5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = true;
                    nameText.text = "Qty x" + (UpgradesManager.Instance.largeTrucksOwned).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfLargeTruckQty;
                }
                else if(UpgradesManager.Instance.largeTrucksOwned >= 5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Qty x" + (UpgradesManager.Instance.largeTrucksOwned).ToString();
                    priceText.text = "Max!";
                }
                else
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Qty x" + (UpgradesManager.Instance.largeTrucksOwned + 1).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfLargeTruckQty;
                }
                break;

            case buttonAction.LargeTruckSpeed:
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentCostOfLargeTruckSpeed && UpgradesManager.Instance.largeTrucksLevel < 5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = true;
                    nameText.text = "Load Speed Lvl x" + (UpgradesManager.Instance.largeTrucksLevel).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfLargeTruckSpeed;
                }
                else if(UpgradesManager.Instance.largeTrucksLevel >= 5)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Load Speed Lvl x" + (UpgradesManager.Instance.largeTrucksLevel).ToString();
                    priceText.text = "Max!";
                }
                else 
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "Load Speed Lvl x" + (UpgradesManager.Instance.largeTrucksLevel).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentCostOfLargeTruckSpeed;
                }
                break;

            case buttonAction.House:
                print(houseIndex);
                if (StatsManager.Instance.cashInHand >= UpgradesManager.Instance.currentHouseCosts[houseIndex-1] && !HouseStateManager.Instance.HouseManagerList[houseIndex].isUnlocked)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = true;
                    nameText.text = "House " + (houseIndex + 1).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentHouseCosts[houseIndex - 1];
                }
                else if(HouseStateManager.Instance.HouseManagerList[houseIndex].isUnlocked)
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "House " + (houseIndex + 1).ToString();
                    priceText.text = "Unlocked!";
                }
                else
                {
                    gameObject.GetComponentInChildren<Button>().interactable = false;
                    nameText.text = "House " + (houseIndex + 1).ToString();
                    priceText.text = "$" + UpgradesManager.Instance.currentHouseCosts[houseIndex-1];
                }
                break;

            default:
                Debug.LogError("Unknown button type: " + currentAction);
                break;
        }
    }
}
