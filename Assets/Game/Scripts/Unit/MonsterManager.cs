using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float separationWeight = 1f;
    [SerializeField] private List<MonsterController> prefabs;

    private StatRulesData statRulesData;
    private PlayerController player;
    private Dictionary<MonsterType, MonsterController> prefabDic;
    private List<MonsterController> monsters;

    private Vector3[] monstersPosBuffer;
    private Vector3[] separationBuffer;
    private float relocateRange;

    public int MonsterCount => monsters.Count;

    public event Action<MonsterController> MonsterOutOfRange; 

    private void Awake()
    {
        prefabDic = new();
        monsters = new();
        
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].Type == MonsterType.None)
                continue;

            if (!prefabDic.TryAdd(prefabs[i].Type, prefabs[i]))
            {
                Debug.LogError($"[MonsterManager] {prefabs[i].gameObject.name} 의 MonsterType이 중복됩니다.");
            }
        }
    }

    public void Init(StatRulesData statRulesData, PlayerController playerController, float relocateRange)
    {
        this.statRulesData = statRulesData;
        player = playerController;
        this.relocateRange = relocateRange;
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

            if (distance > relocateRange) MonsterOutOfRange?.Invoke(monsters[i]);
        }
    }

    public void Spawn(MonsterType type, Vector3 position)
    {
        if (prefabDic.TryGetValue(type, out MonsterController prefab))
        {
            MonsterController monster = PoolManager.Instance.Get(prefab);
            monster.Init(statRulesData, player, position);
            monster.DeathFinished += HandleDeathMonster;
            monsters.Add(monster);
        }
        else
        {
            Debug.LogError($"[MonsterManager] {type} prefab이 등록되어있지 않습니다.");
        }
    }


    private void SwapMonster(int lastIndex, int i)
    {
        (monsters[lastIndex], monsters[i]) = (monsters[i], monsters[lastIndex]);
    }

    private void RemoveMonster(int removeIndex)
    {
        int lastIndex = monsters.Count - 1;

        SwapMonster(lastIndex, removeIndex);
        monsters.RemoveAt(lastIndex);
    }

    public void Relocate(MonsterController monster, Vector3 position)
    {
        monster.SetPosition(position);
    }

    private void HandleDeathMonster(MonsterController monster)
    {
        monster.DeathFinished -= HandleDeathMonster;
        PoolManager.Instance.Release(monster);
    }
}