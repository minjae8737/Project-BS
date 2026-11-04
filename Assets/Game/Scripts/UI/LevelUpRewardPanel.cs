using System;
using UnityEngine;

public class LevelUpRewardPanel : MonoBehaviour
{
    public event Action<int> SelectedReward;

    public void OpenReward()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    
    public void OnClickCard(int cardIndex)
    {
        SelectedReward?.Invoke(cardIndex);
    }
}