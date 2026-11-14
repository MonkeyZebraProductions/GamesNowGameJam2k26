using System;
using System.Collections;
using UnityEngine;

public class PartSpawner : MonoBehaviour
{

    public GameObject[] PartsToSpawn;
    public float Intervil = 3f;
    private int index;
    public Transform[] SpawnPoints;
    private Transform activeSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CallAfterDelay.Create(Intervil, () => { SpawnPart(); });
        foreach (Transform point in SpawnPoints)
        {
            if(point.gameObject.activeSelf)
            {
                activeSpawnPoint = point;
                break;
            }
        }
    }

    private void SpawnPart()
    {
        Instantiate(PartsToSpawn[index],activeSpawnPoint.position,Quaternion.identity);
        index++;
        if(index == PartsToSpawn.Length)
        {
            index = 0;
        }
        CallAfterDelay.Create(Intervil, () => { SpawnPart(); });
    }

    
}
