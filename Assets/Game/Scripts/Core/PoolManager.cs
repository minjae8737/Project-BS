using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<GameObject, PrefabPool> poolsByPrefab;
    private Dictionary<Component, PrefabPool> poolsByInstance;

    protected override void Awake()
    {
        base.Awake();

        poolsByPrefab = new();
        poolsByInstance = new();
    }

    public T Get<T>(T prefab) where T : Component
    {
        Assert.IsNotNull(prefab,$"[PoolManager] Get()요청한 prefab이 null 입니다.");

        if (!poolsByPrefab.TryGetValue(prefab.gameObject, out PrefabPool prefabPool))
        {
            prefabPool = new PrefabPool(prefab, transform);
            poolsByPrefab.Add(prefab.gameObject, prefabPool);
        }

        T instance = (T)prefabPool.Get();
        poolsByInstance[instance] = prefabPool;

        return instance;
    }

    public void Release(Component instance)
    {
        Assert.IsNotNull(instance,$"[PoolManager] Release()요청한 instance가 null 입니다.");
        
        if (poolsByInstance.TryGetValue(instance, out PrefabPool prefabPool))
        {
            prefabPool.Release(instance);
        }
        else
        {
            Debug.LogError($"[{this}] {instance.name} 은(는) 등록되지 않은 인스턴스 입니다.", this);
        }
    }
}