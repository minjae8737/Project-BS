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
    [SerializeField] private StatChangeTextGenerator statChangeTextGenerator;

    private PlayerController playerController;
    private CancellationToken token;
    private UniTaskCompletionSource<int> selectionTcs;
    private int pendingRewardCount;
    private bool isRunning;
    private List<StatChange<SkillStatType>> skillStatChanges = new();
    private List<StatChange<StatType>> statChanges = new();

    public void Init(CancellationToken token, PlayerController playerController)
    {
        this.token = token;
        pendingRewardCount = 0;
        this.playerController = playerController;
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

                List<RewardOption> rewardOptions = CreateRewardOptions();
                if (rewardOptions.Count <= 0)
                {
                    Debug.Log("[LevelUpRewardManager] 자동 스텟 상승.");
                    pendingRewardCount -= 1;
                    continue;
                }
                
                FillDescriptions(rewardOptions);
                ShowOptions(rewardOptions);
                
                int rewardIndex = await tcs.Task.AttachExternalCancellation(token);

                //reward 적용
                if (rewardIndex >= 0 && rewardIndex < rewardOptions.Count)
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
        rewardOptionGenerator.TryCreateRewardOptions(playerController.Abilities, out var rewardOptions);
        return rewardOptions;
    }

    private void FillDescriptions(List<RewardOption> rewardOptions)
    {
        foreach (RewardOption rewardOption in rewardOptions)
        {
            if (rewardOption.NextLevel <= 1)
            {
                rewardOption.SetDescription(rewardOption.Data.Description);
            }
            else
            {
                switch (rewardOption.Data)
                {
                    case SkillData skillData: 
                        if (!skillData.TryFillStatChanges(rewardOption.NextLevel, skillStatChanges))
                        {
                            Debug.LogWarning($"[{rewardOption.Data.name}] 기본 설명으로 대체합니다.", rewardOption.Data);
                            rewardOption.SetDescription(skillData.Description);
                            break;
                        }
                        string skillStatText = statChangeTextGenerator.BuildSkillStatText(skillStatChanges);
                        rewardOption.SetDescription(skillStatText);
                        break;
                    case PassiveData passiveData:
                        if (!passiveData.TryFillStatChanges(rewardOption.NextLevel, statChanges, playerController.StatPreviewProvider))
                        {
                            Debug.LogWarning($"[{rewardOption.Data.name}] 기본 설명으로 대체합니다.", rewardOption.Data);
                            rewardOption.SetDescription(passiveData.Description);
                            break;
                        }
                        string statText = statChangeTextGenerator.BuildStatText(statChanges);
                        rewardOption.SetDescription(statText);
                        break;
                    default:
                        Debug.LogError($"[{rewardOption.Data.name}] 잘못된 AbilityType 입니다.", rewardOption.Data);
                        break;
                }
            }
        }
    }

    private void ShowOptions(List<RewardOption> rewardOptions)
    {
        levelUpRewardPanel.ShowRewardOption(rewardOptions);
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