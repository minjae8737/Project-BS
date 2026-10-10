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
        Vector3 targetPosition = player.transform.position;
        targetPosition.y = 0f;

        CachePositions();

        CalculateSeparation();
        
        MoveMonsters(targetPosition);
    }

    private void CachePositions()
    {
        //FIXME monsters는 활성화된 몬스터만있는 List로 되어있음. -> 나중에 활성화되지않은 몬스터들이 있을 수 있음. 
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

    private void MoveMonsters(Vector3 targetPosition)
    {
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            Vector3 direction = (targetPosition - monstersPosBuffer[i]).normalized;
            monsters[i].Tick(direction + separationBuffer[i] * separationWeight);
        }
    }

    public void AddMonster(MonsterController monster)
    {
        monsters.Add(monster);
    }
}