using System;
using UnityEngine;

public class LevelUpRewardPanel : MonoBehaviour
{
    public event Action<int> RewardSelected;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void OnCardClicked(int cardIndex)
    {
        RewardSelected?.Invoke(cardIndex);
    }
}