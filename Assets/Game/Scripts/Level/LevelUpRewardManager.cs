using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class LevelUpRewardManager
{
    [SerializeField] private LevelUpRewardPanel levelUpRewardPanel;
    [SerializeField] private RewardOptionGenerator rewardOptionGenerator;

    private PlayerController playerController;
    private IReadOnlyPlayerAbilities playerAbilities;
    private CancellationToken token;
    private UniTaskCompletionSource<int> selectionTcs;
    private int pendingRewardCount;
    private bool isRunning;

    public void Init(CancellationToken token, PlayerController playerController)
    {
        this.token = token;
        pendingRewardCount = 0;
        this.playerController = playerController;
        playerAbilities = playerController.Abilities;
        rewardOptionGenerator.Init();
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

                List<RewardOption> rewardOptions = ShowOptions();

                int rewardIndex = await tcs.Task.AttachExternalCancellation(token);

                //reward 적용
                if (rewardOptions.Count > 0) //FIXME 무작위 스텟증가 로직 미처리로 인한 임시 로직
                {
                    playerController.ApplyAbility(rewardOptions[rewardIndex].Data);
                }

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

    private List<RewardOption> CreateRewardOptions()
    {
        if (rewardOptionGenerator.TryCreateRewardOptions(playerAbilities, out var rewardOptions))
        {
            levelUpRewardPanel.ShowRewardOption(rewardOptions);
        }
        else
        {
            //TODO 무작위 스텟증가 로직
            HandleRewardSelected(-1);
        }

        return rewardOptions;
    }

    private List<RewardOption> ShowOptions()
    {
        List<RewardOption> rewardOptions = CreateRewardOptions();

        levelUpRewardPanel.Show();

        return rewardOptions;
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