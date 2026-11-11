using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class RewardOptionGenerator
{
    [SerializeField] private AbilityCatalogData catalog;

    private const int MaxOptionCount = 3;
    
    public void Init()
    {
        catalog.ValidateData();
    }

    public bool TryCreateRewardOptions(IReadOnlyPlayerAbilities playerAbilities, out List<RewardOption> rewardOptions)
    {
        List<AbilityData> allCandidates = new();
        List<AbilityData> upgradeCandidates = new();
        List<AbilityData> chosenAbilities = new();
        rewardOptions = new();

        // 후보목록 생성
        foreach (AbilityData abilityData in catalog.Abilities)
        {
            if (playerAbilities.TryGetLevel(abilityData, out int level) && level < AbilityData.MaxLevel)
            {
                allCandidates.Add(abilityData);
                upgradeCandidates.Add(abilityData);
            }
            else if (playerAbilities.HasEmptySlot(abilityData.Type) && level < AbilityData.MaxLevel)
            {
                allCandidates.Add(abilityData);
            }
        }

        if (allCandidates.Count == 0) return false;

        // 가지고있는 어빌리티중 하나 추가
        if (upgradeCandidates.Count > 0)
        {
            int index = Random.Range(0, upgradeCandidates.Count);
            chosenAbilities.Add(upgradeCandidates[index]);
            allCandidates.Remove(upgradeCandidates[index]);
        }

        // 나머지 어빌리티 추가
        while (chosenAbilities.Count < MaxOptionCount && allCandidates.Count > 0)
        {
            int index = Random.Range(0, allCandidates.Count);
            chosenAbilities.Add(allCandidates[index]);
            allCandidates.RemoveAt(index);
        }

        if (chosenAbilities.Count == 0) return false;

        // 선택지 생성
        for (int i = 0; i < chosenAbilities.Count; i++)
        {
            int optionLevel = 1;

            if (playerAbilities.TryGetLevel(chosenAbilities[i], out int level))
            {
                optionLevel = level + 1;
            }

            RewardOption newReward = new RewardOption(chosenAbilities[i], optionLevel);
            rewardOptions.Add(newReward);
        }
        
        return true;
    }

    #region Log

    private static void LogOptions(List<RewardOption> rewardOptions)
    {
        StringBuilder debugMessage = new StringBuilder("[RewardOptionGenerator] 생성된 보상 선택지:");
        for (int i = 0; i < rewardOptions.Count; i++)
        {
            RewardOption option = rewardOptions[i];
            debugMessage.Append($"\n  {i + 1}. {option.Data.DisplayName} (Lv.{option.NextLevel})");
        }

        Debug.Log(debugMessage.ToString());
    }
    
    #endregion
}
