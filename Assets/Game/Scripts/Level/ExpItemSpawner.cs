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
        if (player == null) return;
        
        Vector3 playerPos = player.transform.position;
        float pickupRadius = player.PickupRadius;

        float deltaTime = Time.deltaTime;

        for (int i = expItems.Count - 1; i >= 0; i--)
        {
            ExpItem expItem = expItems[i];
            
            if (expItem.State == DropItemState.Idle && (playerPos - expItem.transform.position).sqrMagnitude <= pickupRadius * pickupRadius)
            {
                expItem.SetStatePickUp();
            }

            expItem.Tick(playerPos, deltaTime);

            if (expItem.IsReached)
            {
                int lastIndex = expItems.Count - 1;
                (expItems[i], expItems[lastIndex]) = (expItems[lastIndex], expItems[i]);
                PoolManager.Instance.Release(expItems[lastIndex]);
                expItems.RemoveAt(lastIndex);
            }
        }
    }

    public void Spawn(Vector3 position, int exp)
    {
        ExpItem expItem = PoolManager.Instance.Get(expItemPrefab);
        expItem.Init(position, exp, curve, 1f);
        expItems.Add(expItem);
    }
}