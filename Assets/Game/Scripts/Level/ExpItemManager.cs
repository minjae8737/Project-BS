using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ExpItemManager : MonoBehaviour
{
    [SerializeField] private ExpItem expItemPrefab;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float duration = 1f;

    private PlayerController playerController;

    private List<ExpItem> expItems;

    private void Awake()
    {
        expItems = new();

        Assert.IsTrue(duration > 0f, "[ExpItemManager] duration 값이 0이하입니다.");
    }

    public void Init(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    private void Update()
    {
        if (playerController == null) return;

        Vector3 playerPos = playerController.Pos;
        Vector3 playerTargetPos = playerController.TargetPos;
        float pickupRadius = playerController.PickupRadius;
        float sqrPickupRadius = pickupRadius * pickupRadius;

        float deltaTime = Time.deltaTime;

        for (int i = expItems.Count - 1; i >= 0; i--)
        {
            ExpItem expItem = expItems[i];

            TryStartAttract(expItem, playerPos, sqrPickupRadius);

            expItem.Tick(playerTargetPos, deltaTime);

            if (expItem.IsArrived)
            {
                Collect(expItem);
                ReleaseAt(i);
            }
        }
    }

    public void Spawn(Vector3 position, int exp)
    {
        ExpItem expItem = PoolManager.Instance.Get(expItemPrefab);
        expItem.Init(position, exp, curve, duration);
        expItems.Add(expItem);
    }

    private static void TryStartAttract(
        ExpItem expItem,
        Vector3 playerPos,
        float sqrPickupRadius)
    {
        if (expItem.State == DropItemState.Idle &&
            (playerPos - expItem.transform.position).sqrMagnitude <= sqrPickupRadius)
        {
            expItem.StartAttract(playerPos);
        }
    }

    private void Collect(ExpItem expItem)
    {
    }

    private void ReleaseAt(int index)
    {
        int lastIndex = expItems.Count - 1;
        (expItems[index], expItems[lastIndex]) = (expItems[lastIndex], expItems[index]);
        PoolManager.Instance.Release(expItems[lastIndex]);
        expItems.RemoveAt(lastIndex);
    }
}