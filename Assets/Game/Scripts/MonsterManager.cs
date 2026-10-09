using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
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

        //FIXME monsters는 활성화된 몬스터만있는 List로 되어있음. -> 나중에 활성화되지않은 몬스터들이 있을 수 있음. 
        if (monstersPosBuffer == null || separationBuffer == null || monstersPosBuffer.Length < monsters.Count)
        {
            monstersPosBuffer = new Vector3[monsters.Count * 2];
            separationBuffer = new Vector3[monsters.Count * 2];
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            monstersPosBuffer[i] = monsters[i].transform.position;
            separationBuffer[i] = Vector3.zero;
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            for (int j = i + 1; j < monsters.Count; j++)
            {
                float sqrMagnitude = (monstersPosBuffer[i] - monstersPosBuffer[j]).sqrMagnitude;
                float targetDistance = monsters[i].SeparationRadius + monsters[j].SeparationRadius;
                float sqrTargetDistance = targetDistance * targetDistance;

                if (sqrMagnitude < sqrTargetDistance)
                {
                    float distance = Mathf.Sqrt(sqrMagnitude);
                    float overlapRatio = (targetDistance - distance) / targetDistance;
                    Vector3 nomalized = (monstersPosBuffer[i] - monstersPosBuffer[j]) / distance;
                    
                    separationBuffer[i] += nomalized * overlapRatio;
                    separationBuffer[j] += -nomalized * overlapRatio;
                }
            }
        }
        
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            Vector3 direction = targetPosition - monsters[i].transform.position;
            monsters[i].Tick(direction + separationBuffer[i]);
        }
    }

    public void AddMonster(MonsterController monster)
    {
        monsters.Add(monster);
    }
}