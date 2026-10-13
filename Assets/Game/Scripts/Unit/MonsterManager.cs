using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float separationWeight = 1f;
    
    private PlayerController player;
    private List<MonsterController> monsters = new();
    
    private Vector3[] monstersPosBuffer;
    private Vector3[] separationBuffer;
    
    public void Init(PlayerController playerController)
    {
        player = playerController;
    }
    
    private void Update()
    {
        float deltaTime = Time.deltaTime;

        Vector3 targetPosition = player.transform.position;
        targetPosition.y = 0f;

        CachePositions();

        CalculateSeparation();

        TickMonsters(targetPosition, deltaTime);
    }

    private void CachePositions()
    {
        if (monstersPosBuffer == null || separationBuffer == null || monstersPosBuffer.Length < monsters.Count)
        {
            monstersPosBuffer = new Vector3[monsters.Count * 2];
            separationBuffer = new Vector3[monsters.Count * 2];
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            Vector3 monsterPos = monsters[i].transform.position;
            monsterPos.y = 0f;
            
            monstersPosBuffer[i] = monsterPos;
            separationBuffer[i] = Vector3.zero;
        }
    }

    private void CalculateSeparation()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            for (int j = i + 1; j < monsters.Count; j++)
            {
                Vector3 diff = monstersPosBuffer[i] - monstersPosBuffer[j];
                float sqrMagnitude = diff.sqrMagnitude;
                float targetDistance = monsters[i].SeparationRadius + monsters[j].SeparationRadius;
                float sqrTargetDistance = targetDistance * targetDistance;

                if (sqrMagnitude < sqrTargetDistance)
                {
                    float distance = Mathf.Sqrt(sqrMagnitude);
                    float overlapRatio = (targetDistance - distance) / targetDistance;
                    Vector3 direction = distance > 0.00001f ? diff / distance : Vector3.right;
                    
                    separationBuffer[i] += direction * overlapRatio;
                    separationBuffer[j] += -direction * overlapRatio;
                }
            }
        }
    }

    private void TickMonsters(Vector3 targetPosition, float deltaTime)
    {
        float playerRadius = player.Radius;
        
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            if (monsters[i].IsDead)
            {
                RemoveMonster(i);
                continue;
            }
            
            Vector3 diff = targetPosition - monstersPosBuffer[i];
            float distance = diff.magnitude;
            Vector3 direction = distance > 0.00001f ? diff / distance : Vector3.zero;
            Vector3 moveDir = direction + separationBuffer[i] * separationWeight;

            monsters[i].Tick(moveDir, distance, playerRadius, deltaTime);
        }
    }

    public void AddMonster(MonsterController monster)
    {
        monsters.Add(monster);
    }

    private void RemoveMonster(int removeIndex)
    {
        int lastIndex = monsters.Count - 1;

        SwapMonster(lastIndex, removeIndex);
        monsters.RemoveAt(lastIndex);
    }

    private void SwapMonster(int lastIndex, int i)
    {
        (monsters[lastIndex], monsters[i]) = (monsters[i], monsters[lastIndex]);
    }
}