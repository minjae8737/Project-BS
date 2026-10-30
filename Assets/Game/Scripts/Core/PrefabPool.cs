using UnityEngine;
using UnityEngine.Pool;

public class PrefabPool
{
    private ObjectPool<Component> pool;
    private Component prefab;
    private Transform parentRoot;
    
    public PrefabPool(Component prefab, Transform parentRoot)
    {
        this.prefab = prefab;
        this.parentRoot = parentRoot;
     
        pool = new(
            createFunc: Create,
            actionOnGet: ActionOnGet,
            actionOnRelease: ActionOnRelease,
            actionOnDestroy: ActionOnDestroy
            );
    }

    public Component Get()
    {
        return pool.Get();
    }

    public void Release(Component instance)
    {
        pool.Release(instance);
    }

    private Component Create()
    {
        Component instance = GameObject.Instantiate(prefab, parentRoot);
        return instance;
    }

    private void ActionOnDestroy(Component instance)
    {
        GameObject.Destroy(instance.gameObject);
    }

    private void ActionOnGet(Component instance)
    {
        instance.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Component instance)
    {
        instance.gameObject.SetActive(false);
    }
}