using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class LevelUpRewardManager
{
    [SerializeField] private LevelUpRewardPanel levelUpRewardPanel;

    private UniTaskCompletionSource<int> selectionTcs;
    private int remainRewardCount;
    private bool ongoing;

    public bool Ongoing => ongoing;

    public void Init()
    {
        levelUpRewardPanel.SelectedReward += HandleSelectedReward;
    }

    public void AddRewardCount()
    {
        remainRewardCount++;

        if (ongoing) return;
        // Routine(token).Forget();
    }

    private async UniTaskVoid Routine(CancellationToken token)
    {
        GameManager.Instance.RequestPause();
        ongoing = true;

        selectionTcs?.TrySetCanceled();
        UniTaskCompletionSource<int> tcs = null;

        try
        {
            while (remainRewardCount > 0)
            {
                tcs = new UniTaskCompletionSource<int>();
                selectionTcs = tcs;

                CreateReward();
                ShowReward();

                await tcs.Task.AttachExternalCancellation(token);

                //reward 적용
                remainRewardCount--;
            }
        }
        finally
        {
            if (selectionTcs == tcs) selectionTcs = null;
            HideReward();
            ongoing = false;
            GameManager.Instance.RequestResume();
        }
    }

    private void CreateReward()
    {
    }

    private void ShowReward()
    {
        levelUpRewardPanel.OpenReward();
    }

    private void HideReward()
    {
        levelUpRewardPanel.ClosePanel();
    }

    private void HandleSelectedReward(int rewardIndex)
    {
        selectionTcs?.TrySetResult(rewardIndex);
    }
}