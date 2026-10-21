using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnPositionCalculator
{
    [SerializeField] private Vector3 playerSpawnBound;
    [SerializeField] private LayerMask targetLayer;

    private const float MinSpawnDistance = 10f;
    private const float MaxRayDistance = 1500f;
    private const int MaxReTryCount = 4;


    public bool TryGetPlayerPosition(out Vector3 playerPos)
    {
        playerPos = Vector3.zero;
        
        for (int i = 0; i < MaxReTryCount; i++)
        {
            float randX = Random.Range(-playerSpawnBound.x, playerSpawnBound.x);
            float randZ = Random.Range(-playerSpawnBound.z, playerSpawnBound.z);
            Vector3 randPos = new Vector3(randX, 1000f, randZ);

            if (TryRayCastPosition(randPos, out Vector3 hitPoint))
            {
                playerPos = hitPoint;
                return true;
            }
        }

        return false;
    }

    public bool TryGetMonsterPosition(Vector3 playerPos, float range, out Vector3 monsterPos)
    {
        monsterPos = Vector3.zero;
        
        for (int i = 0; i < MaxReTryCount; i++)
        {
            float playerAreaX = playerPos.x + MinSpawnDistance;
            float playerAreaZ = playerPos.z + MinSpawnDistance;
            float randX = Random.Range(playerAreaX, playerAreaX + range) * (Random.value < 0.5f ? -1 : 1);
            float randZ = Random.Range(playerAreaZ, playerAreaZ + range) * (Random.value < 0.5f ? -1 : 1);
            Vector3 randPos = new Vector3(randX, 1000f, randZ);

            if (TryRayCastPosition(randPos, out Vector3 hitPoint))
            {
                monsterPos = hitPoint;
                return true;
            }
        }
        
        return false;
    }

    private bool TryRayCastPosition(Vector3 targetPos, out Vector3 hitPoint)
    {
        hitPoint = Vector3.zero;

        if (Physics.Raycast(targetPos, Vector3.down, out RaycastHit hit, MaxRayDistance, targetLayer))
        {
            hitPoint = hit.point;
            return true;
        }

        return false;
    }
}