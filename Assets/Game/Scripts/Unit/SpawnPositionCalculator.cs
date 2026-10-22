using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnPositionCalculator
{
    [SerializeField] private Vector3 playerSpawnBound;
    [SerializeField] private LayerMask surfaceLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float minSpawnDistance = 10f;
    [SerializeField] private float maxRayDistance = 1500f;
    [SerializeField] private float rayHeight = 1000f;
    [SerializeField] private float checkRadius = 2f;
    [SerializeField] private int maxAttempts = 10;
    
    public bool TryGetPlayerPosition(out Vector3 playerPos)
    {
        playerPos = Vector3.zero;
        
        for (int i = 0; i < maxAttempts; i++)
        {
            float randX = Random.Range(-playerSpawnBound.x, playerSpawnBound.x);
            float randZ = Random.Range(-playerSpawnBound.z, playerSpawnBound.z);
            Vector3 randPos = new Vector3(randX, rayHeight, randZ);

            if (TryRayCastPosition(randPos, out Vector3 hitPoint))
            {
                playerPos = hitPoint;
                return true;
            }
        }

        return false;
    }

    public bool TryGetMonsterPosition(Vector3 centerPos, float range, out Vector3 monsterPos)
    {
        monsterPos = Vector3.zero;
        
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randDir = Random.insideUnitCircle.normalized;
            float randDistance = Random.Range(minSpawnDistance, minSpawnDistance + range);

            Vector3 randPos = new Vector3(
                centerPos.x + randDir.x * randDistance, 
                rayHeight,
                centerPos.z + randDir.y * randDistance);

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

        if (Physics.Raycast(targetPos, Vector3.down, out RaycastHit hit, maxRayDistance, surfaceLayer))
        {
            if ((obstacleLayer.value & (1 << hit.collider.gameObject.layer)) != 0) 
                return false;
            
            if (Physics.CheckSphere(hit.point, checkRadius,obstacleLayer))
                return false;

            hitPoint = hit.point;
            return true;
        }

        return false;
    }
}