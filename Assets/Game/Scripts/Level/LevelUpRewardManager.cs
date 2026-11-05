using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class LevelUpRewardManager
{
    [SerializeField] private LevelUpRewardPanel levelUpRewardPanel;

    private CancellationToken token;
    private UniTaskCompletionSource<int> selectionTcs;
    private int pendingRewardCount;
    private bool isRunning;
    
    public void Init(CancellationToken token)
    {
        this.token = token;
        pendingRewardCount = 0;
    }

    public void AddPendingReward()
    {
        pendingRewardCount++;

        if (isRunning) return;

        levelUpRewardPanel.RewardSelected += HandleRewardSelected;
        RewardRoutine(token).Forget();
    }

    private async UniTaskVoid RewardRoutine(CancellationToken token)
    {
        GameManager.Instance.RequestPause();
        isRunning = true;
        
        UniTaskCompletionSource<int> tcs = null;

        try
        {
            while (pendingRewardCount > 0)
            {
                tcs = new UniTaskCompletionSource<int>();
                selectionTcs = tcs;

                CreateRewardOptions();
                ShowOptions();

                int rewardIndex = await tcs.Task.AttachExternalCancellation(token);

                //reward 적용
                Debug.Log($"Apply Reward Index {rewardIndex}");
                pendingRewardCount--;
            }
        }
        finally
        {
            if (selectionTcs == tcs) selectionTcs = null;
            HidePanel();
            isRunning = false;
            if (GameManager.HasInstance)
                GameManager.Instance.RequestResume();
            levelUpRewardPanel.RewardSelected -= HandleRewardSelected;
        }
    }

    private void CreateRewardOptions()
    {
    }

    private void ShowOptions()
    {
        levelUpRewardPanel.Show();
    }

    private void HidePanel()
    {
        levelUpRewardPanel.Hide();
    }

    private void HandleRewardSelected(int rewardIndex)
    {
        selectionTcs?.TrySetResult(rewardIndex);
    }
}