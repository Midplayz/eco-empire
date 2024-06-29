using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    public int cashInHand = 0;
    public int smallTrucksOwned = 1;
    public int largeTrucksOwned = 0;
    public List<bool> housesUnlocked = new List<bool>();

    public int smallTruckLevel = 0;
    public int largeTrucksLevel = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
