using System.Collections.Generic;
using UnityEngine;

public class ExpItemSpawner : MonoBehaviour
{
    [SerializeField] private ExpItem expItemPrefab;
    [SerializeField] private AnimationCurve curve;

    private PlayerController player;
    
    private List<ExpItem> expItems;

    private void Awake()
    {
        expItems = new();
    }

    public void Init(PlayerController playerController)
    {
        player = playerController;
    }

    private void Update()
    {
        for (int i = expItems.Count - 1; i >= 0; i--)
        {
            expItems[i].Tick();
        }
    }

    public void Spawn(Vector3 position, int exp)
    {
        ExpItem expItem = PoolManager.Instance.Get(expItemPrefab);
        expItem.Init(position, exp);
        expItems.Add(expItem);
    }
}