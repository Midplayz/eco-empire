using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TruckMovementScript : MonoBehaviour
{
    public PathDefiner pathDefiner;
    public Transform[] targetHouses;
    public GameObject trashIntruck;
    public int truckCapacity = 1;
    public float speed = 5f;
    public float rotationSpeed = 2f;
    public float stopDuration = 2f;

    public bool outForDuty = false;

    private int currentHouseIndex = 0;
    private bool isReturning = false;
    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private Vector3 startPosition;

    void Start()
    {
        //if(targetHouses.Count() > truckCapacity)
        //{
        //    Debug.LogError(transform.name + " Has more Targets than Capacity!");
        //    return;
        //}
        //if (pathDefiner == null || pathDefiner.waypoints.Length == 0)
        //{
        //    Debug.LogError("PathDefiner is not assigned or has no waypoints.");
        //    return;
        //}

        //startPosition = pathDefiner.waypoints[0].position;
        //transform.position = new Vector3(startPosition.x, transform.position.y, startPosition.z);

        //StartCoroutine(MoveAlongPath());
    }

    public void StartTrip()
    {
        if (targetHouses.Count() > truckCapacity)
        {
            Debug.LogError(transform.name + " Has more Targets than Capacity!");
            return;
        }
        if (pathDefiner == null || pathDefiner.waypoints.Length == 0)
        {
            Debug.LogError("PathDefiner is not assigned or has no waypoints.");
            return;
        }
        startPosition = pathDefiner.waypoints[0].position;
        transform.position = new Vector3(startPosition.x, transform.position.y, startPosition.z);
        trashIntruck.SetActive(false);
        StartCoroutine(MoveAlongPath());
    }

    private IEnumerator MoveAlongPath()
    {
        while (true)
        {
            if (currentWaypointIndex < pathDefiner.waypoints.Length)
            {
                targetWaypoint = pathDefiner.waypoints[currentWaypointIndex];
                yield return StartCoroutine(MoveToWaypoint(targetWaypoint));

                // Check if we are supposed to stop at a house
                if (!isReturning && currentHouseIndex < targetHouses.Length)
                {
                    Transform targetHouse = targetHouses[currentHouseIndex];
                    int closestMainRoadWaypointIndex = FindClosestMainRoadWaypointIndex(targetHouse.position);

                    // Move through all intermediate waypoints to the closest one before going to the house
                    while (currentWaypointIndex < closestMainRoadWaypointIndex)
                    {
                        targetWaypoint = pathDefiner.waypoints[currentWaypointIndex];
                        yield return StartCoroutine(MoveToWaypoint(targetWaypoint));
                        currentWaypointIndex++;
                    }

                    // Move to the target house
                    yield return StartCoroutine(MoveToTargetHouse(targetHouse));
                    currentHouseIndex++;

                    // After visiting the house, find the next waypoint to follow
                    currentWaypointIndex = FindNextWaypointIndex(targetHouse.position);
                }
                else if (isReturning && currentWaypointIndex >= pathDefiner.waypoints.Length - 1)
                {
                    // Finished returning to the start
                    outForDuty = false;

                    // Reset values for the next run
                    currentHouseIndex = 0;
                    currentWaypointIndex = 0;
                    isReturning = false;
                    trashIntruck.SetActive(false);
                    TrucksManager.Instance.OnTripComplete(this);
                    yield break;
                }

                // Move to the next waypoint
                currentWaypointIndex++;
            }
            else
            {
                // If we finished the main path, start returning to the start
                isReturning = true;
                currentWaypointIndex = pathDefiner.waypoints.Length - 1;
                currentHouseIndex = 0; 
            }
        }
    }

    private IEnumerator MoveToWaypoint(Transform waypoint)
    {
        Vector3 targetPosition = new Vector3(waypoint.position.x, transform.position.y, waypoint.position.z);
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator MoveToTargetHouse(Transform house)
    {
        // Move to the house
        Vector3 targetPosition = new Vector3(house.position.x, transform.position.y, house.position.z);
        yield return StartCoroutine(MoveToWaypoint(house));

        if (transform.forward.z > 0.0f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        //transform.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(stopDuration);

        house.GetComponent<WayPointHouseHolder>().targetHouse.OnTrashPickup();
        trashIntruck.SetActive(true);
        int closestMainRoadWaypointIndex = FindClosestMainRoadWaypointIndex(transform.position);
        Transform closestMainRoadWaypoint = pathDefiner.waypoints[closestMainRoadWaypointIndex+1];
        yield return StartCoroutine(MoveToWaypoint(closestMainRoadWaypoint));
    }

    private int FindClosestMainRoadWaypointIndex(Vector3 targetPosition)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < pathDefiner.waypoints.Length; i++)
        {
            float distance = Vector3.Distance(pathDefiner.waypoints[i].position, targetPosition);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private int FindNextWaypointIndex(Vector3 housePosition)
    {
        int closestIndex = FindClosestMainRoadWaypointIndex(housePosition);
        for (int i = 0; i < pathDefiner.waypoints.Length; i++)
        {
            if (i == closestIndex)
            {
                return i;
            }
        }
        return 0;
    }
}
