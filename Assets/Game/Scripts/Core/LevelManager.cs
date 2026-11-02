using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class LevelManager
{
    [SerializeField] private List<RequiredExpSegment> requiredExpSegments;

    private int level;
    private int requiredExp;
    private int currentExp;

    public event Action<int> LeveledUp;

    public void Init()
    {
        ValidateRequiredExpSegments();

        level = 1;
        currentExp = 0;
        requiredExp = CalculateRequiredExp(level);
    }

    private void ValidateRequiredExpSegments()
    {
        int preStartLevel = 0;

        Assert.IsTrue(requiredExpSegments.Count > 0, "[LevelManager] RequiredExpSegment 목록이 비어있습니다.");
        Assert.IsTrue(requiredExpSegments[0].startLevel == 1, "[LevelManager] RequiredExpSegment 첫 StartLevel이 1로 시작하지 않습니다.");

        for (int i = 0; i < requiredExpSegments.Count; i++)
        {
            Assert.IsTrue(requiredExpSegments[i].startLevel > preStartLevel, "[LevelManager] RequiredExpSegment의 StartLevel이 오름차순이 아닙니다.");
            Assert.IsTrue(requiredExpSegments[i].startRequiredExp >= 0, "[LevelManager] RequiredExpSegment의 startRequiredExp값이 0보다 작습니다.");
            Assert.IsTrue(requiredExpSegments[i].increment >= 0, "[LevelManager] RequiredExpSegment의 increment값이 0보다 작습니다.");

            preStartLevel = requiredExpSegments[i].startLevel;
        }
    }

    private int CalculateRequiredExp(int targetLevel)
    {
        int segmentIndex = 0;

        for (int i = 0; i < requiredExpSegments.Count; i++)
        {
            if (requiredExpSegments[i].startLevel > targetLevel) break;
            segmentIndex = i;
        }

        RequiredExpSegment expSegment = requiredExpSegments[segmentIndex];

        return expSegment.startRequiredExp + expSegment.increment * (targetLevel - expSegment.startLevel);
    }

    private bool TryLevelUp()
    {
        if (requiredExp > currentExp) return false;

        currentExp -= requiredExp;
        level++;
        requiredExp = CalculateRequiredExp(level);
        LeveledUp?.Invoke(level);

        return true;
    }

    public void AddExp(int expAmount)
    {
        currentExp += expAmount;
        while (TryLevelUp()) { } // 레벨업이 실패할때까지 반복
    }
}